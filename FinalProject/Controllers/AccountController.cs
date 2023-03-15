using FinalProject.DAL;
using FinalProject.Helpers;
using FinalProject.Models;
using FinalProject.ViewModels;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit.Text;
using MimeKit;
using System.Data;
using System.Linq;
using System.Security.Claims;
using MailKit.Net.Smtp;

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
                Orders = order,

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
        public IActionResult GoogleLogin()
        {
            string redirectUrl = Url.Action("GoogleResponse", "Account", Request.Scheme);

            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        //[AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null) RedirectToAction("Login");

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value, info.Principal.FindFirst(ClaimTypes.NameIdentifier).Value, };

            if (result.Succeeded)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                AppUser user = new AppUser
                {
                    FullName = info.Principal.FindFirst(ClaimTypes.Name).Value,
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    Image = "default.jpg"
                };

                var createResult = await _userManager.CreateAsync(user);

                var role = await _userManager.AddToRoleAsync(user, "Member");
                if (createResult.Succeeded)
                {
                    createResult = await _userManager.AddLoginAsync(user, info);
                    if (createResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false);
                        return RedirectToAction("index", "home");
                    }
                }

                return Unauthorized();
            }


        }


            public async Task<IActionResult> Logout()
            {
            await _signInManager.SignOutAsync();

            return RedirectToAction("index", "home");
            }

        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPassword)
        {
            AppUser user = await _userManager.FindByEmailAsync(forgotPassword.Email);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var url = Url.Action("VerifyPasswordReset", "Account", new { email = user.Email, token = token }, Request.Scheme);

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("samandarshr@code.edu.az"));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Reset your password!";
            email.Body = new TextPart(TextFormat.Html) { Text = $"<h1>Hi,{user.FullName}, please click <a href=\"{url}\">here</a> to reset password! </h1>" };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("samandarshr@code.edu.az", "xqgevesgpzwitxik");
            smtp.Send(email);
            smtp.Disconnect(true);

            return View();
        }

        public async Task<IActionResult> VerifyPasswordReset(string email, string token)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);

            if (user == null || !await _userManager.VerifyUserTokenAsync(user,
                _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token))
                return NotFound();

            TempData["Email"] = email;
            TempData["token"] = token;

            return RedirectToAction("ResetPassword");
        }

        public IActionResult ResetPassword()
        {
            var email = TempData["Email"];
            var token = TempData["token"];

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(PasswordResetVM resetVM)
        {
            AppUser user = await _userManager.FindByEmailAsync(resetVM.Email);
            if (user == null)
                return  RedirectToAction("error","home");

            var result = await _userManager.ResetPasswordAsync(user, resetVM.Token, resetVM.Password);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return View();
            }
            return RedirectToAction("login");
        }

    }
}
