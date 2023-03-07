using FinalProject.DAL;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.admin.Controllers
{
    [Area("admin")]
    public class ColorController : Controller
    {
        private readonly EtradeDbContext _context;
        public ColorController(EtradeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page =1)
        {
            var model = _context.Colors.Include(x => x.Phones).Skip((page - 1) * 5).Take(5).ToList();
            ViewBag.Page = page;
            ViewBag.TotalPage = (int)Math.Ceiling(_context.Colors.Count() / 5d);

            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Color color)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Name", "Error ModelState");
                return View();
            }
            if (_context.Colors.Any(x => x.Name == color.Name))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }

            _context.Colors.Add(color);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            Color color = _context.Colors.FirstOrDefault(x => x.Id == id);
            if (color == null)
                return RedirectToAction("error", "dashboard");

            return View(color);
        }
        [HttpPost]
        public IActionResult Edit(Color color)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_context.Colors.Any(x => x.Name == color.Name && x.Id != color.Id))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }
            if (color == null)
            {
                return View();
            }
            Color existcolor = _context.Colors.FirstOrDefault(x => x.Id == color.Id);
            if (existcolor == null)
            {
                return RedirectToAction("Index");
            }

            existcolor.Name = color.Name;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Color color = _context.Colors.FirstOrDefault(x => x.Id == id);
            if (color == null)
                return RedirectToAction("error", "dashboard");
            _context.Colors.Remove(color);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
