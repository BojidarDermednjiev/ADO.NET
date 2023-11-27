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
<<<<<<< HEAD
        
        [Unicode]
        [MaxLength(ValidationConstants.AuthorMaxLengthLastName)]
        public string LastName { get; set; } = null!;

        public virtual ICollection<Book> Books { get; set; }
    }
}
=======

        [Unicode]
        [MaxLength(ValidationConstants.AuthorMaxLengthLastName)]
        public string LastName { get; set; } = null!;
        public virtual ICollection<Book> Books { get; set; } = null!;
    }
}
>>>>>>> 796d2338fed3e69138270bd71d1499939436062a
