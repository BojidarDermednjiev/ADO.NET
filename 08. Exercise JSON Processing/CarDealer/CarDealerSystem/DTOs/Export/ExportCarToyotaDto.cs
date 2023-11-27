namespace CarDealerSystem.DTOs.Export
{
    public class ExportCarToyotaDto
    {
        public int Id { get; set; }
        public string Make { get; set; } = null!;
        public string Model { get; set; } = null!;
        public long TraveledDistance { get; set; }
    }
}
