using AutoMapper;
using Company.G02.DAL.Models;
using Company.G02.PL.Helper;
using Company.G02.PL.ViewModels.Employees;
using Company.G02.PL.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.G02.PL.Controllers
{
    [Authorize(Roles ="Admin")]

    public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager, IMapper mapper)
		{
			_userManager = userManager;
            _mapper = mapper;
        }



		public async Task<IActionResult> Index(string InputSearch)
		{
			var users = Enumerable.Empty<UserViewModel>();

			if (string.IsNullOrEmpty(InputSearch))
			{
				users = await _userManager.Users.Select(U => new UserViewModel()
				{
					Id = U.Id,
					FirstName = U.Firstname,
					LastName = U.Lastname,
					Email = U.Email,
                    PhoneNumber= U.PhoneNumber,
					Roles = _userManager.GetRolesAsync(U).GetAwaiter().GetResult()
				}
				).ToListAsync();
			}

			else
			{

				users = await _userManager.Users.Where(U => U.Email.
									ToLower().
									Contains(InputSearch.ToLower())).
						Select(U => new UserViewModel()
						{
							Id = U.Id,
							FirstName = U.Firstname,
							LastName = U.Lastname,
							Email = U.Email,
							Roles = _userManager.GetRolesAsync(U).GetAwaiter().GetResult()
						}).ToListAsync();
			}



			return View(users);
		}


        public async Task<IActionResult> Search(string InputSearch)
        {
            var users = Enumerable.Empty<UserViewModel>();

            if (string.IsNullOrEmpty(InputSearch))
            {
                users = await _userManager.Users.Select(U => new UserViewModel()
                {
                    Id = U.Id,
                    FirstName = U.Firstname,
                    LastName = U.Lastname,
                    Email = U.Email,
                    PhoneNumber=U.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(U).GetAwaiter().GetResult()
                }
                ).ToListAsync();
            }

            else
            {

                users = await _userManager.Users.Where(U => U.Firstname.
                                    ToLower().
                                    Contains(InputSearch.ToLower())).
                        Select(U => new UserViewModel()
                        {
                            Id = U.Id,
                            FirstName = U.Firstname,
                            LastName = U.Lastname,
                            Email = U.Email,
                            PhoneNumber=U.PhoneNumber,
                            Roles = _userManager.GetRolesAsync(U).GetAwaiter().GetResult()
                        }).ToListAsync();
            }



            return PartialView("UserPartialViews/UserSearchPartialView", users);
        }



        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {

            if (id is null) return BadRequest();

			var userFromDb =	await _userManager.FindByIdAsync(id);


            if (userFromDb is null)
            {
                return NotFound();
            }

			var mappedUser=_mapper.Map<ApplicationUser,UserViewModel>(userFromDb);  

            return View(viewName ,mappedUser );

        }


        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {

            return await Details(id, "Edit");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel model, [FromRoute] string id)
        {

            if (model.Id != id) 
            {
                return BadRequest();
            }


            if (ModelState.IsValid)
            {

                try
                {
                    var userFromDb = await _userManager.FindByIdAsync(id);

                    if (userFromDb is null)
                    {
                        return NotFound();
                    }

                    userFromDb.Firstname = model.FirstName;
                    userFromDb.Lastname = model.LastName;
                    userFromDb.Email = model.Email;
                    userFromDb.PhoneNumber = model.PhoneNumber;

                    var result = await _userManager.UpdateAsync(userFromDb);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                    
                }
         
            }

            return View(model);
        }



        public async Task<IActionResult> Delete(string? id)
        {

            return await Details (id,"Delete");


        }



        [ValidateAntiForgeryToken]

        [HttpPost]
        public async Task<IActionResult> Delete(UserViewModel model, [FromRoute] string id)
        {

            if (model.Id != id)
            {
                return BadRequest();
            }
                

            if (ModelState.IsValid)
            {

                try
                {
                    var userFromDb = await _userManager.FindByIdAsync(id);

                    if (userFromDb is null)
                    {
                        return NotFound();
                    }

                    var result = await _userManager.DeleteAsync(userFromDb);

                    if (result.Succeeded)
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

    }
}
