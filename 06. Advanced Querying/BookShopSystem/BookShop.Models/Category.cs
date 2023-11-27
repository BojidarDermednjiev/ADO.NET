namespace BookShop.Models
{
<<<<<<< HEAD
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
=======
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore;
>>>>>>> 796d2338fed3e69138270bd71d1499939436062a

    using Common;

    public class Category
    {
        public Category()
        {
<<<<<<< HEAD
            this.BookCategories = new HashSet<BookCategory>();
=======
            this.CategoryBooks = new HashSet<BookCategory>();
>>>>>>> 796d2338fed3e69138270bd71d1499939436062a
        }

        [Key]
        public int CategoryId { get; set; }

        [Unicode]
<<<<<<< HEAD
        [MaxLength(ValidationConstants.CategoryNameMaxLength)]
        public string Name { get; set; } = null!;
        public virtual ICollection<BookCategory> BookCategories { get; set; } = null!;

=======
        [MaxLength(ValidationConstants.CategoryMaxLengthName)]
        public string Name { get; set; } = null!;
        public ICollection<BookCategory> CategoryBooks { get; set; }
>>>>>>> 796d2338fed3e69138270bd71d1499939436062a
    }
}
