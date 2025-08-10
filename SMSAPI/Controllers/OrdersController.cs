using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMSAPI.Application.Dtos;
using SMSAPI.Application.Repositories;
using SMSAPI.Domain.Entities;
using SmsWebAPI.Entities;
using System.Net;

namespace SmsWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrdersController : ControllerBase
	{
		private readonly IOrderRepository _orderRepository;
		private readonly UserManager<User> _userManager;
		private readonly IProductOrderRepository _productOrderRepository;
		private readonly IProductRepository _productRepository;
		public OrdersController(IOrderRepository orderRepository, UserManager<User> userManager, IProductRepository productRepository,IProductOrderRepository productOrderRepository)
		{
			_orderRepository = orderRepository;
			_userManager = userManager;
			_productRepository = productRepository;
			_productOrderRepository = productOrderRepository;
		}
		[HttpPost("CreateOrder")]
		public async Task<IActionResult> CreateOrder(OrdersCreateDto ordersCreateDto)
		{
			var orderıd = Guid.NewGuid().ToString();

			var ids = await _userManager.Users.Select(u => u.Id).ToListAsync();
			
			foreach (var id in ids)
			{
				if (id == ordersCreateDto.CustomerId)
				{
					
					await _orderRepository.AddAsync(new()
					{			
						Id = orderıd,
						CreatedDate = DateTime.Now,
						Description = ordersCreateDto.Description,
						CustomerId = new Guid(id),			

					});

					await _productOrderRepository.AddAsync(new()
					{
						ProductId= ordersCreateDto.ProductId,
						OrderId=orderıd, 
						CreatedDate = DateTime.Now,
						Id= Guid.NewGuid().ToString()
						
					});

				}
			}		
			return StatusCode((int)HttpStatusCode.Created);
		}
		[HttpPost("DeleteOrder")]
		public async Task<IActionResult> DeleteOrder(string id)
		{
			await _orderRepository.DeletteIdAsync(id);
			return Ok();

		}

		[HttpGet("GetAllOrder")]
		public async Task<IActionResult> GetAllOrder()
		{
			var orders = _orderRepository.GetAll();
			return Ok(orders);

		}

	}
}