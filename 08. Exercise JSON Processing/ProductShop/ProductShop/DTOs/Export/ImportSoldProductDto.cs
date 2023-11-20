namespace ProductShop.DTOs.Export
{
    using Newtonsoft.Json;

    using Models;

    public class ImportSoldProductDto
    {
        public ImportSoldProductDto()
        {
            this.SoldItems = new HashSet<Product>();
        }
        [JsonProperty("firstName")] public string PersonFirstName { get; set; } = null!;

        [JsonProperty("lastName")] public string PersonLastName { get; set; } = null!;

        [JsonProperty("soldProducts")] public ICollection<Product> SoldItems { get; set; }
    }
}
