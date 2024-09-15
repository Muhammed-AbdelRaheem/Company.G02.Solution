using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G02.DAL.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Name Is Required")]
        public string Name { get; set; }
        [Range(25,60,ErrorMessage ="Age Must Be Between 25 and 60")]
        public int? Age { get; set; }

        [RegularExpression("[0-9]{1,5}( [a-zA-Z.]*){1,4},?( [a-zA-Z]*){1,3},? [a-zA-Z]{2},? [0-9]{5}\r\n",ErrorMessage ="address must be like 123-street-city-country -zip")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Salary Is Required")]
        public decimal Salary { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }
        public DateTime HiringDate { get; set; }
        public DateTime DateOfCreation { get; set; }



    }
}
