using Company.G02.BLL.Interfaces;
using Company.G02.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public EmployeesController(IEmployeeRepository  employeeRepository , IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }

        public IActionResult Index(string InputSearch)
        {
            var Employees=Enumerable.Empty<Employee>();
            if (string.IsNullOrEmpty(InputSearch))
            {
             Employees = _employeeRepository.GetAll();

            }

            else
            {
                 Employees= _employeeRepository.GetByName(InputSearch);
            }


            //ViewData["Data01"] = "Hello ViewData";

            //ViewBag.Data02 = "Hello ViewBag";
             
            return View(Employees);
        }


        [HttpGet]

        public IActionResult Create()
        {

            var departments=_departmentRepository.GetAll();
            ViewData["departments"] = departments;
            return View();
        }

        [ValidateAntiForgeryToken]

        [HttpPost]
        public IActionResult Create(Employee model)
        {
            if (ModelState.IsValid)
            {
                var count = _employeeRepository.Add(model);
                if (count > 0)
                {
                    TempData["Message"] = "Employee Is Created";
                }
                else
                {
                    TempData["Message"] = "Employee Is Not Created";
                }
                return RedirectToAction("Index");
            }
            return View(model);


        }

        public IActionResult Details(int? id, string viewName = "employee")
        {

            if (id is null) return BadRequest();
            var Employee = _employeeRepository.Get(id.Value);

            if (Employee is null)
            {
                return NotFound();
            }
            return View(Employee);

        }

        [HttpGet]

        public IActionResult Edit(int? id)
        {
            //if (id is null)
            //{
            //    return BadRequest();
            //}

            //var department = _departmentRepository.Get(id.Value);
            //if (department is null)
            //{ return NotFound(); }
            //return View(department);
            var departments = _departmentRepository.GetAll();
            ViewData["departments"] = departments;
            return Details(id, "edit");

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Employee employee, [FromRoute] int id)
        {

            if (employee.Id != id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {

                try
                {
                    var Count = _employeeRepository.Update(employee);
                    if (Count > 0)
                    {
                        TempData["Message"] = "Employee Is Updated";

                    }
                    else
                    {
                        TempData["Message"] = "Employee Is Updated";

                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employee);
        }


        public IActionResult Delete(int? id)
        {
            //if (id is null)
            //{
            //    return BadRequest();
            //}

            //var department = _departmentRepository.Get(id.Value);
            //if (department is null)
            //{ return NotFound(); }
            //return View(department);

            return Details(id, "Delete");

        }

        [ValidateAntiForgeryToken]

        [HttpPost]
        public IActionResult Delete(Employee employee, [FromRoute] int id)
        {
            if ((id != employee.Id))
            {
                return BadRequest();
            }

            try
            {
                _employeeRepository.Delete(employee);
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(employee);
        }
    }
}
