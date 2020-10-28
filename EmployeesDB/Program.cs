using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using EmployeesDB.Data.Models;

namespace EmployeesDB
{
    class Program
    {
        static private EmployeesContext _context = new EmployeesContext();

        static void Main(string[] args)
        {
            Pr8();
        }

        static string GetEmployeesInformation()
        {
            var employees = _context.Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle
                })
                .ToList();

            var sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle}");
            }

            return sb.ToString().TrimEnd();
        }

        private static void Pr1()
        {
            var list = _context.Employees.Where(e => e.Salary >= 48000).OrderBy(e => e.LastName).ToList();
            foreach (var item in list)
            {
                Console.WriteLine($"{item.FirstName},{item.LastName},{item.Salary}");
            }
        }
        private static void Pr2()
        {
            var list = _context.Employees.Where(e => e.LastName == "Brown").ToList();
            Addresses addresses = new Addresses();
            addresses.AddressText = "Valovaya,28";
            _context.Addresses.Add(addresses);
            _context.SaveChanges();
            foreach (var item in list)
            {
                item.Address = addresses;
            }
            _context.SaveChanges();
        }
        private static void Pr3()
        {
            var projects = _context.Projects
                .Where(p => p.StartDate.Year >= 2003 && p.StartDate.Year <= 2005)
                .Include(p => p.EmployeesProjects)
                .ThenInclude(p => p.Employee).ThenInclude(p => p.Manager).ToList();

            foreach (var item in projects)
            {
                foreach (var t in item.EmployeesProjects)
                {

                }
            }
        }
        private static void Pr4()
        {
            int dbID = int.Parse(Console.ReadLine());
            var employee = _context.Employees
                .Where(e => e.EmployeeId == dbID)
                .Include(e => e.EmployeesProjects)
                .ThenInclude(e => e.Project)
                .First();

            var projects = employee.EmployeesProjects.Select(p => p.Project).ToList();

            Console.WriteLine($"{employee.FirstName},{employee.LastName},{employee.MiddleName}");
            foreach (var item in projects)
            {
                Console.WriteLine($"{item.ProjectId}: {item.Name}");
            }
        }
        private static void Pr5()
        {
            var names = _context.Departments.Where(d => d.Employees.Count < 5).Select(d => d.Name).ToList();
            Console.WriteLine(String.Join(", ", names));
        }

        private static void Pr6()
        {
            int depId = int.Parse(Console.ReadLine());

            int procent = int.Parse(Console.ReadLine());

            var deps = _context.Departments
                .Where(e => e.DepartmentId == depId)
                .Include(e => e.Employees)
                .First();

            foreach (var it in deps.Employees)
            {
                it.Salary *= (decimal)(1 + procent / 100f);
            }
            _context.SaveChanges();
        }
        private static void Pr7()
        {
            int depId = int.Parse(Console.ReadLine());
            var deps = _context.Departments
                .Where(e => e.DepartmentId == depId)
                .Include(e => e.Employees)
                .First();

            _context.RemoveRange(deps.Employees);
            _context.Remove(deps);
            _context.SaveChanges();
        }
        private static void Pr8()
        {
            string townsName = Console.ReadLine();
            var town = _context.Towns
                .Where(e => e.Name == townsName)
                .Include(e => e.Addresses)
                .First();

            _context.Entry(town).Collection(t => t.Addresses).Load();
            _context.Remove(town);
            _context.SaveChanges();
        }


    }
}