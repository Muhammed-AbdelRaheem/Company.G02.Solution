using Company.G02.DAL.Models;
using Company.G02.PL.Helper;
using Company.G02.PL.ViewModels.Auth;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _manager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> manager, SignInManager<ApplicationUser> SignInManager)
        {
            _manager = manager;
            _signInManager = SignInManager;
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

                            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                            if (result.Succeeded)
                            {
                                return RedirectToAction(nameof(Index), "Home");

                            }

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

                    ModelState.AddModelError(string.Empty, ex.Message);

                }
            }



            return View(model);
        }


		public IActionResult GoogleLogin()
		{
            var prop = new AuthenticationProperties()
			{
				RedirectUri = Url.Action(nameof(GoogleResponse))
			};
			return Challenge(prop, GoogleDefaults.AuthenticationScheme);
		}


        public async Task<IActionResult> GoogleResponse()
		{
			var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
			var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(

				claim => new
				{
					claim.Issuer,
					claim.OriginalIssuer,
					claim.Type,
					claim.Value
				}
				);
			return RedirectToAction("Index", "Home");
		}
		#endregion



		#region SignOut

		public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }



        #endregion



        #region Reset Password



        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }




        [HttpPost]

        public async Task<IActionResult> SendResetPassword(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _manager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var token = await _manager.GeneratePasswordResetTokenAsync(user);

                    var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);

                    var email = new Email
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = url

                    };

                    EmailSettings.SendEmail(email);
                    ////////////////////// Send Email here

                    return RedirectToAction(nameof(CheckYourEmail));

                }
                else { ModelState.AddModelError(string.Empty, "invalid Operation , Try Again !!"); }

            }


            return View("ForgetPassword", model);

        }


        [HttpGet]
        public IActionResult CheckYourEmail()
        {
            return View();
        }




        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;



            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {

                    var email = TempData["email"] as string;
                    var token = TempData["token"] as string;

                    var user = await _manager.FindByEmailAsync(email);

                    if (user is not null)
                    {
                        var result = await _manager.ResetPasswordAsync(user, token, model.Password);

                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(SignIn));
                        }
                    }


                    ModelState.AddModelError(string.Empty, "invalid Operation , Try Again !!");
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }



            return View(model);
        }


        #endregion


        public IActionResult AccessDenied()
        {

            return View();
        }






    }
}
