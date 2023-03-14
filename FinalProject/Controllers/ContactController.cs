using FinalProject.DAL;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class ContactController : Controller
    {
        private readonly EtradeDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public ContactController(EtradeDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            if (User.Identity.Name != null)
            {
                AppUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;
                Contact contact = new Contact
                {
                    FullName = user.FullName,
                    Email = user.Email
                };
                return View(contact);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Contact contact)
        {
            

            Contact existContact = new Contact
            {
                FullName = contact.FullName,
                Email = contact.Email,
                Note = contact.Note,
            };
            existContact.Email = contact.Email;
            existContact.Note = contact.Note;
            existContact.FullName = contact.FullName;
            _context.Add(existContact);
            _context.SaveChanges();
            return RedirectToAction("index","contact");
        }
    }
}
