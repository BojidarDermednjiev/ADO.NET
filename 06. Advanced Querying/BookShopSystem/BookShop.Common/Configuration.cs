namespace BookShop.Common
{
    public class Configuration
    {
        public static string ConnectionString
            =>
                @"Server=(localdb)\MSSQLLocalDB;Database=FootballBetting;Integrated Security=True;TrustServerCertificate=True";
    }
}
