using SMSAPI.Domain.Entities;

namespace SMSAPI.Application.Dtos
{
    public class OrderResponseDto
    {
        public string? Id { get; set; }
        public string? CustomerName { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public List<OrderItemResponseDto> Items { get; set; } = new();
        public decimal TotalPrice => Items.Sum(i => i.TotalPrice);
    }
}
