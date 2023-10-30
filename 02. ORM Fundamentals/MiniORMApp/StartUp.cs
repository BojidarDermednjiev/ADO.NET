namespace MiniORMApp
{
    using Data;

    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniDbContext dbContext = new SoftUniDbContext(Config.ConnectionString);
            Console.WriteLine("success");
        }
    }
}