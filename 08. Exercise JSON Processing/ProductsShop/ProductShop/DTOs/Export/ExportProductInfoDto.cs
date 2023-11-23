namespace ProductShop.DTOs.Export
{
    using Newtonsoft.Json;

    public class ExportProductInfoDto
    {
        [JsonProperty("name")]
        public string ProductName { get; set; } = null!;

        [JsonProperty("price")]
        public decimal ProductPrice { get; set; }
    }
}
