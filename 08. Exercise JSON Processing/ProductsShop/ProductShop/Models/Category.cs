namespace ProductShop.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Category
    {
        public Category()
        {
            this.CategoriesProducts = new HashSet<CategoryProduct>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<CategoryProduct> CategoriesProducts { get; set; } = null!;
    }
}
