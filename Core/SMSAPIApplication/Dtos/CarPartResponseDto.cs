namespace SMSAPI.Application.Dtos
{
    public class CarPartResponseDto
    {
        public string? Id { get; set; }
        public string? PartName { get; set; }
        public string? PartNumber { get; set; }
        public string? Brand { get; set; }
        public string? Category { get; set; }
        public string? CompatibleModels { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Weight { get; set; }
        public string? Description { get; set; }
        public bool IsOriginal { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
