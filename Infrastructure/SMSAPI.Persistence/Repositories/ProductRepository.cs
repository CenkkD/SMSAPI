using SMSAPI.Application.Repositories;
using SMSAPI.Domain.Entities;
using SMSAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSAPI.Persistence.Repositories
{
	public class ProductRepository:GenericRepository<Product>,IProductRepository
	{
		public ProductRepository(StockDbContext stockDbContext) : base(stockDbContext) 
		{
		
		}

	}
}
