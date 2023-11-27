namespace ProductShop.DTOs.Export
{
    using Newtonsoft.Json;
    
    public class ExportSoldProductDto
    {
        [JsonProperty("firstName")]
        public string SellerFirstName { get; set; } = null!;

        [JsonProperty("lastName")]
        public string SellerLastName { get; set; } = null!;

        [JsonProperty("soldProducts")]
        public ICollection<ExportProductDto> SoldProducts { get; set; } = null!;
    }
}
