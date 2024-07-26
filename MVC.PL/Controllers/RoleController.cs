using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.DAL.Models;
using MVC.PL.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {


        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        //inject from user manager
        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }



        public async Task<IActionResult> Index(string SearchInput)
        {
            var users = Enumerable.Empty<RoleViewModel>();

            if (string.IsNullOrEmpty(SearchInput))
            {
                users = await _roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName = R.Name,
                }).ToListAsync();

            }
            else
            {
                users = await _roleManager.Roles.Where(U => U.Name.ToLower()
                                                .Contains(SearchInput.ToLower()))
                                                .Select(R => new RoleViewModel()
                                                {
                                                    Id = R.Id,
                                                    RoleName = R.Name,
                                                }).ToListAsync();

            }

            return View(users);

        }



        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {

                var Role = new IdentityRole()
                {
                    Name = model.RoleName,
                };
                await _roleManager.CreateAsync(Role);

                return RedirectToAction(nameof(Index));

            }
            return View(model);
        }



        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest(); //400

            var roleFromDb = await _roleManager.FindByIdAsync(id);

            if (roleFromDb is null)
                return NotFound();

            var user = new RoleViewModel()
            {
                Id = roleFromDb.Id,
                RoleName = roleFromDb.Name

            };

            return View(ViewName, user);


        }



        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var userFromDb = await _roleManager.FindByIdAsync(id);

                if (userFromDb is null)
                    return NotFound();

                userFromDb.Id = model.Id;

                userFromDb.Name = model.RoleName;

                await _roleManager.UpdateAsync(userFromDb);

                return RedirectToAction(nameof(Index));

            }
            return View(model);
        }




        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");


        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, UserViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var userFromDb = await _roleManager.FindByIdAsync(id);

                await _roleManager.DeleteAsync(userFromDb);

                return RedirectToAction(nameof(Index));

            }
            return View(model);


        }





        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role is null)
                return NotFound();

            ViewData[index: "RoleId"] = roleId;

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId, List<UsersInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role is null)
                return NotFound();


            if (ModelState.IsValid)
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
                    }
                    else if (!user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                    {
                        await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                    }
                }

                return RedirectToAction(nameof(Edit), new { id = roleId });
            }
            return View(users);
        }






         // if (result.Succeeded)
         //{                   return RedirectToAction(nameof(SignIn));

         //               foreach (var Error in result.Errors)
                        
         //                   ModelState.AddModelError(string.Empty, Error.Description);

                        


         //              ModelState.AddModelError(string.Empty, "User Email Is Already Exist ):");

                

         //       ModelState.AddModelError(string.Empty, "User Name Is Already Exist ):");
         //   }

















    }


}















