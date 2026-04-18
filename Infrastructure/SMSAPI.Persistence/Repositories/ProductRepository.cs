using SMSAPI.Application.Repositories;
using SMSAPI.Domain.Entities;
using SMSAPI.Persistence.Contexts;

namespace SMSAPI.Persistence.Repositories
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(StockDbContext stockDbContext) : base(stockDbContext)
        {
        }
    }
}
