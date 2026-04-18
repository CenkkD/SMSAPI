using Microsoft.EntityFrameworkCore;
using SMSAPI.Application.Dtos;
using SMSAPI.Application.Repositories;
using SMSAPI.Domain.Entities;
using SMSAPI.Persistence.Contexts;

namespace SMSAPI.Persistence.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly StockDbContext _stockDbContext;

        public OrderRepository(StockDbContext stockDbContext) : base(stockDbContext)
        {
            _stockDbContext = stockDbContext;
        }

        public async Task<bool> CreateOrderWithStockAsync(Order order, OrderItem orderItem, int quantity)
        {
            using var transaction = await _stockDbContext.Database.BeginTransactionAsync();
            try
            {
                if (orderItem.VehicleId is not null)
                {
                    var vehicle = await _stockDbContext.Vehicles.FindAsync(orderItem.VehicleId);
                    if (vehicle == null || vehicle.Stock < quantity)
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }
                    vehicle.Stock -= quantity;
                    vehicle.UpdatedDate = DateTime.Now;
                }
                else if (orderItem.CarPartId is not null)
                {
                    var carPart = await _stockDbContext.CarParts.FindAsync(orderItem.CarPartId);
                    if (carPart == null || carPart.Stock < quantity)
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }
                    carPart.Stock -= quantity;
                    carPart.UpdatedDate = DateTime.Now;
                }
                else
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                await _stockDbContext.Orders.AddAsync(order);
                await _stockDbContext.OrderItems.AddAsync(orderItem);
                await _stockDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Order?> GetOrderWithDetailsAsync(string id)
            => await _stockDbContext.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Vehicle)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.CarPart)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);

        public async Task<PagedResult<Order>> GetOrdersWithDetailsAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var query = _stockDbContext.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Vehicle)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.CarPart)
                .AsNoTracking()
                .Where(o => !o.IsDeleted);

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(o => o.CreatedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Order>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<PagedResult<Order>> GetOrdersByCustomerAsync(string customerId, int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var query = _stockDbContext.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Vehicle)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.CarPart)
                .AsNoTracking()
                .Where(o => o.CustomerId == customerId && !o.IsDeleted);

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(o => o.CreatedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Order>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<bool> CancelOrderAsync(string orderId)
        {
            using var transaction = await _stockDbContext.Database.BeginTransactionAsync();
            try
            {
                var order = await _stockDbContext.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.Id == orderId && !o.IsDeleted);

                if (order is null) return false;

                if (order.Status == OrderStatus.Delivered || order.Status == OrderStatus.Cancelled)
                    return false;

                foreach (var item in order.OrderItems.Where(i => !i.IsDeleted))
                {
                    if (item.VehicleId is not null)
                    {
                        var vehicle = await _stockDbContext.Vehicles.FindAsync(item.VehicleId);
                        if (vehicle is not null)
                        {
                            vehicle.Stock += item.Quantity;
                            vehicle.UpdatedDate = DateTime.Now;
                        }
                    }
                    else if (item.CarPartId is not null)
                    {
                        var carPart = await _stockDbContext.CarParts.FindAsync(item.CarPartId);
                        if (carPart is not null)
                        {
                            carPart.Stock += item.Quantity;
                            carPart.UpdatedDate = DateTime.Now;
                        }
                    }
                }

                order.Status = OrderStatus.Cancelled;
                order.UpdatedDate = DateTime.Now;

                await _stockDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
