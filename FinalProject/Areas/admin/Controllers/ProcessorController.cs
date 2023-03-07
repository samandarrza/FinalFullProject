using FinalProject.DAL;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.admin.Controllers
{
    [Area("admin")]
    public class ProcessorController : Controller
    {

        private readonly EtradeDbContext _context;
        public ProcessorController(EtradeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            var model = _context.ProcessorNames.Include(x => x.Phones).Skip((page - 1) * 5).Take(5).ToList();
            ViewBag.Page = page;
            ViewBag.TotalPage = (int)Math.Ceiling(_context.ProcessorNames.Count() / 5d);

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ProcessorName processorName)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Name", "Error ModelState");
                return View();
            }
            if (_context.ProcessorNames.Any(x => x.Name == processorName.Name))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }

            _context.ProcessorNames.Add(processorName);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            ProcessorName processorName = _context.ProcessorNames.FirstOrDefault(x => x.Id == id);
            if (processorName == null)
                return RedirectToAction("error", "dashboard");

            return View(processorName);
        }
        [HttpPost]
        public IActionResult Edit(ProcessorName processorName)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_context.ProcessorNames.Any(x => x.Name == processorName.Name && x.Id != processorName.Id))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }
            if (processorName == null)
            {
                return View();
            }
            ProcessorName existProcessorName = _context.ProcessorNames.FirstOrDefault(x => x.Id == processorName.Id);
            if (existProcessorName == null)
            {
                return RedirectToAction("Index");
            }

            existProcessorName.Name = processorName.Name;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            ProcessorName processorName = _context.ProcessorNames.FirstOrDefault(x => x.Id == id);
            if (processorName == null)
                return RedirectToAction("error", "dashboard");
            _context.ProcessorNames.Remove(processorName);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
