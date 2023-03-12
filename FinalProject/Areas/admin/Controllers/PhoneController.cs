using FinalProject.Areas.admin.ViewModels;
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

    public class PhoneController : Controller
    {
        private readonly EtradeDbContext _context;
        private readonly IWebHostEnvironment _env;
        public PhoneController(EtradeDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int page = 1)
        {
            var query = _context.Phones
            .Include(x => x.Battery)
            .Include(x => x.Color)
            .Include(x => x.Display)
            .Include(x => x.Memory)
            .Include(x => x.PhoneModel)
            .Include(x => x.PhoneSystem)
            .Include(x => x.ProcessorName)
            .Include(x => x.RAM)
            .Include(x => x.PhoneImages);
            var model = PaginatedList<Phone>.Create(query, page, 5);
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.PhoneModels = _context.PhoneModels.ToList();
            ViewBag.RAMs = _context.RAMs.ToList();
            ViewBag.Memories = _context.Memories.ToList();
            ViewBag.PhoneSystems = _context.PhoneSystems.ToList();
            ViewBag.ProcessorNames = _context.ProcessorNames.ToList();
            ViewBag.Colors = _context.Colors.ToList();
            ViewBag.Batteries = _context.Batteries.ToList();
            ViewBag.Displays = _context.Displays.ToList();
            ViewBag.PhoneTags = _context.PhoneTags.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Create(Phone phone)
        {
            if (!_context.PhoneModels.Any(x => x.Id == phone.PhoneModelId))
                ModelState.AddModelError("PhoneModelId", "Phone Model not found");

            if (!_context.RAMs.Any(x => x.Id == phone.RAMId))
                ModelState.AddModelError("RAMId", "RAM not found");

            if (!_context.Memories.Any(x => x.Id == phone.MemoryId))
                ModelState.AddModelError("MemoryId", "Memory not found");


            if (!_context.PhoneSystems.Any(x => x.Id == phone.PhoneSystemId))
                ModelState.AddModelError("PhoneSystemId", "Phone System not found");

            if (!_context.ProcessorNames.Any(x => x.Id == phone.ProcessorNameId))
                ModelState.AddModelError("ProcessorNameId", "Processor Name not found");

            if (!_context.Colors.Any(x => x.Id == phone.ColorId))
                ModelState.AddModelError("ColorId", "Color not found");

            if (!_context.Batteries.Any(x => x.Id == phone.BatteryId))
                ModelState.AddModelError("BatteryId", "Battery not found");

            if (!_context.Displays.Any(x => x.Id == phone.DisplayId))
                ModelState.AddModelError("DisplayId", "Display not found");

            if (!_context.PhoneTags.Any(x => x.Id == phone.PhoneTagId))
                ModelState.AddModelError("PhoneTagId", "Phone Tag not found");

            if (!ModelState.IsValid)
            {
                ViewBag.PhoneModels = _context.PhoneModels.ToList();
                ViewBag.RAMs = _context.RAMs.ToList();
                ViewBag.Memories = _context.Memories.ToList();
                ViewBag.PhoneSystems = _context.PhoneSystems.ToList();
                ViewBag.ProcessorNames = _context.ProcessorNames.ToList();
                ViewBag.Colors = _context.Colors.ToList();
                ViewBag.Batteries = _context.Batteries.ToList();
                ViewBag.Displays = _context.Displays.ToList();
                ViewBag.PhoneTags = _context.PhoneTags.ToList();
                ModelState.AddModelError("Name", "Model is not valid");
                return View();
            }

            phone.PhoneImages = new List<PhoneImage>();

            if (phone.PosterFile == null)
            {
                ViewBag.PhoneModels = _context.PhoneModels.ToList();
                ViewBag.RAMs = _context.RAMs.ToList();
                ViewBag.Memories = _context.Memories.ToList();
                ViewBag.PhoneSystems = _context.PhoneSystems.ToList();
                ViewBag.ProcessorNames = _context.ProcessorNames.ToList();
                ViewBag.Colors = _context.Colors.ToList();
                ViewBag.Batteries = _context.Batteries.ToList();
                ViewBag.Displays = _context.Displays.ToList();
                ViewBag.PhoneTags = _context.PhoneTags.ToList();
                ModelState.AddModelError("PosterFile", "Required");
                return View();
            }
            PhoneImage poster = new PhoneImage
            {
                Name = FileManager.Save(phone.PosterFile, _env.WebRootPath, "uploads/phones"),
                Status = true,
            };
            phone.PhoneImages.Add(poster);

            if (phone.HoverPosterFile == null)
            {
                ViewBag.PhoneModels = _context.PhoneModels.ToList();
                ViewBag.RAMs = _context.RAMs.ToList();
                ViewBag.Memories = _context.Memories.ToList();
                ViewBag.PhoneSystems = _context.PhoneSystems.ToList();
                ViewBag.ProcessorNames = _context.ProcessorNames.ToList();
                ViewBag.Colors = _context.Colors.ToList();
                ViewBag.Batteries = _context.Batteries.ToList();
                ViewBag.Displays = _context.Displays.ToList();
                ViewBag.PhoneTags = _context.PhoneTags.ToList();
                ModelState.AddModelError("PosterFile", "Required");
                return View();
            }
            PhoneImage hoverPoster = new PhoneImage
            {
                Name = FileManager.Save(phone.PosterFile, _env.WebRootPath, "uploads/phones"),
                Status = false,
            };
            phone.PhoneImages.Add(hoverPoster);

            if (phone.ImageFiles == null)
            {
                ViewBag.PhoneModels = _context.PhoneModels.ToList();
                ViewBag.RAMs = _context.RAMs.ToList();
                ViewBag.Memories = _context.Memories.ToList();
                ViewBag.PhoneSystems = _context.PhoneSystems.ToList();
                ViewBag.ProcessorNames = _context.ProcessorNames.ToList();
                ViewBag.Colors = _context.Colors.ToList();
                ViewBag.Batteries = _context.Batteries.ToList();
                ViewBag.Displays = _context.Displays.ToList();
                ViewBag.PhoneTags = _context.PhoneTags.ToList();
                ModelState.AddModelError("ImageFiles", "Required");
                return View();
            }

            foreach (var imgFile in phone.ImageFiles)
            {
                PhoneImage phoneImage = new PhoneImage
                {
                    Name = FileManager.Save(imgFile, _env.WebRootPath, "uploads/phones")
                };
                phone.PhoneImages.Add(phoneImage);
            }

            phone.StockStatus = true;

            _context.Phones.Add(phone);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            Phone phone = _context.Phones.Include(x => x.PhoneImages).FirstOrDefault(x => x.Id == id);
            if (phone == null)
                return RedirectToAction("error", "dashboard");

            ViewBag.PhoneModels = _context.PhoneModels.ToList();
            ViewBag.RAMs = _context.RAMs.ToList();
            ViewBag.Memories = _context.Memories.ToList();
            ViewBag.PhoneSystems = _context.PhoneSystems.ToList();
            ViewBag.ProcessorNames = _context.ProcessorNames.ToList();
            ViewBag.Colors = _context.Colors.ToList();
            ViewBag.Batteries = _context.Batteries.ToList();
            ViewBag.Displays = _context.Displays.ToList();
            ViewBag.PhoneTags = _context.PhoneTags.ToList();
            return View(phone);
        }
        [HttpPost]
        public IActionResult Edit(Phone phone)
        {
            Phone existPhone = _context.Phones.Include(x => x.PhoneImages).FirstOrDefault(x => x.Id == phone.Id);

            if (existPhone == null)
                return RedirectToAction("error", "dashboard");

            if (existPhone.PhoneModelId != phone.PhoneModelId && !_context.PhoneModels.Any(x => x.Id == phone.PhoneModelId))
                ModelState.AddModelError("PhoneModelId", "Phone Model not found");

            if (existPhone.RAMId != phone.RAMId && !_context.RAMs.Any(x => x.Id == phone.RAMId))
                ModelState.AddModelError("RAMId", "RAM not found");

            if (existPhone.MemoryId != phone.MemoryId && !_context.Memories.Any(x => x.Id == phone.MemoryId))
                ModelState.AddModelError("MemoryId", "Memory not found");

            if (existPhone.PhoneSystemId != phone.PhoneSystemId && !_context.PhoneSystems.Any(x => x.Id == phone.PhoneSystemId))
                ModelState.AddModelError("PhoneSystemId", "PhoneSystem not found");

            if (existPhone.ProcessorNameId != phone.ProcessorNameId && !_context.ProcessorNames.Any(x => x.Id == phone.ProcessorNameId))
                ModelState.AddModelError("ProcessorNameId", "ProcessorName not found");

            if (existPhone.ColorId != phone.ColorId && !_context.Colors.Any(x => x.Id == phone.ColorId))
                ModelState.AddModelError("ColorId", "Color not found");

            if (existPhone.BatteryId != phone.BatteryId && !_context.Batteries.Any(x => x.Id == phone.BatteryId))
                ModelState.AddModelError("BatteryId", "Battery not found");

            if (existPhone.DisplayId != phone.DisplayId && !_context.Displays.Any(x => x.Id == phone.DisplayId))
                ModelState.AddModelError("DisplayId", "Display not found");

            if (existPhone.PhoneTagId != phone.PhoneTagId && !_context.PhoneTags.Any(x => x.Id == phone.PhoneTagId))
                ModelState.AddModelError("PhoneTagId", "PhoneTag not found");

            if (!ModelState.IsValid)
            {
                ViewBag.PhoneModels = _context.PhoneModels.ToList();
                ViewBag.RAMs = _context.RAMs.ToList();
                ViewBag.Memories = _context.Memories.ToList();
                ViewBag.PhoneSystems = _context.PhoneSystems.ToList();
                ViewBag.ProcessorNames = _context.ProcessorNames.ToList();
                ViewBag.Colors = _context.Colors.ToList();
                ViewBag.Batteries = _context.Batteries.ToList();
                ViewBag.Displays = _context.Displays.ToList();
                ViewBag.PhoneTags = _context.PhoneTags.ToList();          

                return View(existPhone);
            }

            if (phone.PosterFile != null)
            {
                var poster = existPhone.PhoneImages.FirstOrDefault(x => x.Status == true);
                var newPosterName = FileManager.Save(phone.PosterFile, _env.WebRootPath, "uploads/phones");
                FileManager.Delete(_env.WebRootPath, "uploads/phones", poster.Name);
                poster.Name = newPosterName;
            }



            if (phone.HoverPosterFile != null)
            {
                var hover = existPhone.PhoneImages.FirstOrDefault(x => x.Status == false);
                var newHoverName = FileManager.Save(phone.HoverPosterFile, _env.WebRootPath, "uploads/phones");
                FileManager.Delete(_env.WebRootPath, "uploads/phones", hover.Name);
                hover.Name = newHoverName;
            }
            var removedFiles = existPhone.PhoneImages.FindAll(x => x.Status == null && !phone.PhoneImageIds.Contains(x.Id));
            foreach (var item in removedFiles)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/phones", item.Name);
            }

            existPhone.PhoneImages.RemoveAll(x => removedFiles.Contains(x));

            if (phone.ImageFiles != null)
            {
                foreach (var imgFile in phone.ImageFiles)
                {
                    PhoneImage phoneImage = new PhoneImage
                    {
                        Name = FileManager.Save(imgFile, _env.WebRootPath, "uploads/phones"),
                    };
                    existPhone.PhoneImages.Add(phoneImage);
                }
            }

            existPhone.Name = phone.Name;
            existPhone.StockStatus = phone.StockStatus;
            existPhone.SalePrice = phone.SalePrice;
            existPhone.CostPrice = phone.CostPrice;
            existPhone.DiscountPercent = phone.DiscountPercent;
            existPhone.BatteryId = phone.BatteryId;
            existPhone.ColorId = phone.ColorId;
            existPhone.DisplayId = phone.DisplayId;
            existPhone.MemoryId = phone.MemoryId;
            existPhone.PhoneModelId = phone.PhoneModelId;
            existPhone.ProcessorNameId = phone.ProcessorNameId;
            existPhone.PhoneSystemId = phone.PhoneSystemId;
            existPhone.RAMId = phone.RAMId;
            existPhone.IsNew = phone.IsNew;
            existPhone.MostPopular = phone.MostPopular;
            existPhone.PhoneTagId = phone.PhoneTagId;
            _context.SaveChanges();
            return RedirectToAction("index");
        }


        public IActionResult Delete(int id)
        {
            Phone phone = _context.Phones.FirstOrDefault(x => x.Id == id);

            PhoneImage phoneImage = _context.PhoneImages.FirstOrDefault(x => x.PhoneId == id);
            if (phone == null)
                return RedirectToAction("error", "dashboard");
            if (phoneImage != null)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/phones", phoneImage.Name);
            }

            _context.Phones.Remove(phone);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult StockOff(int id)
        {
            Phone phone = _context.Phones.Include(x => x.PhoneImages).FirstOrDefault(x => x.Id == id);

            if (phone == null)
                return RedirectToAction("error", "dashboard");

            phone.StockStatus = false;

            _context.SaveChanges();

            return RedirectToAction("index");
        }
        [HttpPost]
        public IActionResult StockOn(int id)
        {
            Phone phone = _context.Phones.Include(x => x.PhoneImages).FirstOrDefault(x => x.Id == id);

            if (phone == null)
                return RedirectToAction("error", "dashboard");

            phone.StockStatus = true;

            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
