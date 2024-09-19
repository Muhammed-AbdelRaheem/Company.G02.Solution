using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    public class DepartmentsController : Controller
    {
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfwork unitOfwork;

        public DepartmentsController(/*IDepartmentRepository departmentRepository*/ IUnitOfwork unitOfwork)
        {
            //_departmentRepository = departmentRepository;
            this.unitOfwork = unitOfwork;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var departments = unitOfwork.DepartmentRepository.GetAll();
            return View(departments);
        }


        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]

        [HttpPost]
        public IActionResult Create(Department model)
        {
            if (ModelState.IsValid)
            {
                 unitOfwork.DepartmentRepository.Add(model);
                var count = unitOfwork.SaveChange();
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);


        }

        public IActionResult Details(int? id , string viewName= "departments")
        {

            if (id is null) return BadRequest();
            var departments = unitOfwork.DepartmentRepository.Get(id.Value);

            if (departments is null)
            {
                return NotFound();
            }
            return View(departments);

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
            return Details(id, "edit");

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Department department, [FromRoute] int id)
        {

            if (department.Id !=id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {

                try
                {
                 unitOfwork.DepartmentRepository.Update(department);
                    var Count = unitOfwork.SaveChange();

                    if (Count >0 )
                    {
                    return RedirectToAction(nameof(Index));

                    }
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
            //if (id is null)
            //{
            //    return BadRequest();
            //}

            //var department = _departmentRepository.Get(id.Value);
            //if (department is null)
            //{ return NotFound(); }
            //return View(department);
            return Details(id,"Delete");

        }

        [ValidateAntiForgeryToken]

        [HttpPost]
        public IActionResult Delete(Department department, [FromRoute] int id)
        {
            if ((id != department.Id))
            {
                return BadRequest();
            }

            try
            {
                unitOfwork.DepartmentRepository.Delete(department);
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
