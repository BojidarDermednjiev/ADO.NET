namespace DemoVersionOfORM
{
    using Microsoft.EntityFrameworkCore;
    using Data;
    using Models;

    public class StartUp
    {
        static async Task Main(string[] args)
        {
            await using SoftUniContext context = new SoftUniContext();
            await PickUpOldEmployees(context);
            await FindPersonByID(context, 30);
        }
        private static async Task PickUpOldEmployees(SoftUniContext context)
        {
            DateTime date = new DateTime(2000, 1, 1);
            List<Employee> employees = await context.Employees
                 .Where(e => e.HireDate < date)
                 .ToListAsync();
            foreach (var employee in employees)
                Console.WriteLine($"{employee.FirstName} {employee.LastName}");
        }
        private static async Task FindPersonByID(SoftUniContext context, int id)
        {
            var findPerson = await context.Employees.FindAsync(id);
            if (findPerson != null)
                Console.WriteLine($"{findPerson?.FirstName} {findPerson?.LastName}");
        }
    }
}