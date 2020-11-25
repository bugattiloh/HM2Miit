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
            Pr7();
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
            var list = from e in _context.Employees
                       where e.Salary > 48000
                       orderby e
                       select e;

            foreach (var item in list)
            {
                Console.WriteLine($"{item.FirstName},{item.LastName},{item.Salary}");
            }
        }
        private static void Pr2()
        {
            Addresses adr = new Addresses();
            adr.AddressText = "Deulino,30";
            _context.Addresses.Add(adr);

            _context.SaveChanges();

            var list = from e in _context.Employees
                       where e.LastName == "Brown"
                       select e;

            foreach (var item in list)
            {
                item.Address = adr;
            }
            _context.SaveChanges();
        }
        private static void Pr3()
        {
            var list = from p in _context.Projects
                       where p.StartDate.Year >= 2002 && p.StartDate.Year <= 2005
                       select p;

            int k = 0;
            foreach (var item in list)
            {
                if (k == 4)
                {
                    break;
                }
                Console.WriteLine($"{item.Name},{item.StartDate},{item.StartDate}");
                k++;
            }
        }
        private static void Pr4()
        {
            int dbID = int.Parse(Console.ReadLine());

            var employee = from e in _context.Employees
                           where e.EmployeeId == dbID
                           select e;

            var projects = from p in _context.EmployeesProjects
                           where p.EmployeeId == dbID
                           select p.Project;


            foreach (var item in employee)
            {
                Console.WriteLine($"{item.FirstName},{item.LastName},{item.MiddleName}");
            }

            foreach (var item in projects)
            {
                Console.WriteLine($"{item.ProjectId}: {item.Name}");
            }
        }
        private static void Pr5()
        {
            var names = from d in _context.Departments
                        where d.Employees.Count < 5
                        select d;

            foreach (var item in names)
            {
                Console.WriteLine($"{item.Name}");
            }
        }

        private static void Pr6()
        {
            int depId = int.Parse(Console.ReadLine());

            int procent = int.Parse(Console.ReadLine());

            var employees = from d in _context.Departments
                            where d.DepartmentId == depId
                            select d.Employees;

            var ems = employees.SelectMany(e => e);

            foreach (var e in ems)
            {
                e.Salary *= (decimal)(1 + procent / 100f);
            }
            _context.SaveChanges();
        }
        private static void Pr7()
        {
            int depId = int.Parse(Console.ReadLine());

            var deps = from d in _context.Departments
                       where d.DepartmentId == depId
                       select d;

            foreach (var d in deps)
            {
                _context.Departments.Remove(d);
                
            }

            _context.SaveChanges();
        }
        private static void Pr8()
        {
            {
                string townName = Console.ReadLine();

                var towns = from t in _context.Towns
                           where t.Name == townName
                          select t;

                foreach (var t in towns)
                {
                    _context.Towns.Remove(t);

                }

                _context.SaveChanges();
            }
        }


    }
}