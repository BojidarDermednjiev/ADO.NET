namespace BookShop.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;

    using Common;

    public class Author
    {
        public Author()
        {
            this.Books = new HashSet<Book>();
        }

        [Key]
        public int AuthorId { get; set; }

        [Unicode]
        [MaxLength(ValidationConstants.AuthorMaxLengthFirstName)]
        public string FirstName { get; set; } = null!;
        
        [Unicode]
        [MaxLength(ValidationConstants.AuthorMaxLengthLastName)]
        public string LastName { get; set; } = null!;

        public virtual ICollection<Book> Books { get; set; }
    }
}
