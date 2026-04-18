using Microsoft.EntityFrameworkCore;
using SMSAPI.Application.Dtos;
using SMSAPI.Application.Repositories;
using SMSAPI.Domain.Entities.Common;
using SMSAPI.Persistence.Contexts;
using System.Linq.Expressions;

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

        public async Task<List<T>> GetAllAsync()
            => await _stockDbContext.Set<T>().AsNoTracking().ToListAsync();

        public async Task<PagedResult<T>> GetPagedAsync(int pageNumber, int pageSize)
            => await GetPagedAsync(null, pageNumber, pageSize);

        public async Task<PagedResult<T>> GetPagedAsync(Expression<Func<T, bool>> filter, int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var query = _stockDbContext.Set<T>().AsNoTracking();
            if (filter != null) query = query.Where(filter);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<T> GetByIdAsync(string id)
            => await Table.AsNoTracking().FirstOrDefaultAsync(data => data.Id == id);

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
            if (id == null) throw new ArgumentNullException(nameof(id));
            var existing = await Table.FindAsync(id);
            if (existing == null) return false;
            entity.Id = id;
            entity.UpdatedDate = DateTime.Now;
            _stockDbContext.Entry(existing).CurrentValues.SetValues(entity);
            await _stockDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletteIdAsync(string id)
        {
            var data = await Table.FindAsync(id);
            if (data == null) return false;
            data.IsDeleted = true;
            data.UpdatedDate = DateTime.Now;
            await _stockDbContext.SaveChangesAsync();
            return true;
        }
    }
}
