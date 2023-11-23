namespace CarDealerSystem.DTOs.Export
{
    public class ExportOrderedCustomersDto
    {
        public string CustomerName { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public bool IsYoungDriver { get; set; }
    }
}
