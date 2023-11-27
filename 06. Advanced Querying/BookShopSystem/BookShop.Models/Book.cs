namespace BookShop.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Enum;
    using Common;

    public class Book
    {
        public Book()
        {
            this.BookCategories = new HashSet<BookCategory>();
        }

        [Key]
        public int BookId { get; set; }

        [Unicode]
        [MaxLength(ValidationConstants.TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Unicode]
        [MaxLength(ValidationConstants.DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        public DateTime? ReleaseDate { get; set; }
        public int Copies { get; set; }
        public decimal Price { get; set; }
        public EditionType EditionType { get; set; }
        public AgeRestriction AgeRestriction { get; set; }
        
        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; } = null!;
        public virtual ICollection<BookCategory> BookCategories { get; set; } = null!;
    }
}
