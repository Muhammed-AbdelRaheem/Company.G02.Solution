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
        public async Task<IActionResult> Index()
        {
            var departments =await unitOfwork.DepartmentRepository.GetAllAsync();
            return View(departments);
        }


        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]

        [HttpPost]
        public async Task<IActionResult> Create(Department model)
        {
            if (ModelState.IsValid)
            {
                 unitOfwork.DepartmentRepository.Add(model);
                var count =await unitOfwork.SaveChangeAsync();
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);


        }

        public async Task<IActionResult> Details(int? id , string viewName= "departments")
        {

            if (id is null) return BadRequest();
            var departments =await unitOfwork.DepartmentRepository.GetAsync(id.Value);

            if (departments is null)
            {
                return NotFound();
            }
            return View(departments);

        }

        [HttpGet]

        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id is null)
                {
                    return BadRequest();
                }

                var department = await unitOfwork.DepartmentRepository.GetAsync(id.Value);
                if (department is null)
                { return NotFound(); }
                return View(department);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
                    }

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(Department department, [FromRoute] int id)
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
                    var Count =await unitOfwork.SaveChangeAsync();

                    if (Count >0 )
                    {
                    return RedirectToAction(nameof(Index));

                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return RedirectToAction("Error", "Home");
                }
            }
            return View(department);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id is null)
                {
                    return BadRequest();
                }

                var department = await unitOfwork.DepartmentRepository.GetAsync(id.Value);
                if (department is null)
                { return NotFound(); }
                return View(department);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }

        }

        [ValidateAntiForgeryToken]

        [HttpPost]
        public async Task<IActionResult> Delete(Department department, [FromRoute] int id)
        {
            if ((id != department.Id))
            {
                return BadRequest();
            }

            try
            {
                unitOfwork.DepartmentRepository.Delete(department);
                var Count = await unitOfwork.SaveChangeAsync();

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
