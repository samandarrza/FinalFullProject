using FinalProject.DAL;
using FinalProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using NuGet.Protocol.Plugins;

namespace FinalProject.Areas.admin.Controllers
{
    [Area("admin")]
    public class BatteryController : Controller
    {
        private readonly EtradeDbContext _context;
        public BatteryController(EtradeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            var model = _context.Batteries.Include(x => x.Phones).Skip((page - 1) * 5).Take(5).ToList();
            ViewBag.Page = page;
            ViewBag.TotalPage = (int)Math.Ceiling(_context.Batteries.Count() / 5d);

            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Battery battery)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Name", "Error ModelState");
                return View();
            }
            if (_context.Batteries.Any(x => x.Name == battery.Name))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }

            _context.Batteries.Add(battery);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            Battery battery = _context.Batteries.FirstOrDefault(x => x.Id == id);
            if (battery == null)
                return RedirectToAction("error", "dashboard");

            return View(battery);
        }
        [HttpPost]
        public IActionResult Edit(Battery battery)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_context.Batteries.Any(x => x.Name == battery.Name && x.Id != battery.Id))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }
            if (battery == null)
            {
                return View();
            }
            Battery existBattery = _context.Batteries.FirstOrDefault(x => x.Id == battery.Id);
            if (existBattery == null)
            {
                return RedirectToAction("Index");
            }

            existBattery.Name = battery.Name;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Battery battery = _context.Batteries.FirstOrDefault(x => x.Id == id);
            if (battery == null)
                return RedirectToAction("error", "dashboard");
            _context.Batteries.Remove(battery);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }

}
