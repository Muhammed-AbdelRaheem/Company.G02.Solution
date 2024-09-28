using Company.G02.DAL.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Company.G02.PL.ViewModels.Departments
{
    public class DepartmentViewModel
    {
        [Required(ErrorMessage = "Code Is Required")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name Is Required")]

        public string Code { get; set; }


        [DisplayName("DepartmentName")]

        public string Name { get; set; }

        public DateTime DateOfCreation { get; set; }


        public ICollection<Employee>? Employees { get; set; }
    }
}
