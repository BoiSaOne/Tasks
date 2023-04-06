using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using TaskOne.Models;

namespace TaskOne.DataBase
{
    public static class DbInitializer
    {
        private static string[] names = { "Joe", "Liza", "Gleb", "Alex", "Райн", "Dima", "Denis", "Viktor", "Bob", "Aline", "Misa", "Ilya", "Anna", "Isak" };

        public static void Initialize(AppDbContext context)
        {

            if (context.Employees.Any())
            {
                return;
            }

            var departments = new Department[]
            {
                new Department { Name = "Managers"},
                new Department { Name = "Sales department"},
                new Department { Name = "IT"}
            };

            Random rnd = new Random();

            foreach (var department in departments)
            {
                int countBranches = rnd.Next(1, 4);
                var employees = GetEmployees(countBranches, new List<Employee>(), new List<Employee>(), department);
                context.Employees.AddRange(employees);
            }

            context.SaveChanges();
        }

        private static List<Employee> GetEmployees(int countBranches, List<Employee> employees, List<Employee>? parents, Department department)
        {
            if (countBranches == 0)
            {
                return employees;
            }

            Random rnd = new Random();
            int countEmployees =  rnd.Next(1, 6);
            int salary;
            Employee? parent = null;

            for (int i = 0; i < countEmployees; i++)
            {
                salary = rnd.Next(20_000, 100_000);
                if (parents?.Count != 0)
                {
                    parent = i % 2 == 0 ? parents?[Math.Min((byte)i, (byte) (parents?.Count-1 ?? i))] : null;
                }

                Employee emp = new Employee { Department = department, Salary = salary, Name = names[rnd.Next(0, names.Length-1)], Chief = parent };
                if (i == 0 || i == rnd.Next(1, 8))
                {
                    parents?.Add(emp);
                }
                employees.Add(emp);
            }

            countBranches--;
            return GetEmployees(countBranches, employees, parents, department);
        }
    }
}
