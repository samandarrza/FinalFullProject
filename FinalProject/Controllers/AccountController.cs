using FinalProject.DAL;
using FinalProject.Helpers;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;


namespace FinalProject.Controllers
{
	public class AccountController : BaseController
	{
        private readonly EtradeDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _env;
		public AccountController(EtradeDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IWebHostEnvironment env)
		{
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
        }
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Index()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            var order = _context.Orders.Include(x => x.OrderItems).Where(x => x.AppUserId == UserId).ToList();

            MemberUpdateVM memberVM = new MemberUpdateVM
            {
                UserName = user.UserName,
                FullName = user.FullName,
                Image = user.Image,
                Email = user.Email,
                Orders = order

            };
            return View(memberVM);
        }


        [Authorize(Roles = "Member")]
        [HttpPost]
        public async Task<IActionResult> Index(MemberUpdateVM memberVM)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
                return RedirectToAction("login");

            if (user.NormalizedUserName != memberVM.UserName.ToUpper() && _context.Users.Any(x => x.NormalizedUserName == memberVM.UserName.ToUpper()))
                ModelState.AddModelError("Username", "Username has already taken");

            if (user.NormalizedEmail != memberVM.Email.ToUpper() && _context.Users.Any(x => x.NormalizedEmail == memberVM.Email.ToUpper()))
                ModelState.AddModelError("Email", "Email has already taken");

            if (!ModelState.IsValid)
                return View();

            if (memberVM.ImageFile != null)
            {
                var newImage = FileManager.Save(memberVM.ImageFile, _env.WebRootPath, "uploads/userImage");

                if (user.Image != null)
                    FileManager.Delete(_env.WebRootPath, "uploads/userImage", user.Image);

                user.Image = newImage;
            }

            if (memberVM.Password != null)
            {
                if (memberVM.CurrentPassword == null)
                {
                    ModelState.AddModelError("CurrentPassword", "CurrentPassword is required!");
                    return View();
                }

                if (!await _userManager.CheckPasswordAsync(user, memberVM.CurrentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "CurrentPassword is not correct!");
                    return View();
                }

                var changePassword = await _userManager.ChangePasswordAsync(user, memberVM.CurrentPassword, memberVM.Password);

                if (!changePassword.Succeeded)
                {
                    foreach (var item in changePassword.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View();
                }
            }

            user.UserName = memberVM.UserName;
            user.FullName = memberVM.FullName;
            user.Email = memberVM.Email;

            var result = await _userManager.UpdateAsync(user);
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("index");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(MemberLoginVM memberVM, string returnUrl)
        {
            AppUser user = await _userManager.FindByNameAsync(memberVM.UserName);

            if (user == null)
            {
                ModelState.AddModelError("", "Username or Password is incorrect");
                return View();
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (!roles.Contains("Member"))
            {
                ModelState.AddModelError("", "Username or Password is incorrect!");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, memberVM.Password, false, true);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or Password is incorrect");
                return View();
            }

            if (returnUrl != null)
                return Redirect(returnUrl);

            return RedirectToAction("index", "home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(MemberRegisterVM memberVM)
        {
            if (!ModelState.IsValid)
                return View();

            if (await _userManager.FindByNameAsync(memberVM.UserName) != null)
            {
                ModelState.AddModelError("Username", "Username already exist!");
                return View();
            }
            if (await _userManager.FindByEmailAsync(memberVM.Email) != null)
            {
                ModelState.AddModelError("Email", "Email already exist!");
                return View();
            }

            AppUser user = new AppUser
            {
                UserName = memberVM.UserName,
                Email = memberVM.Email,
                FullName = memberVM.FullName,
            };

            if (memberVM.ImageFile != null)
            {
                user.Image = FileManager.Save(memberVM.ImageFile, _env.WebRootPath, "uploads/userImage");
            }

            var result = await _userManager.CreateAsync(user, memberVM.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(user, "Member");

            return RedirectToAction("login");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("index", "home");
        }
    }
}
