<<<<<<< HEAD
﻿namespace BookShop.Data
=======
﻿using System.Reflection;

namespace BookShop.Data
>>>>>>> 213b3d268ce47892be7f97dcc60892da38918cb6
{
    using Microsoft.EntityFrameworkCore;

    using Models;
    using EntityConfiguration;
    using System.Collections.Generic;
    using System.Reflection.Emit;
<<<<<<< HEAD
    using System.Reflection;
=======
>>>>>>> 213b3d268ce47892be7f97dcc60892da38918cb6

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
<<<<<<< HEAD
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString)
                    .UseLazyLoadingProxies();
=======
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
>>>>>>> 213b3d268ce47892be7f97dcc60892da38918cb6
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new BookCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        }
    }
}
