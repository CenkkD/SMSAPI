using SMSAPI.Domain.Entities.Common;

namespace SMSAPI.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string? Description { get; set; }
        public string? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public string? CustomerName { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
