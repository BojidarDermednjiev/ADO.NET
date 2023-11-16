namespace BookShop.Models
{
    using System.Collections.Generic;

    public class Author
    {
        public Author()
        {
            this.Books = new HashSet<Book>();
        }

        public int AuthorId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

<<<<<<< HEAD
        public virtual ICollection<Book> Books { get; set; }
=======
        public ICollection<Book> Books { get; set; }
>>>>>>> 213b3d268ce47892be7f97dcc60892da38918cb6
    }
}