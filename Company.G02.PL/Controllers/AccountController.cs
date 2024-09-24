using Company.G02.DAL.Models;
using Company.G02.PL.ViewModels.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _manager;

		public AccountController(UserManager<ApplicationUser> manager)
		{
			_manager = manager;
		}


		#region SignUp


		[HttpGet]
		public IActionResult SignUp()
		{
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var user = await _manager.FindByNameAsync(model.UserName);

					if (user is null)
					{
						user = await _manager.FindByEmailAsync(model.Email);

						if (user is null)
						{
							user = new ApplicationUser()
							{
								UserName = model.UserName,
								Firstname = model.FirstName,
								Lastname = model.LastName,
								Email = model.Email,
								IsAgree = model.IsAgree
							};


							var Result = await _manager.CreateAsync(user, model.Password);

							if (Result.Succeeded)
							{
								return RedirectToAction(nameof(SignIn));
							}
							else
							{
								foreach (var Error in Result.Errors)
								{
									ModelState.AddModelError(string.Empty, Error.Description);
								}
							}
						}

						else
						{
							ModelState.AddModelError(string.Empty, "Email Is Already Exits !!");
						}
						return View();

					}

					else
					{
						ModelState.AddModelError(string.Empty, "UserName Is Already Exits !!");
					}
				}
				catch (Exception ex)
				{

					ModelState.AddModelError(string.Empty, ex.Message);
					;
				}
			}


			return View();
		}


		#endregion

		#region SignIn

		[HttpGet]
		public IActionResult SignIn()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignIn(SingInViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var user = await _manager.FindByEmailAsync(model.Email);

					if (user is not null)
					{
						var password = await _manager.CheckPasswordAsync(user, model.Password);

						if (password)
						{

							return RedirectToAction(nameof(Index), "Home");

						}

						else
						{
							ModelState.AddModelError(string.Empty, "Invalid Login !!");
						}
						return View(model);

					}
					else
					{
						ModelState.AddModelError(string.Empty, "Invalid Login !!");
					}

				}
				catch (Exception ex)
				{

					ModelState.AddModelError (string.Empty, ex.Message);
					
				}
			}
		


			return View(model);
		}




		#endregion


	}
}
