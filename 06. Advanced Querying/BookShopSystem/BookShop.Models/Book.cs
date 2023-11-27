namespace BookShop.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
<<<<<<< HEAD

    using Enum;
    using Common;

=======
    
    using Enum;
    using Common;
>>>>>>> 796d2338fed3e69138270bd71d1499939436062a
    public class Book
    {
        public Book()
        {
            this.BookCategories = new HashSet<BookCategory>();
        }

        [Key]
        public int BookId { get; set; }

        [Unicode]
<<<<<<< HEAD
        [MaxLength(ValidationConstants.TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Unicode]
        [MaxLength(ValidationConstants.DescriptionMaxLength)]
=======
        [MaxLength(ValidationConstants.BookMaxLengthTitle)]
        public string Title { get; set; } = null!;

        [Unicode]
        [MaxLength(ValidationConstants.BookMaxLengthDescription)]
>>>>>>> 796d2338fed3e69138270bd71d1499939436062a
        public string Description { get; set; } = null!;

        public DateTime? ReleaseDate { get; set; }
        public int Copies { get; set; }
        public decimal Price { get; set; }
        public EditionType EditionType { get; set; }
        public AgeRestriction AgeRestriction { get; set; }
<<<<<<< HEAD
        
=======

>>>>>>> 796d2338fed3e69138270bd71d1499939436062a
        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; } = null!;
        public virtual ICollection<BookCategory> BookCategories { get; set; } = null!;
    }
}
