namespace BookShop.Initializer.Generators
{
    using Models;

    internal class AuthorGenerator
    {
        internal static Author[] CreateAuthors()
        {
            string[] authorNames = new string[]
            {
                "Nayden Vitanov",
                "Deyan Tanev",
                "Desislav Petkov",
                "Dyakon Hristov",
                "Milen Todorov",
                "Aleksander Kishishev",
                "Ilian Stoev",
                "Milen Balkanski",
                "Kostadin Nakov",
                "Petar Strashilov",
                "Bozhidara Valova",
                "Lyubina Kostova",
                "Radka Antonova",
                "Vladimira Blagoeva",
                "Bozhidara Rysinova",
                "Borislava Dimitrova",
                "Anelia Velichkova",
                "Violeta Kochanova",
                "Lyubov Ivanova",
                "Blagorodna Dineva",
                "Desislav Bachev",
                "Mihael Borisov",
                "Ventsislav Petrova",
                "Hristo Kirilov",
                "Penko Dachev",
                "Nikolai Zhelyaskov",
                "Petar Tsvetanov",
                "Spas Dimitrov",
                "Stanko Popov",
                "Miro Kochanov",
                "Pesho Stamatov",
                "Roger Porter",
                "Jeffrey Snyder",
                "Louis Coleman",
                "George Powell",
                "Jane Ortiz",
                "Randy Morales",
                "Lisa Davis",

            };

            int authorCount = authorNames.Length;

            Author[] authors = new Author[authorCount];

<<<<<<< HEAD
            for (int i = 0; i < authorCount; i++)
            {
                string[] authorNameTokens = authorNames[i].Split();
=======
            for (int currentAuthor = 0; currentAuthor < authorCount; currentAuthor++)
            {
                string[] authorNameTokens = authorNames[currentAuthor].Split();
>>>>>>> 796d2338fed3e69138270bd71d1499939436062a

                Author author = new Author()
                {
                    FirstName = authorNameTokens[0],
                    LastName = authorNameTokens[1],
                };

<<<<<<< HEAD
                authors[i] = author;
=======
                authors[currentAuthor] = author;
>>>>>>> 796d2338fed3e69138270bd71d1499939436062a
            }

            return authors;
        }
    }
}