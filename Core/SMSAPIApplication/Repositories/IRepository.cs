using Microsoft.EntityFrameworkCore;
using SMSAPI.Application.Dtos;
using SMSAPI.Domain.Entities.Common;
using System.Linq.Expressions;

namespace SMSAPI.Application.Repositories
{
	public interface IRepository<T> where T : BaseEntity
	{
		//Read OP
		IQueryable<T> GetAll();
		Task<List<T>> GetAllAsync();
		Task<PagedResult<T>> GetPagedAsync(int pageNumber, int pageSize);
		Task<PagedResult<T>> GetPagedAsync(Expression<Func<T, bool>> filter, int pageNumber, int pageSize);
		IQueryable<T> GetWhere(Expression<Func<T, bool>> entity);
		Task<T> GetSingleAsync(Expression<Func<T, bool>> entity);
		Task<T> GetByIdAsync(string id);

		//Write OP
		Task AddAsync(T entity);

		Task<bool> UpdateAsync(string id, T entity);


		Task<bool> DeletteIdAsync(string id);
		
	}
}