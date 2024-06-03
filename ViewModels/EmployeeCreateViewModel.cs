using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace EmployeeManagement.ViewModels
{
    public class EmployeeCreateViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(15)]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Office Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter a valid Department")]
        public Dept? Department { get; set; }
        public IFormFile Photo { get; set; }
    }
}
