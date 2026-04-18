using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMSAPI.Application.Dtos;
using SMSAPI.Application.Repositories;
using SMSAPI.Domain.Entities;
using System.Security.Claims;

namespace SmsWebAPI.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ICarPartRepository _carPartRepository;
        private readonly ICustomerRepository _customerRepository;

        public OrdersController(IOrderRepository orderRepository, IVehicleRepository vehicleRepository,
            ICarPartRepository carPartRepository, ICustomerRepository customerRepository)
        {
            _orderRepository = orderRepository;
            _vehicleRepository = vehicleRepository;
            _carPartRepository = carPartRepository;
            _customerRepository = customerRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrdersCreateDto dto)
        {
            if (dto is null) return BadRequest("Order data is required.");
            if (string.IsNullOrWhiteSpace(dto.CustomerId)) return BadRequest("Customer id is required.");
            if (string.IsNullOrWhiteSpace(dto.VehicleId) && string.IsNullOrWhiteSpace(dto.CarPartId))
                return BadRequest("Either VehicleId or CarPartId is required.");
            if (!string.IsNullOrWhiteSpace(dto.VehicleId) && !string.IsNullOrWhiteSpace(dto.CarPartId))
                return BadRequest("Only one of VehicleId or CarPartId can be set per order.");
            if (dto.Quantity <= 0) return BadRequest("Quantity must be at least 1.");

            // Customer validated against local StockDB copy — no cross-DB call needed
            var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);
            if (customer is null) return NotFound("Customer not found.");

            if (!string.IsNullOrWhiteSpace(dto.VehicleId))
            {
                var vehicle = await _vehicleRepository.GetByIdAsync(dto.VehicleId);
                if (vehicle is null) return NotFound("Vehicle not found.");
                if (vehicle.Stock < dto.Quantity)
                    return BadRequest($"Insufficient stock. Available: {vehicle.Stock}, Requested: {dto.Quantity}.");
            }
            else
            {
                var carPart = await _carPartRepository.GetByIdAsync(dto.CarPartId!);
                if (carPart is null) return NotFound("Car part not found.");
                if (carPart.Stock < dto.Quantity)
                    return BadRequest($"Insufficient stock. Available: {carPart.Stock}, Requested: {dto.Quantity}.");
            }

            var orderId = Guid.NewGuid().ToString();
            var order = new Order
            {
                Id = orderId,
                CreatedDate = DateTime.Now,
                Description = dto.Description,
                CustomerId = customer.Id,
                CustomerName = $"{customer.FirstName} {customer.LastName}".Trim(),
                Status = OrderStatus.Pending,
            };

            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid().ToString(),
                VehicleId = dto.VehicleId,
                CarPartId = dto.CarPartId,
                OrderId = orderId,
                CreatedDate = DateTime.Now,
                Quantity = dto.Quantity,
            };

            var success = await _orderRepository.CreateOrderWithStockAsync(order, orderItem, dto.Quantity);
            if (!success) return BadRequest("Failed to create order. Stock may have changed.");

            return StatusCode(201);
        }

        [HttpGet]
        [Authorize(Policy = "StockManagerOrAdmin")]
        public async Task<IActionResult> GetAllOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _orderRepository.GetOrdersWithDetailsAsync(pageNumber, pageSize);
            return Ok(new PagedResult<OrderResponseDto>
            {
                Items = result.Items.Select(MapToResponse).ToList(),
                TotalCount = result.TotalCount,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            });
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

            var result = await _orderRepository.GetOrdersByCustomerAsync(userId, pageNumber, pageSize);
            return Ok(new PagedResult<OrderResponseDto>
            {
                Items = result.Items.Select(MapToResponse).ToList(),
                TotalCount = result.TotalCount,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest("Order id is required.");
            var order = await _orderRepository.GetOrderWithDetailsAsync(id);
            if (order is null) return NotFound($"Order with id '{id}' not found.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdminOrManager = User.IsInRole("Admin") || User.IsInRole("StockManager");
            if (!isAdminOrManager && order.CustomerId != userId)
                return Forbid();

            return Ok(MapToResponse(order));
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest("Order id is required.");

            var order = await _orderRepository.GetByIdAsync(id);
            if (order is null) return NotFound($"Order with id '{id}' not found.");
            if (order.Status == OrderStatus.Delivered)
                return BadRequest("Delivered orders cannot be cancelled.");
            if (order.Status == OrderStatus.Cancelled)
                return BadRequest("Order is already cancelled.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdminOrManager = User.IsInRole("Admin") || User.IsInRole("StockManager");
            if (!isAdminOrManager && order.CustomerId != userId)
                return Forbid();

            var success = await _orderRepository.CancelOrderAsync(id);
            if (!success) return BadRequest("Order could not be cancelled.");

            return Ok(new { message = "Order cancelled and stock restored." });
        }

        [HttpPatch("{id}/status")]
        [Authorize(Policy = "StockManagerOrAdmin")]
        public async Task<IActionResult> UpdateOrderStatus(string id, [FromBody] OrderStatus status)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest("Order id is required.");
            if (!Enum.IsDefined(typeof(OrderStatus), status)) return BadRequest("Invalid order status value.");

            var order = await _orderRepository.GetByIdAsync(id);
            if (order is null) return NotFound($"Order with id '{id}' not found.");
            if (order.Status == OrderStatus.Cancelled)
                return BadRequest("Cannot update status of a cancelled order.");

            order.Status = status;
            await _orderRepository.UpdateAsync(id, order);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "StockManagerOrAdmin")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest("Order id is required.");
            var order = await _orderRepository.GetByIdAsync(id);
            if (order is null) return NotFound($"Order with id '{id}' not found.");
            await _orderRepository.DeletteIdAsync(id);
            return Ok();
        }

        private static OrderResponseDto MapToResponse(Order o) => new()
        {
            Id = o.Id,
            CustomerName = o.CustomerName,
            Description = o.Description,
            Status = o.Status.ToString(),
            CreatedDate = o.CreatedDate,
            Items = o.OrderItems?
                .Where(i => !i.IsDeleted)
                .Select(i => new OrderItemResponseDto
                {
                    Id = i.Id,
                    VehicleId = i.VehicleId,
                    VehicleName = i.Vehicle != null ? $"{i.Vehicle.BrandName} {i.Vehicle.ModelName}" : null,
                    CarPartId = i.CarPartId,
                    CarPartName = i.CarPart?.PartName,
                    Quantity = i.Quantity,
                    UnitPrice = i.Vehicle?.Price ?? i.CarPart?.Price ?? 0,
                }).ToList() ?? new()
        };
    }
}
