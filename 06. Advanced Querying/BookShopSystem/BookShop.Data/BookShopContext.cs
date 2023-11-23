namespace BookShop.Data
{
    using Microsoft.EntityFrameworkCore;

    using Common;
    using Models;
    public class BookShopContext : DbContext
    {
        public BookShopContext()
        {
        }

        public BookShopContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<BookCategory> BooksCategories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookCategory>(entity => entity.HasKey(ps => new { ps.BookId, ps.CategoryId }));
            //modelBuilder.Entity<Author>(entity => entity.HasKey(e => e.AuthorId));
            //modelBuilder.Entity<Book>(entity => entity.HasKey(e => e.BookId));
            //modelBuilder.Entity<Category>(entity => entity.HasKey(e => e.CategoryId));
        }
    }
}