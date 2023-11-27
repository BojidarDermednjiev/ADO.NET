namespace BookShop.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;

    using Common;

    public class Category
    {
        public Category()
        {
            this.BookCategories = new HashSet<BookCategory>();
        }

        [Key]
        public int CategoryId { get; set; }

        [Unicode]
        [MaxLength(ValidationConstants.CategoryNameMaxLength)]
        public string Name { get; set; } = null!;
        public virtual ICollection<BookCategory> BookCategories { get; set; } = null!;

    }
}
