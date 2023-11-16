<<<<<<< HEAD
﻿using System.Text;
using BookShop.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShop
{
    using System.Diagnostics;

    using Data;
    using Models.Enums;
=======
﻿namespace BookShop
{
    using Data;
    using Initializer;
>>>>>>> 213b3d268ce47892be7f97dcc60892da38918cb6

    public class StartUp
    {
        public static void Main()
        {
<<<<<<< HEAD
            using var dbContext = new BookShopContext();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var output = GetMostRecentBooks(dbContext);
            sw.Stop();
            Console.WriteLine(output);
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            //string[] bookTitles = context.Books
            //    .ToArray()
            //    .Where(b => string.Equals(b.AgeRestriction.ToString(), command, StringComparison.CurrentCultureIgnoreCase))
            //    .OrderBy(b => b.Title)
            //    .Select(b => b.Title)
            //    .ToArray();
            //return string.Join(Environment.NewLine, bookTitles);

            try
            {
                AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(command, true);
                string[] bookTitles = context.Books
                    .Where(b => b.AgeRestriction == ageRestriction)
                    .OrderBy(b => b.Title)
                    .Select(b => b.Title)
                    .ToArray();
                return string.Join(Environment.NewLine, bookTitles);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var bookTitles = context.Books
                .Where(b => b.EditionType == EditionType.Gold
                && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();
            return string.Join(Environment.NewLine, bookTitles);
        }

        //public static string GetBooksByPrice(BookShopContext context)
        //{
        //}

        //public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        //{
        //}

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLower())
                .ToArray();
            var bookTitles = context.Books
                .Where(b => b.BookCategories.Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();
            return string.Join(Environment.NewLine, bookTitles);
        }

        //public static string GetBooksReleasedBefore(BookShopContext context, string date)
        //{
        //}

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authorName = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.LastName)
                .Select(a => $"{a.FirstName} {a.LastName}")
                .ToArray();

            return string.Join(Environment.NewLine, authorName);
        }

        //public static string GetBookTitlesContaining(BookShopContext context, string input)
        //{
        //}

        //public static string GetBooksByAuthor(BookShopContext context, string input)
        //{
        //}

        //public static int CountBooks(BookShopContext context, int lengthCheck)
        //{
        //}

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            //var sb = new StringBuilder();
            var authorsWithBooksCopies = context.Authors
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName,
                    TotalCopies = a.Books
                        .Sum(b => b.Copies)
                })
                .ToArray()
                .OrderByDescending(b => b.TotalCopies);

            //foreach (var a in authorsWithBooksCopies)
            //    sb.AppendLine($"{a.FullName} - {a.TotalCopies}");

            //return sb.ToString().TrimEnd();
            return string.Join(Environment.NewLine,
                authorsWithBooksCopies.Select(a => $"{a.FullName} - {a.TotalCopies}"));
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categoriesWithProfit = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    TotalProfit = c.CategoryBooks.Sum(cb => cb.Book.Copies * cb.Book.Price)
                })
                .ToArray()
                .OrderByDescending(c => c.TotalProfit)
                .ThenBy(c => c.CategoryName);

            return string.Join(Environment.NewLine, categoriesWithProfit.Select(b => $"{b.CategoryName} ${b.TotalProfit:F2}"));
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var categoriesWithMostRecentBooks = context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    CategoryName = c.Name,
                    MostRecentBook = c.CategoryBooks
                        .OrderByDescending(cb => cb.Book.ReleaseDate)
                        //.Take(3)
                        .Select(b => new
                        {
                            BookTitle = b.Book.Title,
                            ReleaseYear = b.Book.ReleaseDate!.Value.Year,
                        })
                        .ToArray()
                })
                .ToArray();

            foreach (var cb in categoriesWithMostRecentBooks)
            {
                sb.AppendLine($"--{cb.CategoryName}");
                foreach (var b in cb.MostRecentBook.Take(3))
                    sb.AppendLine($"{b.BookTitle} ({b.ReleaseYear})");
            }

            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            Book[] booksReleaseBefore2010 = context.Books
                .Where(b => b.ReleaseDate.HasValue && b.ReleaseDate!.Value.Year < 2010)
                .ToArray();

            foreach (var b in booksReleaseBefore2010)
                b.Price += 5;

            context.BulkUpdate(booksReleaseBefore2010);
        }

        //public static int RemoveBooks(BookShopContext context)
        //{
        //}
=======
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);
        }
>>>>>>> 213b3d268ce47892be7f97dcc60892da38918cb6
    }
}


