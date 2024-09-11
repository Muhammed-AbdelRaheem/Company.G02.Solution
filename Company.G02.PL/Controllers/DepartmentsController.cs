using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentsController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository= departmentRepository;
        }
        public IActionResult Index()
        {
          var departments=  _departmentRepository.GetAll();
            return View(departments);
        }
    }
}
