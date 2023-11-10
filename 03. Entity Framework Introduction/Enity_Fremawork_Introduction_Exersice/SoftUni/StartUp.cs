
namespace SoftUni
{
    using System.Globalization;
    using System.Text;
    using Data;
    using Models;
    using Messages;

    public class StartUp
    {

        static void Main(string[] args)
        {
            SoftUniContext dbContext = new SoftUniContext();
            // string theChosen method = MethodName(dbContext)
            Console.WriteLine(/*output the chosen method execution*/);
        }
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
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
            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
                sb.AppendLine(string.Format(OutputMessages.GetEmployeesFullInformation, employee.FirstName, employee.LastName, employee.MiddleName, employee.JobTitle, employee.Salary));

            return sb.ToString().TrimEnd();
        }
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.Salary > 50000)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
                sb.AppendLine(string.Format(OutputMessages.GetEmployeesWithSalaryOver50000, employee.FirstName, employee.Salary));

            return sb.ToString().TrimEnd();
        }
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
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

            StringBuilder content = new StringBuilder();

            foreach (var e in employeesRnD)
                content.AppendLine(string.Format(OutputMessages.GetEmployeesFromResearchAndDevelopment, e.FirstName, e.LastName, e.DepartmentName, e.Salary));

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
                sb.AppendLine(string.Format(OutputMessages.GetEmployeesInPeriodEmployee, e.FirstName, e.LastName, e.ManagerFirstName, e.ManagerLastName));

                foreach (var p in e.Projects)
                    sb.AppendLine(string.Format(OutputMessages.GetEmployeesInPeriodProject, p.ProjectName, p.StartDate, p.EndDate));
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetAddressesByTown(SoftUniContext context)
        {
            string[] adresses = context.Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town!.Name)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .Select(a => string.Format(OutputMessages.GetAddressesByTown, a.AddressText, a.Town!.Name, a.Employees.Count))
                .ToArray();

            return string.Join(Environment.NewLine, adresses);
        }
        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    Project = e.EmployeesProjects
                        .Select(p => new { p.Project.Name })
                        .OrderBy(p => p.Name)
                        .ToArray()
                })
                .FirstOrDefault();

            var sb = new StringBuilder();
            sb.AppendLine(string.Format(OutputMessages.GetEmployee147, employee!.FirstName, employee!.LastName, employee.JobTitle));
            sb.AppendLine(string.Join(Environment.NewLine, employee.Project.Select(p => p.Name)));
            return sb.ToString().TrimEnd();
        }
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var departments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(e => e.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    DepartmentName = d.Name,
                    ManagerName = string.Format(OutputMessages.DepartmentManger, d.Manager.FirstName, d.Manager.LastName),
                    Employees = d.Employees
                        .OrderBy(e => e.FirstName)
                        .ThenBy(e => e.LastName)
                        .Select(e => new
                        {
                            EmployeeData = string.Format(OutputMessages.EmployeeData, e.FirstName, e.LastName, e.Salary)
                        })
                        .ToArray()
                })
                .ToArray();

            foreach (var department in departments)
            {
                sb.AppendLine(string.Format(OutputMessages.GetDepartmentsWithMoreThan5Employees, department.DepartmentName, department.ManagerName));
                sb.Append(string.Join(Environment.NewLine, department.Employees.Select(e => e.EmployeeData)));
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .OrderBy(p => p.Name)
                .Select(p => new
                {
                    ProjectName = p.Name,
                    Description = p.Description,
                    StartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                })
                .ToArray();
            foreach (var p in projects)
            {
                sb.AppendLine(p.ProjectName
                              + Environment.NewLine
                              + p.Description
                              + Environment.NewLine
                              + p.StartDate);
            }
            return sb.ToString().TrimEnd();
        }
        public static string IncreaseSalaries(SoftUniContext context)
        {
            string[] departmentName = new string[] { "Engineering", "Tool Design", "Marketing", "Information Services" };
            var employees = context.Employees
                .Where(e => departmentName.Contains(e.Department.Name))
                .ToArray();
            foreach (var e in employees)
                e.Salary *= 1.12m;
            context.SaveChanges();
            var employeeInfo = context.Employees
                .Where(e => departmentName.Contains(e.Department.Name))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Select(e => string.Format(OutputMessages.IncreaseSalaries, e.FirstName, e.LastName, e.Salary))
                .ToArray();
            return string.Join(Environment.NewLine, employeeInfo);
        }
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Select(e => string.Format(OutputMessages.GetEmployeesByFirstNameStartingWithSa, e.FirstName, e.LastName, e.JobTitle, e.Salary))
                .ToArray();

            return string.Join(Environment.NewLine, employees);
        }
        public static string DeleteProjectById(SoftUniContext context)
        {
            var removeProject = context.Projects.Where(p => p.ProjectId == 2);
            context.Projects.RemoveRange(removeProject);
            var removeEmpProject = context.EmployeesProjects.Where(e => e.ProjectId == 2);
            context.EmployeesProjects.RemoveRange(removeEmpProject);

            context.SaveChanges();


            var projects = context.Projects
                .Take(10)
                .Select(p => p.Name)
                .ToArray();

            return string.Join(Environment.NewLine, projects);
        }
        public static string RemoveTown(SoftUniContext context)
        {
            var townToDelete = context.Towns
                .FirstOrDefault(t => t.Name == "Seattle");
            var addressesToDelete = context.Addresses
                .Where(a => a.TownId == townToDelete!.TownId)
                .ToArray();
            var employees = context.Employees
                .Where(e => addressesToDelete.Contains(e.Address))
                .ToArray();
            foreach (var e in employees)
                e.AddressId = null;
            context.Addresses.RemoveRange(addressesToDelete);
            context.Towns.Remove(townToDelete!);
            context.SaveChanges();

            return string.Format(OutputMessages.RemoveTown, addressesToDelete.Count());

        }
    }
}