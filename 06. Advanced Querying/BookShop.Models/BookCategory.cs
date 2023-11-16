namespace BookShop.Models
{
    public class BookCategory
    {
        public int BookId { get; set; }
<<<<<<< HEAD
        public virtual Book Book { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
=======
        public Book Book { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
>>>>>>> 213b3d268ce47892be7f97dcc60892da38918cb6
    }
}
