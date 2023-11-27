namespace BookShop.Initializer.Generators
{
    using Models;

    internal class CategoryGenerator
    {
        internal static Category[] CreateCategories()
        {
            string[] categoryNames = new string[]
            {
                "Science Fiction",
                "Drama",
                "Action",
                "Adventure",
                "Romance",
                "Mystery",
                "Horror",
                "Health",
                "Travel",
                "Children's",
                "Science",
                "History",
                "Poetry",
                "Comics",
                "Art",
                "Cookbooks",
                "Journals",
                "Biographies",
                "Fantasy",
            };

            int categoryCount = categoryNames.Length;

            Category[] categories = new Category[categoryCount];

            for (int currentCategory = 0; currentCategory < categoryCount; currentCategory++)
            {
                Category category = new Category()
                {
                    Name = categoryNames[currentCategory],
                };

                categories[currentCategory] = category;
            }

            return categories;
        }
    }
}