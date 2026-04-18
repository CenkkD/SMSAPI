namespace SMSAPI.Application.Dtos
{
    public class OrderItemResponseDto
    {
        public string? Id { get; set; }
        public string? VehicleId { get; set; }
        public string? VehicleName { get; set; }
        public string? CarPartId { get; set; }
        public string? CarPartName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
