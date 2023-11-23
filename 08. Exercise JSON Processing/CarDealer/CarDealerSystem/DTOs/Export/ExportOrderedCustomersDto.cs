namespace CarDealerSystem.DTOs.Export
{
    public class ExportOrderedCustomersDto
    {
        public string CustomerName { get; set; } = null!;
        public string BirthDate { get; set; } = null!;
        public bool IsYoungDriver { get; set; }
    }
}
