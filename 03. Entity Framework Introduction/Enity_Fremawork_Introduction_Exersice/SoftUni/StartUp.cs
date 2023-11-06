namespace SoftUni
{
    using System.Globalization;
    using System.Text;
    using Data;
    using Models;
    //using Messages;

    public class StartUp
    {

        static void Main(string[] args)
        {
            SoftUniContext dbContext = new SoftUniContext();
            string output = GetEmployeesInPeriod(dbContext);
            Console.WriteLine(output);
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employees = context.Employees
                 .OrderBy(e => e.EmployeeId)
                 .Select(e => new
                 {
                     e.FirstName,
                     e.MiddleName,
                     e.LastName,
                     e.JobTitle,
                     e.Salary
                 })
                 .ToArray();
            foreach (var employee in employees)
            {
                //sb.AppendLine(string.Format(OutputMessages.EmployeeInfoOutput, employee.FirstName, employee.MiddleName, employee.LastName, employee.JobTitle, employee.Salary));
                sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:F2}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder content = new StringBuilder();
            var employeesRnD = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    DepartmentName = e.Department.Name,
                    e.Salary
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToArray();
            foreach (var e in employeesRnD)
            {
                content.AppendLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary:f2}");
            }
            return content.ToString().TrimEnd();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Address newAdress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };
            Employee? employee = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov");
            //context.Addresses.Add(newAdress);
            employee.Address = newAdress;
            context.SaveChanges();
            string[] employeesAdress = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => e.Address!.AddressText)
                .ToArray();
            return string.Join(Environment.NewLine, employeesAdress);
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employeesWithProjects = context.Employees
                .Where(e => e.EmployeesProjects.Any(ep =>
                    ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                .Take(10)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerFirstName = e.Manager!.FirstName,
                    ManagerLastName = e.Manager!.LastName,
                    Projects = e.EmployeesProjects
                        .Where(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003)
                        .Select(ep => new
                        {
                            ProjectName = ep.Project.Name,
                            StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.CurrentCulture),
                            EndDate = ep.Project.EndDate.HasValue
                                ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                                : "not finished"
                        })
                        .ToArray()
                })
                .ToArray();

            foreach (var e in employeesWithProjects)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");
                foreach (var p in e.Projects)
                    sb.AppendLine($"--{p.ProjectName} - {p.StartDate} - {p.EndDate}");
            }
            return sb.ToString().TrimEnd();
        }
    }
}