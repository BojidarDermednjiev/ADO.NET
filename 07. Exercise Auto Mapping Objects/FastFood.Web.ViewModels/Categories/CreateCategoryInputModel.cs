namespace FastFood.Web.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;
    
    using Common.EntityConfigurations;

    public class CreateCategoryInputModel
    {
        [MinLength(ValidationConstants.CategoryMinLength)]
        [MaxLength(ValidationConstants.CategoryMaxLength)]
        public string CategoryName { get; set; } = null!;
    }
}
