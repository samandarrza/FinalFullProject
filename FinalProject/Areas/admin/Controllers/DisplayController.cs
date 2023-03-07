using FinalProject.DAL;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.admin.Controllers
{
    [Area("admin")]
    public class DisplayController : Controller
    {

        private readonly EtradeDbContext _context;
        public DisplayController(EtradeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            var model = _context.Displays.Include(x => x.Phones).Skip((page - 1) * 5).Take(5).ToList();
            ViewBag.Page = page;
            ViewBag.TotalPage = (int)Math.Ceiling(_context.Displays.Count() / 5d);

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Display display)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Name", "Error ModelState");
                return View();
            }
            if (_context.Displays.Any(x => x.Name == display.Name))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }

            _context.Displays.Add(display);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            Display display = _context.Displays.FirstOrDefault(x => x.Id == id);
            if (display == null)
                return RedirectToAction("error", "dashboard");

            return View(display);
        }
        [HttpPost]
        public IActionResult Edit(Display display)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_context.Displays.Any(x => x.Name == display.Name && x.Id != display.Id))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }
            if (display == null)
            {
                return View();
            }
            Display existDisplay = _context.Displays.FirstOrDefault(x => x.Id == display.Id);
            if (existDisplay == null)
            {
                return RedirectToAction("Index");
            }

            existDisplay.Name = display.Name;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Display display = _context.Displays.FirstOrDefault(x => x.Id == id);
            if (display == null)
                return RedirectToAction("error", "dashboard");
            _context.Displays.Remove(display);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
