using FinalProject.DAL;
using FinalProject.Helpers;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FinalProject.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]

    public class SliderController : Controller
    {
        private readonly EtradeDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(EtradeDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var model = _context.Sliders.Skip((page - 1) * 5).Take(5).ToList();
            ViewBag.Page = page;
            ViewBag.TotalPage = (int)Math.Ceiling(_context.Sliders.Count() / 5d);

            return View(model);
        }
        public IActionResult Create()
        {
            var slider = _context.Sliders.OrderByDescending(x => x.Order).FirstOrDefault();
            int order = slider == null ? 1 : slider.Order + 1;
            ViewBag.Order = order;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if (slider == null)
            {
                return View();
            }
            if (slider.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Required");
                return View();
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            slider.Image = FileManager.Save(slider.ImageFile, _env.WebRootPath, "uploads/sliders");

            _context.Sliders.Add(slider);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            Slider slider = _context.Sliders.FirstOrDefault(x => x.Id == id);

            if (slider == null)
                return RedirectToAction("error", "dashboard");

            return View(slider);
        }

        [HttpPost]
        public IActionResult Edit(Slider slider)
        {
            Slider existSlider = _context.Sliders.FirstOrDefault(x => x.Id == slider.Id);

            if (existSlider == null)
                return RedirectToAction("index");

            if (!ModelState.IsValid)
            {
                return View(existSlider);
            }

            existSlider.Order = slider.Order;
            existSlider.Title1 = slider.Title1;
            existSlider.Title2 = slider.Title2;
            existSlider.BtnText = slider.BtnText;
            existSlider.RedirectUrl = slider.RedirectUrl;

            if (slider.ImageFile != null)
            {
                var newImageFile = FileManager.Save(slider.ImageFile, _env.WebRootPath, "uploads/sliders");
                FileManager.Delete(_env.WebRootPath, "uploads/sliders", existSlider.Image);
                existSlider.Image = newImageFile;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Slider slider = _context.Sliders.FirstOrDefault(x => x.Id == id);

            if (slider == null)
                return RedirectToAction("error", "dashboard");

            FileManager.Delete(_env.WebRootPath, "uploads/sliders", slider.Image);

            _context.Sliders.Remove(slider);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
