using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMSAPI.Application.Dtos;
using SMSAPI.Application.Repositories;
using SMSAPI.Domain.Entities;
using System.Net;

namespace SmsWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly IProductRepository _productRepository;
		public ProductsController(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}
		[HttpPost("CreateProduct")]
		public async Task<IActionResult> CreateProduct(ProductsCreateDto productCreateDto)
		{
			await _productRepository.AddAsync(new()
			{
				Id = Guid.NewGuid().ToString(),
				BrandName = productCreateDto.BrandName,
				ModelName = productCreateDto.ModelName,
				ModelYear = productCreateDto.ModelYear,
				CreatedDate = DateTime.Now,
				Price = productCreateDto.Price,
				Stock = productCreateDto.Stock,
				HorsePower = productCreateDto.HorsePower,
				Kw = productCreateDto.Kw,
				Torque = productCreateDto.Torque,
				TurboType = productCreateDto.TurboType,
				TractionSystem = productCreateDto.TractionSystem,
				Transmission = productCreateDto.Transmission,
				TopSpeed = productCreateDto.TopSpeed,
				Zero2Hundread = productCreateDto.Zero2Hundread,
				Hundread2TwoHundread = productCreateDto.Hundread2TwoHundread,
				FuelType = productCreateDto.FuelType,
				FuelEffiency = productCreateDto.FuelEffiency,
				ChassisMaterial = productCreateDto.ChassisMaterial,
				CargoSpace = productCreateDto.CargoSpace,
				Doors = productCreateDto.Doors,
				PassengerCapacity = productCreateDto.PassengerCapacity,
				BuildQuality = productCreateDto.BuildQuality,
				Length = productCreateDto.Length,
				Width = productCreateDto.Width,
				Height = productCreateDto.Height,
			});

			return StatusCode((int)HttpStatusCode.Created);
		}
		[HttpPost("DeleteProduct")]
		public async Task<IActionResult> DeleteProduct(string id)
		{
			await _productRepository.DeletteIdAsync(id);
			return Ok();
		}
		[HttpPut("UpdateProduct")]
		public async Task<IActionResult> UpdateProduct(string id, ProductsUpdateDto productsUpdatedDto)
		{
			await _productRepository.UpdateAsync(id, new()
			{
				Id = id,
				BrandName = productsUpdatedDto.BrandName,
				ModelName = productsUpdatedDto.ModelName,
				ModelYear = productsUpdatedDto.ModelYear,
				Price = productsUpdatedDto.Price,
				Stock = productsUpdatedDto.Stock,
				HorsePower = productsUpdatedDto.HorsePower,
				Kw = productsUpdatedDto.Kw,
				Torque = productsUpdatedDto.Torque,
				TurboType = productsUpdatedDto.TurboType,
				TractionSystem = productsUpdatedDto.TractionSystem,
				Transmission = productsUpdatedDto.Transmission,
				TopSpeed = productsUpdatedDto.TopSpeed,
				Zero2Hundread = productsUpdatedDto.Zero2Hundread,
				Hundread2TwoHundread = productsUpdatedDto.Hundread2TwoHundread,
				FuelType = productsUpdatedDto.FuelType,
				FuelEffiency = productsUpdatedDto.FuelEffiency,
				ChassisMaterial = productsUpdatedDto.ChassisMaterial,
				CargoSpace = productsUpdatedDto.CargoSpace,
				Doors = productsUpdatedDto.Doors,
				PassengerCapacity = productsUpdatedDto.PassengerCapacity,
				BuildQuality = productsUpdatedDto.BuildQuality,
				Length = productsUpdatedDto.Length,
				Width = productsUpdatedDto.Width,
				Height = productsUpdatedDto.Height,
				CreatedDate = DateTime.Now,
			});
			return StatusCode((int)HttpStatusCode.OK);
		}
		[HttpGet("GetAllProduct")]
		public async Task<IActionResult> GetAllProduct()
		{

			var products = _productRepository.GetAll();

			return Ok(products);

		}
		[HttpGet("GetByBrandName")]
		public async Task<IActionResult> GetByBrandName(string name, ProductsGetDto productsGetDto)
		{

			var products = await _productRepository.GetWhere(p => p.BrandName.Equals(name));
		

			return Ok(products);




		}
	}
}