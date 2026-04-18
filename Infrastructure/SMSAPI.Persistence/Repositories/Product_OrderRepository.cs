using SMSAPI.Application.Repositories;
using SMSAPI.Domain.Entities;
using SMSAPI.Persistence.Contexts;

namespace SMSAPI.Persistence.Repositories
{
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(StockDbContext stockDbContext) : base(stockDbContext)
        {
        }
    }
}
