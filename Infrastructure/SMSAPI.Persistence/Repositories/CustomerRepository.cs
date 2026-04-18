using Microsoft.EntityFrameworkCore;
using SMSAPI.Application.Repositories;
using SMSAPI.Domain.Entities;
using SMSAPI.Persistence.Contexts;

namespace SMSAPI.Persistence.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly StockDbContext _stockDbContext;

        public CustomerRepository(StockDbContext stockDbContext) : base(stockDbContext)
        {
            _stockDbContext = stockDbContext;
        }

        public async Task<Customer?> GetByEmailAsync(string email)
            => await _stockDbContext.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Email == email && !c.IsDeleted);
    }
}
