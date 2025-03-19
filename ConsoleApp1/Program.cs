using System;
using System.Collections.Generic;

// Interface Segregation: Define a specific interface for salary calculation
interface ISalaryCalculator
{
    decimal CalculateSalary(decimal baseSalary);
}

// Single Responsibility: Each class has a single responsibility
class PermanentEmployeeSalary : ISalaryCalculator
{
    public decimal CalculateSalary(decimal baseSalary) => baseSalary + (baseSalary * 0.2m);
}

class ContractEmployeeSalary : ISalaryCalculator
{
    public decimal CalculateSalary(decimal baseSalary) => baseSalary;
}

// Single Responsibility: Employee class handles employee data and salary calculation
class Employee
{
    public int Id { get; }
    public string Name { get; set; }
    public decimal BaseSalary { get; private set; }
    public decimal CalculatedSalary { get; private set; }
    private ISalaryCalculator _salaryCalculator;

    // Dependency Inversion: Depend on abstractions (ISalaryCalculator) rather than concrete classes
    // Liskov Substitution: Any implementation of ISalaryCalculator can be used here
    public Employee(int id, string name, decimal baseSalary, ISalaryCalculator salaryCalculator)
    {
        Id = id;
        Name = name;
        BaseSalary = baseSalary;
        _salaryCalculator = salaryCalculator;
        CalculatedSalary = _salaryCalculator.CalculateSalary(baseSalary);
    }

    // Open/Closed: Can extend with new salary types without modifying existing code
    public void UpdateSalaryTypeAndBaseSalary(ISalaryCalculator newSalaryCalculator, decimal newBaseSalary)
    {
        _salaryCalculator = newSalaryCalculator;
        BaseSalary = newBaseSalary;
        CalculatedSalary = _salaryCalculator.CalculateSalary(BaseSalary);
        Console.WriteLine($"Salary type and base salary updated for {Name}.\n");
    }

    public void ShowBaseSalary()
    {
        Console.WriteLine($"ID: {Id} | Name: {Name} | Base Salary: ${BaseSalary}\n");
    }

    public void ShowCalculatedSalary()
    {
        Console.WriteLine($"ID: {Id} | Name: {Name} | Calculated Salary: ${CalculatedSalary}\n");
    }
}

// Single Responsibility: Program class handles user interaction and program flow
class Program
{
    static List<Employee> employees = new List<Employee>();
    static int employeeIdCounter = 1;

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n--- Employee Management System ---");
            Console.WriteLine("1. Add Employee");
            Console.WriteLine("2. View Employees");
            Console.WriteLine("3. Update Employee Salary Type and Base Salary");
            Console.WriteLine("4. Delete Employee");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");

            if (!int.TryParse(Console.ReadLine(), out int option) || option < 1 || option > 5)
            {
                Console.WriteLine("Invalid choice. Please enter a number between 1 and 5.");
                continue;
            }

            switch (option)
            {
                case 1:
                    AddEmployee();
                    break;
                case 2:
                    ViewEmployees();
                    break;
                case 3:
                    UpdateEmployee();
                    break;
                case 4:
                    DeleteEmployee();
                    break;
                case 5:
                    Console.WriteLine("Exiting program...");
                    return;
            }
        }
    }

    static void AddEmployee()
    {
        Console.Write("\nEnter employee name: ");
        string name = Console.ReadLine();

        Console.Write("\nEnter base salary: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal baseSalary) || baseSalary <= 0)
        {
            Console.WriteLine("Invalid input. Enter a valid base salary.");
            return;
        }

        Console.WriteLine("Select Employee Type:\n1. Permanent\n2. Contract");
        if (!int.TryParse(Console.ReadLine(), out int choice) || (choice != 1 && choice != 2))
        {
            Console.WriteLine("Invalid choice. Please enter 1 for Permanent or 2 for Contract.");
            return;
        }

        ISalaryCalculator salaryCalculator;
        if (choice == 1)
        {
            salaryCalculator = new PermanentEmployeeSalary();
        }
        else
        {
            salaryCalculator = new ContractEmployeeSalary();
        }

        Employee newEmployee = new Employee(employeeIdCounter++, name, baseSalary, salaryCalculator);
        employees.Add(newEmployee);
        Console.WriteLine("Employee added successfully!\n");
    }

    static void ViewEmployees()
    {
        if (employees.Count == 0)
        {
            Console.WriteLine("\nNo employees found.\n");
            return;
        }

        Console.WriteLine("\n--- Employee List ---");
        foreach (var employee in employees)
        {
            employee.ShowBaseSalary();
        }
    }

    static void UpdateEmployee()
    {
        if (employees.Count == 0)
        {
            Console.WriteLine("\nNo employees found.\n");
            return;
        }

        Console.Write("\nEnter Employee ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int id) || !employees.Exists(e => e.Id == id))
        {
            Console.WriteLine("Invalid ID. Please enter a valid Employee ID.");
            return;
        }

        Employee employee = employees.Find(e => e.Id == id);
        Console.WriteLine($"Updating {employee.Name}'s salary type and base salary...");

        Console.Write("\nEnter new base salary: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal newBaseSalary) || newBaseSalary <= 0)
        {
            Console.WriteLine("Invalid input. Enter a valid base salary.");
            return;
        }

        Console.WriteLine("Select new Employee Type:\n1. Permanent\n2. Contract");
        if (!int.TryParse(Console.ReadLine(), out int choice) || (choice != 1 && choice != 2))
        {
            Console.WriteLine("Invalid choice. Please enter 1 for Permanent or 2 for Contract.");
            return;
        }

        ISalaryCalculator newSalaryCalculator;
        if (choice == 1)
        {
            newSalaryCalculator = new PermanentEmployeeSalary();
        }
        else
        {
            newSalaryCalculator = new ContractEmployeeSalary();
        }

        employee.UpdateSalaryTypeAndBaseSalary(newSalaryCalculator, newBaseSalary);
    }

    static void DeleteEmployee()
    {
        if (employees.Count == 0)
        {
            Console.WriteLine("\nNo employees found.\n");
            return;
        }

        Console.Write("\nEnter Employee ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int id) || !employees.Exists(e => e.Id == id))
        {
            Console.WriteLine("Invalid ID. Please enter a valid Employee ID.");
            return;
        }

        Employee employee = employees.Find(e => e.Id == id);
        employees.Remove(employee);
        Console.WriteLine($"Employee {employee.Name} (ID: {id}) has been deleted.\n");
    }
}

