using FinalProject.DAL;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.admin.Controllers
{
    [Area("admin")]
    public class RAMController : Controller
    {
        private readonly EtradeDbContext _context;
        public RAMController(EtradeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            var model = _context.RAMs.Include(x => x.Phones).Skip((page - 1) * 5).Take(5).ToList();
            ViewBag.Page = page;
            ViewBag.TotalPage = (int)Math.Ceiling(_context.RAMs.Count() / 5d);

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(RAM ram)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Name", "Error ModelState");
                return View();
            }
            if (_context.RAMs.Any(x => x.Name == ram.Name))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }

            _context.RAMs.Add(ram);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            RAM ram = _context.RAMs.FirstOrDefault(x => x.Id == id);
            if (ram == null)
                return RedirectToAction("error", "dashboard");

            return View(ram);
        }
        [HttpPost]
        public IActionResult Edit(RAM ram)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_context.RAMs.Any(x => x.Name == ram.Name && x.Id != ram.Id))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }
            if (ram == null)
            {
                return View();
            }
            RAM existRAM = _context.RAMs.FirstOrDefault(x => x.Id == ram.Id);
            if (existRAM == null)
            {
                return RedirectToAction("Index");
            }

            existRAM.Name = ram.Name;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            RAM ram = _context.RAMs.FirstOrDefault(x => x.Id == id);
            if (ram == null)
                return RedirectToAction("error", "dashboard");
            _context.RAMs.Remove(ram);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
