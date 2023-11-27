namespace BookShop
{
    using Initializer;

    public class StartUp
    {
        static void Main(string[] args)
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);
        }
    }
}
