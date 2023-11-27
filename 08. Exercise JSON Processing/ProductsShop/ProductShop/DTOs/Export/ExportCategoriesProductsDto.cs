namespace ProductShop.DTOs.Export
{
    using Newtonsoft.Json;

    public class ExportCategoriesProductsDto
    {
        [JsonProperty("category")]
        public string CategoryName { get; set; } = null!;

        [JsonProperty("productsCount")]
        public int  ProductCount { get; set; }

        [JsonProperty("averagePrice")]
        public double AveragePrice { get; set; }

        [JsonProperty("totalRevenue")]
        public decimal TotalRevenue { get; set; }

    }
}
