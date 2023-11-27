namespace BookShop.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class BookCategory
    {
        [ForeignKey(nameof(Book))]
<<<<<<< HEAD
        public int  BookId { get; set; }
=======
        public int BookId { get; set; }
>>>>>>> 796d2338fed3e69138270bd71d1499939436062a
        public virtual Book Book { get; set; } = null!;

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;
    }
}
