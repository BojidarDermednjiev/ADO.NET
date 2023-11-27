using Microsoft.EntityFrameworkCore;

using BookShop.Common;
using BookShop.Models;

public class BookShopContext : DbContext
{
    public BookShopContext() { }

    public BookShopContext(DbContextOptions options)
        : base(options) { }

    public DbSet<Book> Books { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Author> Authors { get; set; }

    public DbSet<BookCategory> BooksCategories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(Configuration.ConnectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.ApplyConfiguration(new AuthorConfiguration());
        //modelBuilder.ApplyConfiguration(new BookCategoryConfiguration());
        //modelBuilder.ApplyConfiguration(new BookConfiguration());
        //modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.Entity<BookCategory>(entity => { entity.HasKey(ps => new { ps.BookId, ps.CategoryId }); });
    }
}
