using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SMSAPI.Application.Dtos;
using SMSAPI.Application.Repositories;
using SMSAPI.Domain.Entities;
using SMSAPI.Domain.Entities.Common;
using SMSAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SMSAPI.Persistence.Repositories
{
	public class GenericRepository<T> : IRepository<T> where T : BaseEntity
	{
		private readonly StockDbContext _stockDbContext;

		public GenericRepository(StockDbContext stockDbContext)
		{
			_stockDbContext = stockDbContext;
		}

		public DbSet<T> Table => _stockDbContext.Set<T>();

		public IQueryable<T> GetAll()
		=> _stockDbContext.Set<T>().AsNoTracking();

		public async Task<T> GetByIdAsync(string id)
		=> await Table.AsNoTracking().FirstOrDefaultAsync(data=>data.Id.Contains(id));

		public async Task<T> GetSingleAsync(Expression<Func<T, bool>> entity)
			=> await Table.AsNoTracking().FirstOrDefaultAsync(entity);

		public IQueryable<T> GetWhere(Expression<Func<T, bool>> entity)
		=> Table.AsNoTracking().Where(entity);

		public async Task AddAsync(T entity)
		{
			await _stockDbContext.AddAsync(entity);
			await _stockDbContext.SaveChangesAsync();			 
		}
		public async Task<bool> UpdateAsync(string id, T entity)
		{		
			var exitingproduct = await _stockDbContext.Products.FindAsync(id);		
			if (id == null) throw new ArgumentNullException("id");
			{
				_stockDbContext.Entry(exitingproduct).CurrentValues.SetValues(entity);
				await _stockDbContext.SaveChangesAsync();
				return true;
			}
		}
		public async Task<bool> DeletteIdAsync(string id)
		{
			var data = await GetByIdAsync(id);
			
			_stockDbContext.Set<T>().Remove(data);
			await _stockDbContext.SaveChangesAsync();
			return true;
		}

		
	}




}

