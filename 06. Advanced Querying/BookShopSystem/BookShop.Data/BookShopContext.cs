<<<<<<< HEAD
﻿using Microsoft.EntityFrameworkCore;

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
=======
﻿namespace BookShop.Data
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
>>>>>>> 796d2338fed3e69138270bd71d1499939436062a
