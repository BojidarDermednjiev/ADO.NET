namespace ProductShop.DTOs.Export
{
    using Newtonsoft.Json;

    public class ExportUserProductDto
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("products")]
        public ICollection<ExportProductInfoDto> Products { get; set; } = null!;
    }
}
