using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G02.DAL.Models
{
    public class Department: BaseEntity
    {
        [Required(ErrorMessage ="Code Is Required")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name Is Required")]

        public string Code { get; set; }

        public string Name { get; set; }

        public DateTime  DateOfCreation { get; set; }


    }
}
