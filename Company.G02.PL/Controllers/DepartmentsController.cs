using AutoMapper;
using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Models;
using Company.G02.PL.ViewModels.Departments;
using Company.G02.PL.ViewModels.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    [Authorize]
    public class DepartmentsController : Controller
    {
        private readonly IUnitOfwork unitOfwork;
        private readonly IMapper _mapper;

        public DepartmentsController(IUnitOfwork unitOfwork,IMapper mapper)
        {
            this.unitOfwork = unitOfwork;
            _mapper = mapper;
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
        public async Task<IActionResult> Create(DepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var department = _mapper.Map<Department>(model);

                unitOfwork.DepartmentRepository.Add(department);
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
            var departmentViewModel = _mapper.Map<DepartmentViewModel>(departments);

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
        public async Task<IActionResult> Edit(DepartmentViewModel model, [FromRoute] int id)
        {

            if (model.Id !=id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {

                try
                {
                    var departments = _mapper.Map<Department>(model);
                     unitOfwork.DepartmentRepository.Update(departments);
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
            return View(model);
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
        public async Task<IActionResult> Delete(DepartmentViewModel model, [FromRoute] int id)
        {
            if ((id != model.Id))
            {
                return BadRequest();
            }

            try
            {
                var departments = _mapper.Map<Department>(model);

                unitOfwork.DepartmentRepository.Delete(departments);
                var Count = await unitOfwork.SaveChangeAsync();

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(model);
        }

    }

}
