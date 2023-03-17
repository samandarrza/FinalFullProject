using FinalProject.Areas.admin.ViewModels;
using FinalProject.DAL;
using FinalProject.Helpers;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FinalProject.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class UserController : Controller
    {
        private readonly EtradeDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;
        public UserController(EtradeDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IWebHostEnvironment env, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _env = env;
        }

        public IActionResult Index(int page = 1)
        {
            ViewBag.Page = page;
            ViewBag.TotalPage = (int)Math.Ceiling(_context.Batteries.Count() / 5d);

            var model = _context.AppUsers.Skip((page - 1) * 5).Take(5).ToList();
            return View(model);
        }


        [Authorize(Roles = "SuperAdmin")]
        public IActionResult CreateAdmin()
        {
            ViewBag.Roles = _roleManager.Roles.ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> CreateAdmin(AdminCreatedVM createdVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _roleManager.Roles.ToList();
                return View(createdVM);
            }

            if (await _userManager.FindByEmailAsync(createdVM.Email) != null)
            {
                ModelState.AddModelError("Email", "This email is already taken");
                ViewBag.Roles = _roleManager.Roles.ToList();
                return View(createdVM);
            }

            if (await _userManager.FindByNameAsync(createdVM.UserName) != null)
            {
                ViewBag.Roles = _roleManager.Roles.ToList();
                ModelState.AddModelError("Username", "This Username is already taken");
                return View(createdVM);
            }

            AppUser AdminUser = new AppUser
            {
                FullName = createdVM.FullName,
                UserName = createdVM.UserName,
                Email = createdVM.Email,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(AdminUser, createdVM.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    ViewBag.Roles = _roleManager.Roles.ToList();
                    return View(createdVM);
                }
            }

            foreach (var roleName in createdVM.RoleName)
            {
                var role = _roleManager.Roles.FirstOrDefault(x => x.Name == roleName);
                if (role != null)
                {
                    await _userManager.AddToRoleAsync(AdminUser, role.Name);
                }
            }       
            _context.SaveChanges();
            return RedirectToAction("index","user");
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();
            await _userManager.DeleteAsync(user);

            return RedirectToAction("index","user");

        }

    }
}
