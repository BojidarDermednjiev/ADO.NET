namespace BookShop
{
<<<<<<< HEAD
    using Initializer;
=======
    using System.Text;

    using Data;
    using Models.Enum;
    using Models;
>>>>>>> 796d2338fed3e69138270bd71d1499939436062a

    public class StartUp
    {
        static void Main(string[] args)
        {
<<<<<<< HEAD
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);
        }
    }
}
=======
            using var DbContext = new BookShopContext();
            //DbInitializer.ResetDatabase(DbContext);
            Console.WriteLine(RemoveBooks(DbContext));
        }
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            try
            {
                AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(command, true);
                string[] books = context.Books
                    .Where(b => b.AgeRestriction == ageRestriction)
                    .OrderBy(b => b.Title)
                    .Select(b => b.Title)
                    .ToArray();
                return string.Join(Environment.NewLine, books);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public static string GetGoldenBooks(BookShopContext context)
        {
            try
            {
                EditionType editionType = Enum.Parse<EditionType>("Gold", true);
                string[] books = context.Books
                    .Where(b => b.EditionType == editionType && b.Copies < 5000)
                    .OrderBy(b => b.BookId)
                    .Select(b => b.Title)
                    .ToArray();
                return string.Join(Environment.NewLine, books);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public static string GetBooksByPrice(BookShopContext context)
        {
            try
            {
                string[] books = context.Books
                    .Where(b => b.Price > 40)
                    .OrderByDescending(b => b.Price)
                    .Select(b => $"{b.Title} - ${b.Price:f2}")
                    .ToArray();
                return string.Join(Environment.NewLine, books);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            try
            {
                string[] books = context.Books
                    .Where(b => b.ReleaseDate!.Value.Year != year && b.ReleaseDate.HasValue)
                    .OrderBy(b => b.BookId)
                    .Select(b => b.Title)
                    .ToArray();
                return string.Join(Environment.NewLine, books);
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            try
            {
                string[] category = input.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();
                string[] books = context.Books
                    .Where(b => b.BookCategories.Any(bc => category.Contains(bc.Category.Name)))//.ToLower()
                    .OrderBy(b => b.Title)
                    .Select(b => b.Title)
                    .ToArray();
                return string.Join(Environment.NewLine, books);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            try
            {
                DateTime parseDateTime = DateTime.ParseExact(date, "dd-MM-yyyy", null);
                string[] books = context.Books
                    .Where(b => b.ReleaseDate.HasValue && b.ReleaseDate < parseDateTime)
                    .OrderByDescending(b => b.ReleaseDate)
                    .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price}")
                    .ToArray();
                return string.Join(Environment.NewLine, books);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            try
            {
                string[] authors = context.Authors
                    .Where(a => a.FirstName.EndsWith(input))
                    .OrderBy(a => a.FirstName)
                    .ThenBy(a => a.LastName)
                    .Select(a => $"{a.FirstName} {a.LastName}")
                    .ToArray();
                return string.Join(Environment.NewLine, authors);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            try
            {
                string[] books = context.Books
                    .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                    .OrderBy(b => b.Title)
                    .Select(b => b.Title)
                    .ToArray();
                return string.Join(Environment.NewLine, books);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            try
            {
                string[] books = context.Books
                    .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                    .OrderBy(b => b.BookId)
                    .Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})")
                    .ToArray();
                return string.Join(Environment.NewLine, books);
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            try
            {
                var books = context.Books
                    .Count(b => b.Title.Length > lengthCheck);
                return books;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                var authorBooksCopies = context.Authors
                    .Select(a => new
                    {
                        AuthorFullName = $"{a.FirstName} {a.LastName}",
                        TotalCopies = a.Books.Sum(b => b.Copies)
                    })
                    .ToArray()
                    .OrderByDescending(b => b.TotalCopies);

                foreach (var a in authorBooksCopies)
                    sb.AppendLine($"{a.AuthorFullName} - {a.TotalCopies}");

                return sb.ToString().TrimEnd();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                var categoriesWithProfit = context.Categories
                    .Select(c => new
                    {
                        CategoryName = c.Name,
                        TotalProfit = c.CategoryBooks.Sum(cb => cb.Book.Price * cb.Book.Copies)
                    })
                    .ToArray()
                    .OrderByDescending(c => c.TotalProfit);
                foreach (var c in categoriesWithProfit)
                {
                    sb.AppendLine($"{c.CategoryName} ${c.TotalProfit:f2}");
                }
                return sb.ToString().TrimEnd();
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }
        public static string GetMostRecentBooks(BookShopContext context)
        {
            try
            {
                StringBuilder sb= new StringBuilder();
                var categories = context.Categories
                    .OrderBy(c => c.Name)
                    .Select(c => new
                    {
                        CategoryName = c.Name,
                        MostRecentBook = c.CategoryBooks
                            .OrderByDescending(cb => cb.Book.ReleaseDate)
                            .Take(3)
                            .Select(cd => new
                            {
                                BookTitle = cd.Book.Title,
                                ReleaseYear = cd.Book.ReleaseDate!.Value.Year
                            })
                            .ToArray()
                    })
                    .ToArray();
                foreach (var c in categories)
                {
                    sb.AppendLine($"--{c.CategoryName}");
                    foreach (var b in c.MostRecentBook)
                        sb.AppendLine($"{b.BookTitle} ({b.ReleaseYear})");
                }

                return sb.ToString().TrimEnd();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public static void IncreasePrices(BookShopContext context)
        {
            Book[] books = context.Books
                .Where(b => b.ReleaseDate.HasValue && b.ReleaseDate.Value.Year < 2010)
                .ToArray();

            foreach (var book in books)
                book.Price += 5;

            //context.SaveChanges();
            context.BulkUpdate(books);
        }
        public static int RemoveBooks(BookShopContext context)
        {
            Book[] books = context.Books
                .Where(b => b.Copies < 4200)
                .ToArray();
            context.BulkDelete(books);
            context.SaveChanges();
            return books.Length;
        }
    }
}
>>>>>>> 796d2338fed3e69138270bd71d1499939436062a
