using AutoMapper;
using Company.G02.BLL.Interfaces;
using Company.G02.DAL.Models;
using Company.G02.PL.Helper;
using Company.G02.PL.ViewModels.Employees;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    public class EmployeesController : Controller
    {
        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfwork _unitOfwork;
        private readonly IMapper _mapper;

        public EmployeesController(
                                    //IEmployeeRepository employeeRepository,
                                    //IDepartmentRepository departmentRepository,
                                    IUnitOfwork unitOfwork,
                                    IMapper mapper)
        {
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _unitOfwork = unitOfwork;
            _mapper = mapper;
        }

        public IActionResult Index(string InputSearch)
        {
            var employees = Enumerable.Empty<Employee>();
            if (string.IsNullOrEmpty(InputSearch))
            {
                employees = _unitOfwork.EmployeeRepository.GetAll();

            }

            else
            {
                employees = _unitOfwork.EmployeeRepository.GetByName(InputSearch);
            }

            var Result = _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);
            //ViewData["Data01"] = "Hello ViewData";

            //ViewBag.Data02 = "Hello ViewBag";

            return View(Result);
        }


        [HttpGet]

        public IActionResult Create()
        {

            var departments = _unitOfwork.DepartmentRepository.GetAll();
            ViewData["departments"] = departments;
            return View();
        }

        [ValidateAntiForgeryToken]

        [HttpPost]
        public IActionResult Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.ImageName = DecumentSettings.UplodeFile(model.Image, "Images");
                //var Employee = _mapper.Map<Employee>(model);

                var employee = _mapper.Map<Employee>(model);
                _unitOfwork.EmployeeRepository.Add(employee);
                var count = _unitOfwork.SaveChange();
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
            var employee = _unitOfwork.EmployeeRepository.Get(id.Value);

            if (employee is null)
            {
                return NotFound();
            }

            var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);

            return View(employeeViewModel);

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
            var departments = _unitOfwork.DepartmentRepository.GetAll();
            ViewData["departments"] = departments;

            if (id is null) { return BadRequest(); }
            var employees = _unitOfwork.EmployeeRepository.Get(id.Value);
            if (employees is null) { return NotFound(); }
            var employeeViewModel = _mapper.Map<EmployeeViewModel>(employees);


            return Details(id, "edit");

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(EmployeeViewModel model, [FromRoute] int id)
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
                    var Count = _unitOfwork.SaveChange();
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


        public IActionResult Delete(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var employee = _unitOfwork.EmployeeRepository.Get(id.Value);
            if (employee is null)
            { return NotFound(); }


            var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);

            return View(employeeViewModel);


        }



        [ValidateAntiForgeryToken]

        [HttpPost]
        public IActionResult Delete(EmployeeViewModel model, [FromRoute] int id)
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
                    var Count = _unitOfwork.SaveChange();


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
