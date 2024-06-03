using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, Department = Dept.HR, Email = "timi@test.com", Name = "Timi"},
                new Employee { Id = 2, Department = Dept.HR, Email = "seun@test.com", Name = "Seun" },
                new Employee { Id = 3, Department = Dept.HR, Email = "funke@test.com", Name = "Titi" }
                );
        }
    }
}
