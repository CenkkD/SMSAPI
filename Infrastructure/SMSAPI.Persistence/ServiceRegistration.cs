
using FluentAssertions.Common;
using IoC;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SMSAPI.Application.Repositories;

using SMSAPI.Persistence.Contexts;
using SMSAPI.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SMSAPI.Persistence
{
	public static class ServiceRegistration
	{
		public static void AddPersistenceServices(this IServiceCollection services)
		{

			services.AddDbContext<StockDbContext>(options => options
			.UseSqlServer(Configuration.ConnectionString));



			services.AddScoped<IProductRepository, ProductRepository>();
			services.AddScoped<IOrderRepository, OrderRepository>();
			services.AddScoped<IProductOrderRepository,Product_OrderRepository>();

			
		}   
	}
}
