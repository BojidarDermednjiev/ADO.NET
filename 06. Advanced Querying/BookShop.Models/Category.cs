namespace BookShop.Models
{
    using System.Collections.Generic;

    public class Category
    {
        public Category()
        {
            this.CategoryBooks = new HashSet<BookCategory>();
        }

        public int CategoryId { get; set; }

        public string Name { get; set; }

<<<<<<< HEAD
        public virtual ICollection<BookCategory> CategoryBooks { get; set; }
=======
        public ICollection<BookCategory> CategoryBooks { get; set; }
>>>>>>> 213b3d268ce47892be7f97dcc60892da38918cb6
    }
}