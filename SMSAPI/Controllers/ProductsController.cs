using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMSAPI.Application.Dtos;
using SMSAPI.Application.Repositories;
using SMSAPI.Domain.Entities;

namespace SmsWebAPI.Controllers
{
    [Route("api/vehicles")]
    [ApiController]
    [Authorize]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehiclesController(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        [HttpPost]
        [Authorize(Policy = "StockManagerOrAdmin")]
        public async Task<IActionResult> CreateVehicle([FromBody] VehicleCreateDto dto)
        {
            if (dto is null) return BadRequest("Vehicle data is required.");
            if (string.IsNullOrWhiteSpace(dto.BrandName)) return BadRequest("Brand name is required.");
            if (string.IsNullOrWhiteSpace(dto.ModelName)) return BadRequest("Model name is required.");
            if (string.IsNullOrWhiteSpace(dto.ModelYear)) return BadRequest("Model year is required.");
            if (dto.Price <= 0) return BadRequest("Price must be greater than zero.");
            if (dto.Stock < 0) return BadRequest("Stock cannot be negative.");

            await _vehicleRepository.AddAsync(new Vehicle
            {
                Id = Guid.NewGuid().ToString(),
                CreatedDate = DateTime.Now,
                BrandName = dto.BrandName,
                ModelName = dto.ModelName,
                ModelYear = dto.ModelYear,
                Price = dto.Price,
                Stock = dto.Stock,
                HorsePower = dto.HorsePower,
                Kw = dto.Kw,
                Torque = dto.Torque,
                TurboType = dto.TurboType,
                TractionSystem = dto.TractionSystem,
                Transmission = dto.Transmission,
                TopSpeed = dto.TopSpeed,
                Zero2Hundread = dto.Zero2Hundread,
                Hundread2TwoHundread = dto.Hundread2TwoHundread,
                FuelType = dto.FuelType,
                FuelEffiency = dto.FuelEffiency,
                ChassisMaterial = dto.ChassisMaterial,
                CargoSpace = dto.CargoSpace,
                Doors = dto.Doors,
                PassengerCapacity = dto.PassengerCapacity,
                BuildQuality = dto.BuildQuality,
                Length = dto.Length,
                Width = dto.Width,
                Height = dto.Height,
            });

            return StatusCode(201);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVehicles(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? brand = null,
            [FromQuery] string? fuelType = null,
            [FromQuery] string? transmission = null,
            [FromQuery] string? modelYear = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null)
        {
            var result = await _vehicleRepository.GetPagedAsync(
                v => (!v.IsDeleted) &&
                     (brand == null || v.BrandName!.ToLower().Contains(brand.ToLower())) &&
                     (fuelType == null || v.FuelType == fuelType) &&
                     (transmission == null || v.Transmission == transmission) &&
                     (modelYear == null || v.ModelYear == modelYear) &&
                     (minPrice == null || v.Price >= minPrice) &&
                     (maxPrice == null || v.Price <= maxPrice),
                pageNumber, pageSize);

            return Ok(new PagedResult<VehicleResponseDto>
            {
                Items = result.Items.Select(MapToResponse).ToList(),
                TotalCount = result.TotalCount,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest("Vehicle id is required.");
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle is null) return NotFound($"Vehicle with id '{id}' not found.");
            return Ok(MapToResponse(vehicle));
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "StockManagerOrAdmin")]
        public async Task<IActionResult> UpdateVehicle(string id, [FromBody] VehicleUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest("Vehicle id is required.");
            if (dto is null) return BadRequest("Vehicle data is required.");
            var existing = await _vehicleRepository.GetByIdAsync(id);
            if (existing is null) return NotFound($"Vehicle with id '{id}' not found.");
            if (dto.Price <= 0) return BadRequest("Price must be greater than zero.");
            if (dto.Stock < 0) return BadRequest("Stock cannot be negative.");

            await _vehicleRepository.UpdateAsync(id, new Vehicle
            {
                Id = id,
                BrandName = dto.BrandName,
                ModelName = dto.ModelName,
                ModelYear = dto.ModelYear,
                Price = dto.Price,
                Stock = dto.Stock,
                HorsePower = dto.HorsePower,
                Kw = dto.Kw,
                Torque = dto.Torque,
                TurboType = dto.TurboType,
                TractionSystem = dto.TractionSystem,
                Transmission = dto.Transmission,
                TopSpeed = dto.TopSpeed,
                Zero2Hundread = dto.Zero2Hundread,
                Hundread2TwoHundread = dto.Hundread2TwoHundread,
                FuelType = dto.FuelType,
                FuelEffiency = dto.FuelEffiency,
                ChassisMaterial = dto.ChassisMaterial,
                CargoSpace = dto.CargoSpace,
                Doors = dto.Doors,
                PassengerCapacity = dto.PassengerCapacity,
                BuildQuality = dto.BuildQuality,
                Length = dto.Length,
                Width = dto.Width,
                Height = dto.Height,
                CreatedDate = existing.CreatedDate,
            });

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "StockManagerOrAdmin")]
        public async Task<IActionResult> DeleteVehicle(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest("Vehicle id is required.");
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle is null) return NotFound($"Vehicle with id '{id}' not found.");
            await _vehicleRepository.DeletteIdAsync(id);
            return Ok();
        }

        private static VehicleResponseDto MapToResponse(Vehicle v) => new()
        {
            Id = v.Id,
            BrandName = v.BrandName,
            ModelName = v.ModelName,
            ModelYear = v.ModelYear,
            Price = v.Price,
            Stock = v.Stock,
            HorsePower = v.HorsePower,
            Kw = v.Kw,
            Torque = v.Torque,
            TurboType = v.TurboType,
            TractionSystem = v.TractionSystem,
            Transmission = v.Transmission,
            TopSpeed = v.TopSpeed,
            Zero2Hundread = v.Zero2Hundread,
            Hundread2TwoHundread = v.Hundread2TwoHundread,
            FuelType = v.FuelType,
            FuelEffiency = v.FuelEffiency,
            ChassisMaterial = v.ChassisMaterial,
            CargoSpace = v.CargoSpace,
            Doors = v.Doors,
            PassengerCapacity = v.PassengerCapacity,
            BuildQuality = v.BuildQuality,
            Length = v.Length,
            Width = v.Width,
            Height = v.Height,
            CreatedDate = v.CreatedDate,
        };
    }
}
