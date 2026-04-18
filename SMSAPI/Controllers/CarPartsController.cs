using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMSAPI.Application.Dtos;
using SMSAPI.Application.Repositories;
using SMSAPI.Domain.Entities;

namespace SmsWebAPI.Controllers
{
    [Route("api/carparts")]
    [ApiController]
    [Authorize]
    public class CarPartsController : ControllerBase
    {
        private readonly ICarPartRepository _carPartRepository;

        public CarPartsController(ICarPartRepository carPartRepository)
        {
            _carPartRepository = carPartRepository;
        }

        [HttpPost]
        [Authorize(Policy = "StockManagerOrAdmin")]
        public async Task<IActionResult> CreateCarPart([FromBody] CarPartCreateDto dto)
        {
            if (dto is null) return BadRequest("Car part data is required.");
            if (string.IsNullOrWhiteSpace(dto.PartName)) return BadRequest("Part name is required.");
            if (string.IsNullOrWhiteSpace(dto.PartNumber)) return BadRequest("Part number is required.");
            if (string.IsNullOrWhiteSpace(dto.Brand)) return BadRequest("Brand is required.");
            if (string.IsNullOrWhiteSpace(dto.Category)) return BadRequest("Category is required.");
            if (dto.Price <= 0) return BadRequest("Price must be greater than zero.");
            if (dto.Stock < 0) return BadRequest("Stock cannot be negative.");

            await _carPartRepository.AddAsync(new CarPart
            {
                Id = Guid.NewGuid().ToString(),
                CreatedDate = DateTime.Now,
                PartName = dto.PartName,
                PartNumber = dto.PartNumber,
                Brand = dto.Brand,
                Category = dto.Category,
                CompatibleModels = dto.CompatibleModels,
                Price = dto.Price,
                Stock = dto.Stock,
                Weight = dto.Weight,
                Description = dto.Description,
                IsOriginal = dto.IsOriginal,
            });

            return StatusCode(201);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCarParts(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? brand = null,
            [FromQuery] string? category = null,
            [FromQuery] bool? isOriginal = null,
            [FromQuery] string? compatibleModel = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null)
        {
            var result = await _carPartRepository.GetPagedAsync(
                p => (!p.IsDeleted) &&
                     (brand == null || p.Brand!.ToLower().Contains(brand.ToLower())) &&
                     (category == null || p.Category == category) &&
                     (isOriginal == null || p.IsOriginal == isOriginal) &&
                     (compatibleModel == null || p.CompatibleModels!.ToLower().Contains(compatibleModel.ToLower())) &&
                     (minPrice == null || p.Price >= minPrice) &&
                     (maxPrice == null || p.Price <= maxPrice),
                pageNumber, pageSize);

            return Ok(new PagedResult<CarPartResponseDto>
            {
                Items = result.Items.Select(MapToResponse).ToList(),
                TotalCount = result.TotalCount,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarPart(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest("Car part id is required.");
            var part = await _carPartRepository.GetByIdAsync(id);
            if (part is null) return NotFound($"Car part with id '{id}' not found.");
            return Ok(MapToResponse(part));
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "StockManagerOrAdmin")]
        public async Task<IActionResult> UpdateCarPart(string id, [FromBody] CarPartUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest("Car part id is required.");
            if (dto is null) return BadRequest("Car part data is required.");
            var existing = await _carPartRepository.GetByIdAsync(id);
            if (existing is null) return NotFound($"Car part with id '{id}' not found.");
            if (dto.Price <= 0) return BadRequest("Price must be greater than zero.");
            if (dto.Stock < 0) return BadRequest("Stock cannot be negative.");

            await _carPartRepository.UpdateAsync(id, new CarPart
            {
                Id = id,
                PartName = dto.PartName,
                PartNumber = dto.PartNumber,
                Brand = dto.Brand,
                Category = dto.Category,
                CompatibleModels = dto.CompatibleModels,
                Price = dto.Price,
                Stock = dto.Stock,
                Weight = dto.Weight,
                Description = dto.Description,
                IsOriginal = dto.IsOriginal,
                CreatedDate = existing.CreatedDate,
            });

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "StockManagerOrAdmin")]
        public async Task<IActionResult> DeleteCarPart(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest("Car part id is required.");
            var part = await _carPartRepository.GetByIdAsync(id);
            if (part is null) return NotFound($"Car part with id '{id}' not found.");
            await _carPartRepository.DeletteIdAsync(id);
            return Ok();
        }

        private static CarPartResponseDto MapToResponse(CarPart p) => new()
        {
            Id = p.Id,
            PartName = p.PartName,
            PartNumber = p.PartNumber,
            Brand = p.Brand,
            Category = p.Category,
            CompatibleModels = p.CompatibleModels,
            Price = p.Price,
            Stock = p.Stock,
            Weight = p.Weight,
            Description = p.Description,
            IsOriginal = p.IsOriginal,
            CreatedDate = p.CreatedDate,
        };
    }
}
