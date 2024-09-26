using AutoMapper;
using Company.G02.DAL.Models;
using Company.G02.PL.ViewModels;
using Company.G02.PL.ViewModels.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.G02.PL.Controllers
{

    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index(string InputSearch)
        {


            if (string.IsNullOrEmpty(InputSearch))
            {
                var Roles = await _roleManager.Roles.ToListAsync();

                var MappedRole = _mapper.Map<IEnumerable<IdentityRole>, IEnumerable<RoleViewModel>>(Roles);
                return View(MappedRole);
            }

            else
            {

                var Role = await _roleManager.FindByNameAsync(InputSearch);

                var MappedRole = _mapper.Map<IdentityRole, RoleViewModel>(Role);


                return View(new List<RoleViewModel>() { MappedRole });

            }


        }


        public IActionResult Create()
        {
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var MappedRole = _mapper.Map<RoleViewModel, IdentityRole>(model);

                    var role = await _roleManager.CreateAsync(MappedRole);

                    if (role.Succeeded)
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



        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {

            if (id is null) return BadRequest();

            var RoleFromDb = await _roleManager.FindByIdAsync(id);


            if (RoleFromDb is null)
            {
                return NotFound();
            }

            var mappedUser = _mapper.Map<IdentityRole, RoleViewModel>(RoleFromDb);

            return View(viewName, mappedUser);

        }


        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {

            return await Details(id, "Edit");

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleViewModel model, [FromRoute] string id)
        {

            if (model.Id != id)
            {
                return BadRequest();
            }


            if (ModelState.IsValid)
            {

                try
                {
                    var RoleFromDb = await _roleManager.FindByIdAsync(id);

                    if (RoleFromDb is null)
                    {
                        return NotFound();
                    }

                    RoleFromDb.Name = model.RoleName;



                    var result = await _roleManager.UpdateAsync(RoleFromDb);

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

            return await Details(id, "Delete");


        }



        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Delete(RoleViewModel model, [FromRoute] string id)
        {

            if (model.Id != id)
            {
                return BadRequest();
            }


            if (ModelState.IsValid)
            {

                try
                {
                    var RoleFromDb = await _roleManager.FindByIdAsync(id);

                    if (RoleFromDb is null)
                    {
                        return NotFound();
                    }

                    var result = await _roleManager.DeleteAsync(RoleFromDb);

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



        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUser(string roleId)
        {

            var role = await _roleManager.FindByIdAsync(roleId);

            if (role is null)
            {
                NotFound();
            }

            ViewData["RoleId"] = roleId;
            var usersInRole = new List<UsersInRoleViewModel>();

            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userInRole = new UsersInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName

                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRole.IsSelected = true;
                }
                else
                {
                    userInRole.IsSelected = false;

                }

                usersInRole.Add(userInRole);

            }

            return View(usersInRole);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUser(string roleId ,List<UsersInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role is null)
            {
                NotFound();
            }
            if (ModelState.IsValid)
            {

                try
                {
                    foreach (var user in users)
                    {
                        var appUser = await _userManager.FindByIdAsync(user.UserId);
                        if (appUser is not null)
                        {

                            if (user.IsSelected && !await _userManager.IsInRoleAsync(appUser, role.Name))
                            {
                                await _userManager.AddToRoleAsync(appUser, role.Name);
                            }
                            else if (!user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                            {
                                await _userManager.RemoveFromRoleAsync(appUser, role.Name);

                            }
                        }

                    }
                    return RedirectToAction(nameof(Edit), new { id = roleId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }



            }


            return View(users);
        }


    }
}