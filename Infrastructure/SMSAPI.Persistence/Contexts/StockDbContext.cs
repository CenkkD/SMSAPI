using Microsoft.EntityFrameworkCore;
using SMSAPI.Domain.Entities;
using SMSAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SMSAPI.Persistence.Contexts
{
	public class StockDbContext : DbContext
	{
		public StockDbContext(DbContextOptions<StockDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Product_Order>().HasKey(po => new
			{
				po.ProductId,
				po.OrderId,
			});

			modelBuilder.Entity<Product_Order>().HasOne(po => po.Product).WithMany(po=>po.Product_Orders).HasForeignKey(po=>po.ProductId);

			modelBuilder.Entity<Product_Order>().HasOne(po => po.Order).WithMany(po=>po.Product_Orders).HasForeignKey(po=>po.OrderId);

			base.OnModelCreating(modelBuilder);
		}





		public DbSet<Product> Products { get; set; }
		public DbSet<Order> Orders { get; set; }
		
		public DbSet<Product_Order> Products_Orders { get; set; }
	}
}
