using AutoMapper;
using Company.G02.BLL.Interfaces;
using Company.G02.DAL.Models;
using Company.G02.PL.Helper;
using Company.G02.PL.ViewModels.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
	[Authorize]
	public class EmployeesController : Controller
    {
        private readonly IUnitOfwork _unitOfwork;
        private readonly IMapper _mapper;

        public EmployeesController(
                                    IUnitOfwork unitOfwork,
                                    IMapper mapper)
        {
            
            _unitOfwork = unitOfwork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string InputSearch)
        {
            var employees = Enumerable.Empty<Employee>();
            if (string.IsNullOrEmpty(InputSearch))
            {
                employees = await _unitOfwork.EmployeeRepository.GetAllAsync();

            }

            else
            {
                employees =await _unitOfwork.EmployeeRepository.GetByNameAsync(InputSearch);
            }

            var Result = _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);
   

            return View(Result);
        }
        public async Task<IActionResult> Search(string InputSearch)
        {
            var employees = Enumerable.Empty<Employee>();
            if (string.IsNullOrEmpty(InputSearch))
            {
                employees = await _unitOfwork.EmployeeRepository.GetAllAsync();

            }

            else
            {
                employees = await _unitOfwork.EmployeeRepository.GetByNameAsync(InputSearch);
            }

            var Result = _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);


            return PartialView("PartialViews/EmployeeSearchPartialView", Result);
        }


        [HttpGet]

        public async Task<IActionResult> Create()
        {

            var departments = await _unitOfwork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;
            return View();
        }

        [ValidateAntiForgeryToken]

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.ImageName = DecumentSettings.UplodeFile(model.Image, "Images");
                //var Employee = _mapper.Map<Employee>(model);

                var employee = _mapper.Map<Employee>(model);
                _unitOfwork.EmployeeRepository.Add(employee);
                var count = await _unitOfwork.SaveChangeAsync();
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

        public async Task<IActionResult> Details(int? id, string viewName = "employee")
        {

            if (id is null) return BadRequest();
            var employee =await _unitOfwork.EmployeeRepository.GetAsync(id.Value);

            if (employee is null)
            {
                return NotFound();
            }

            var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);

            return View(employeeViewModel);

        }

        [HttpGet]

        public async Task<IActionResult> Edit(int? id)
        {
            //if (id is null)
            //{
            //    return BadRequest();
            //}

            //var department = _departmentRepository.Get(id.Value);
            //if (department is null)
            //{ return NotFound(); }
            //return View(department);
            try
            {
                var departments = await _unitOfwork.DepartmentRepository.GetAllAsync();
                ViewData["departments"] = departments;

                if (id is null) { return BadRequest(); }
                var employees = await _unitOfwork.EmployeeRepository.GetAsync(id.Value);
                if (employees is null) { return NotFound(); }
                var employeeViewModel = _mapper.Map<EmployeeViewModel>(employees);


                return View(employeeViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeViewModel model, [FromRoute] int id)
        {

            if (model.Id != id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {

               

                try
                {

                    if (model.ImageName is not null)
                    {
                        DecumentSettings.DeleteFile(model.ImageName, "Images");
                    }
                 model.ImageName=   DecumentSettings.UplodeFile(model.Image, "Images");

                    var employee = _mapper.Map<Employee>(model);

                    _unitOfwork.EmployeeRepository.Update(employee);
                    var Count =await _unitOfwork.SaveChangeAsync();
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

            return View(model);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var employee =await _unitOfwork.EmployeeRepository.GetAsync(id.Value);
            if (employee is null)
            { return NotFound(); }


            var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);

            return View(employeeViewModel);


        }



        [ValidateAntiForgeryToken]

        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeViewModel model, [FromRoute] int id)
        {

            try
            {
                if ((id != model.Id))
                {
                    return BadRequest();
                }

                if (ModelState.IsValid)
                {

                    var employee = _mapper.Map<Employee>(model);

                    _unitOfwork.EmployeeRepository.Delete(employee);
                    var Count =await _unitOfwork.SaveChangeAsync();


                    if (Count > 0)
                    {
                        DecumentSettings.DeleteFile(model.ImageName, "Images");
                        return RedirectToAction(nameof(Index)); }

                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(model);
        }
    }
}
