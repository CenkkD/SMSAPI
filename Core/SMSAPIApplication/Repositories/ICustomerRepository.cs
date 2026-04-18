using SMSAPI.Domain.Entities;

namespace SMSAPI.Application.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer?> GetByEmailAsync(string email);
    }
}
