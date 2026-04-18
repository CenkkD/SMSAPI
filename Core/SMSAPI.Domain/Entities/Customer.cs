using SMSAPI.Domain.Entities.Common;

namespace SMSAPI.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
