using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentsController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var departments = _departmentRepository.GetAll();
            return View(departments);
        }


        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department model)
        {
            if (ModelState.IsValid)
            {
                var count = _departmentRepository.Add(model);
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);


        }

        public IActionResult Details(int? id)
        {

            if (id is null) return BadRequest();
            var departments = _departmentRepository.Get(id.Value);

            if (departments is null)
            {
                return NotFound();
            }
            return View(departments);

        }

        [HttpGet]

        public IActionResult Edit(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var department = _departmentRepository.Get(id.Value);
            if (department is null)
            { return NotFound(); }
            return View(department);
        }


        [HttpPost]
        public IActionResult Edit(Department department)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    _departmentRepository.Update(department);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(department);
        }


        public IActionResult Delete(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var department = _departmentRepository.Get(id.Value);
            if (department is null)
            { return NotFound(); }
            return View(department);
        }


        [HttpPost]
        public IActionResult Delete(Department department, [FromRoute] int id)
        {
            if ((id != department.Id))
            {
                return BadRequest();
            }

            try
            {
                _departmentRepository.Delete(department);
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(department);
        }

    }

}
