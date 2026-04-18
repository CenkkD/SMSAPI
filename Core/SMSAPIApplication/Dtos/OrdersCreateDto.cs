namespace SMSAPI.Application.Dtos
{
    public class OrdersCreateDto
    {
        public string? CustomerId { get; set; }
        public string? VehicleId { get; set; }
        public string? CarPartId { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
