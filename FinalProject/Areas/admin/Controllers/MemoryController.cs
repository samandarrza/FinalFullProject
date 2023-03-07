using FinalProject.DAL;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.admin.Controllers
{
    [Area("admin")]
    public class MemoryController : Controller
    {
        private readonly EtradeDbContext _context;
        public MemoryController(EtradeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            var model = _context.Memories.Include(x => x.Phones).Skip((page - 1) * 5).Take(5).ToList();
            ViewBag.Page = page;
            ViewBag.TotalPage = (int)Math.Ceiling(_context.Memories.Count() / 5d);

            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Memory memory)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Name", "Error ModelState");
                return View();
            }
            if (_context.Memories.Any(x => x.Name == memory.Name))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }

            _context.Memories.Add(memory);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            Memory memory = _context.Memories.FirstOrDefault(x => x.Id == id);
            if (memory == null)
                return RedirectToAction("error", "dashboard");

            return View(memory);
        }
        [HttpPost]
        public IActionResult Edit(Memory memory)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_context.Memories.Any(x => x.Name == memory.Name && x.Id != memory.Id))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }
            if (memory == null)
            {
                return View();
            }
            Memory existMemory = _context.Memories.FirstOrDefault(x => x.Id == memory.Id);
            if (existMemory == null)
            {
                return RedirectToAction("Index");
            }

            existMemory.Name = memory.Name;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Memory memory = _context.Memories.FirstOrDefault(x => x.Id == id);
            if (memory == null)
                return RedirectToAction("error", "dashboard");
            _context.Memories.Remove(memory);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
