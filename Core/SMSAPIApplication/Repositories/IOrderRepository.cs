using SMSAPI.Application.Dtos;
using SMSAPI.Domain.Entities;

namespace SMSAPI.Application.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<bool> CreateOrderWithStockAsync(Order order, OrderItem orderItem, int quantity);
        Task<Order?> GetOrderWithDetailsAsync(string id);
        Task<PagedResult<Order>> GetOrdersWithDetailsAsync(int pageNumber, int pageSize);
        Task<PagedResult<Order>> GetOrdersByCustomerAsync(string customerId, int pageNumber, int pageSize);
        Task<bool> CancelOrderAsync(string orderId);
    }
}
