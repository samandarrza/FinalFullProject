using FinalProject.DAL;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.admin.Controllers
{
    [Area("admin")]
    public class ModelController : Controller
    {
        private readonly EtradeDbContext _context;
        public ModelController(EtradeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            var model = _context.PhoneModels.Include(x => x.Phones).Skip((page - 1) * 5).Take(5).ToList();
            ViewBag.Page = page;
            ViewBag.TotalPage = (int)Math.Ceiling(_context.PhoneModels.Count() / 5d);

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(PhoneModel phoneModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Name", "Error ModelState");
                return View();
            }
            if (_context.PhoneModels.Any(x => x.Name == phoneModel.Name))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }

            _context.PhoneModels.Add(phoneModel);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            PhoneModel phoneModel = _context.PhoneModels.FirstOrDefault(x => x.Id == id);
            if (phoneModel == null)
                return RedirectToAction("error", "dashboard");

            return View(phoneModel);
        }
        [HttpPost]
        public IActionResult Edit(PhoneModel phoneModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_context.PhoneModels.Any(x => x.Name == phoneModel.Name && x.Id != phoneModel.Id))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }
            if (phoneModel == null)
            {
                return View();
            }
            PhoneModel existPhoneModel = _context.PhoneModels.FirstOrDefault(x => x.Id == phoneModel.Id);
            if (existPhoneModel == null)
            {
                return RedirectToAction("Index");
            }

            existPhoneModel.Name = phoneModel.Name;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            PhoneModel phoneModel = _context.PhoneModels.FirstOrDefault(x => x.Id == id);
            if (phoneModel == null)
                return RedirectToAction("error", "dashboard");
            _context.PhoneModels.Remove(phoneModel);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
