using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employees;
        public MockEmployeeRepository()
        {
            _employees = new List<Employee>
                {
                    new Employee{Id =1, Name = "Folashade", Department = Dept.HR, Email = "mary@test.com" },
                    new Employee{Id =2, Name = "James", Department = Dept.IT, Email = "james@test.com" },
                    new Employee{Id =3, Name = "Doe", Department = Dept.Payroll, Email = "doe@test.com" }
                };
        }

        public Employee AddEmployee(Employee employee)
        {
            employee.Id = _employees.Max(e => e.Id) + 1;
            _employees.Add(employee);
            return employee;
        }

        public Employee DeleteEmployee(int id)
        {
            Employee employee = _employees.FirstOrDefault(e => e.Id == id);
            if (employee != null)
            {
                _employees.Remove(employee);
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employees;
        }

        public Employee GetEmployee(int id)
        {
            return _employees.FirstOrDefault(e => e.Id == id);
        }

        public Employee UpdateEmployee(Employee employeeChanges)
        {
            Employee employee = _employees.FirstOrDefault(e => e.Id == employeeChanges.Id);
            if (employee != null)
            {
                employee.Name = employee.Name;
                employee.Department = employee.Department;
                employee.Email = employee.Email;
            }
            return employee;
        }
    }
}
