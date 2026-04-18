using SMSAPI.Application.Repositories;
using SMSAPI.Domain.Entities;
using SMSAPI.Persistence.Contexts;

namespace SMSAPI.Persistence.Repositories
{
    public class CarPartRepository : GenericRepository<CarPart>, ICarPartRepository
    {
        public CarPartRepository(StockDbContext stockDbContext) : base(stockDbContext)
        {
        }
    }
}
