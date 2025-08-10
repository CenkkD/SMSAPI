using Microsoft.EntityFrameworkCore;
using SMSAPI.Domain.Entities.Common;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMSAPI.Application.Dtos;

namespace SMSAPI.Application.Repositories
{
	public interface IRepository<T> where T : BaseEntity
	{
		//Read OP
		IQueryable<T> GetAll();
		IQueryable<T> GetWhere(Expression<Func<T, bool>> entity);
		Task<T> GetSingleAsync(Expression<Func<T, bool>> entity);
		Task<T> GetByIdAsync(string id);

		//Write OP
		Task AddAsync(T entity);

		Task<bool> UpdateAsync(string id, T entity);


		Task<bool> DeletteIdAsync(string id);
		
	}
}