namespace ProductShop.DTOs.Import
{
    using Newtonsoft.Json;

    public class ImportCategoriesDto
    {
        [JsonProperty("name")] 
        public string? Name { get; set; }
    }
}
