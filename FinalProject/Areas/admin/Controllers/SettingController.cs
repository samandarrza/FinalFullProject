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
    public class SettingController : Controller
    {
        private readonly EtradeDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SettingController(EtradeDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            var model = _context.Settings.ToList();
            return View(model);
        }
        public IActionResult Edit(int id)
        {
            Setting setting = _context.Settings.FirstOrDefault(x => x.Id == id);
            if (setting == null)
                return RedirectToAction("error", "dashboard");
            return View(setting);
        }

        [HttpPost]
        public IActionResult Edit(Setting setting)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (_context.Settings.Any(x => x.Key == setting.Key && x.Value == setting.Value))
            {
                ModelState.AddModelError("Value", "This Value is already taken");
                return View();
            }
            if (setting.ImageFile != null)
            {
                var newImage = FileManager.Save(setting.ImageFile, _env.WebRootPath, "assets/images");
                if (setting.Value != null)
                {
                    FileManager.Delete(_env.WebRootPath, "assets/images", setting.Value);

                }
                setting.Value = newImage;
            }

            Setting existSetting = _context.Settings.FirstOrDefault(x => x.Key == setting.Key);
            if (existSetting == null)
            {
                return RedirectToAction("error", "dashboard");
            }
            existSetting.Value = setting.Value;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
