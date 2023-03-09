using FinalProject.DAL;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FinalProject.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]

    public class SystemController : Controller
    {

        private readonly EtradeDbContext _context;
        public SystemController(EtradeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            var model = _context.PhoneSystems.Include(x => x.Phones).Skip((page - 1) * 5).Take(5).ToList();
            ViewBag.Page = page;
            ViewBag.TotalPage = (int)Math.Ceiling(_context.PhoneSystems.Count() / 5d);

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(PhoneSystem phoneSystem)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Name", "Error ModelState");
                return View();
            }
            if (_context.PhoneSystems.Any(x => x.Name == phoneSystem.Name))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }

            _context.PhoneSystems.Add(phoneSystem);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            PhoneSystem phoneSystem = _context.PhoneSystems.FirstOrDefault(x => x.Id == id);
            if (phoneSystem == null)
                return RedirectToAction("error", "dashboard");

            return View(phoneSystem);
        }
        [HttpPost]
        public IActionResult Edit(PhoneSystem phoneSystem)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_context.PhoneSystems.Any(x => x.Name == phoneSystem.Name && x.Id != phoneSystem.Id))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }
            if (phoneSystem == null)
            {
                return View();
            }
            PhoneSystem existPhoneSystem = _context.PhoneSystems.FirstOrDefault(x => x.Id == phoneSystem.Id);
            if (existPhoneSystem == null)
            {
                return RedirectToAction("Index");
            }

            existPhoneSystem.Name = phoneSystem.Name;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            PhoneSystem phoneSystem = _context.PhoneSystems.FirstOrDefault(x => x.Id == id);
            if (phoneSystem == null)
                return RedirectToAction("error", "dashboard");
            _context.PhoneSystems.Remove(phoneSystem);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
