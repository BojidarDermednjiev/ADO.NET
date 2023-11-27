namespace ProductShop.DTOs.Export
{
    using Newtonsoft.Json;
    public class ExportUserDto
    {
        [JsonProperty("lastName")]
        public string LastName { get; set; } = null!;

        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("soldProducts")]
        public ExportUserProductDto SoldProducts { get; set; } = null!;
    }
}
