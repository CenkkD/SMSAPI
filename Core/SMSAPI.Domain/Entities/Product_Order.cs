using SMSAPI.Domain.Entities.Common;

namespace SMSAPI.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public string? VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        public string? CarPartId { get; set; }
        public CarPart? CarPart { get; set; }

        public string OrderId { get; set; }
        public Order Order { get; set; }

        public int Quantity { get; set; } = 1;
    }
}
