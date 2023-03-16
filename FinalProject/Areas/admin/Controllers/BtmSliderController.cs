using FinalProject.DAL;
using FinalProject.Helpers;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FinalProject.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]

    public class BtmSliderController : Controller
    {
        private readonly EtradeDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BtmSliderController(EtradeDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var model = _context.BtmPromotions.Skip((page - 1) * 5).Take(5).ToList();
            ViewBag.Page = page;
            ViewBag.TotalPage = (int)Math.Ceiling(_context.BtmPromotions.Count() / 5d);

            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BtmPromotion slider)
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

            _context.BtmPromotions.Add(slider);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            BtmPromotion slider = _context.BtmPromotions.FirstOrDefault(x => x.Id == id);

            if (slider == null)
                return RedirectToAction("error", "dashboard");

            return View(slider);
        }

        [HttpPost]
        public IActionResult Edit(BtmPromotion slider)
        {
            BtmPromotion existSlider = _context.BtmPromotions.FirstOrDefault(x => x.Id == slider.Id);

            if (existSlider == null)
                return RedirectToAction("index");

            if (!ModelState.IsValid)
            {
                return View(existSlider);
            }

            existSlider.Title = slider.Title;
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
            BtmPromotion slider = _context.BtmPromotions.FirstOrDefault(x => x.Id == id);

            if (slider == null)
                return RedirectToAction("error", "dashboard");

            FileManager.Delete(_env.WebRootPath, "uploads/sliders", slider.Image);

            _context.BtmPromotions.Remove(slider);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}

