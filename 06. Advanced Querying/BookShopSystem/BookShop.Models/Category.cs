namespace BookShop.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore;

    using Common;

    public class Category
    {
        public Category()
        {
            this.CategoryBooks = new HashSet<BookCategory>();
        }

        [Key]
        public int CategoryId { get; set; }

        [Unicode]
        [MaxLength(ValidationConstants.CategoryMaxLengthName)]
        public string Name { get; set; } = null!;
        public ICollection<BookCategory> CategoryBooks { get; set; }
    }
}
