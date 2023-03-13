using FinalProject.Areas.admin.ViewModels;
using FinalProject.DAL;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class ShopController : Controller
    {
        public readonly EtradeDbContext _context;
        public ShopController(EtradeDbContext context)
        {
            _context = context;
        }
       
        public IActionResult Index(int page = 1, List<int>? bateryId =null, List<int>? colorId = null,
            List<int>? displayId = null, List<int>? memoryId = null, List<int>? phoneModelId = null,
            List<int>? phoneSystemId = null, List<int>? processorNameId = null, List<int>? RAMId = null,
            decimal? minPrice = null, decimal? maxPrice = null, string? sort = "", string? search = "")
        {
            ViewBag.SelectedBatteryIds = bateryId;
            ViewBag.SelectedColorIds = colorId;
            ViewBag.SelectedDisplayIds = displayId;
            ViewBag.SelectedMemoryIds = memoryId;
            ViewBag.SelectedPhoneModelIds = phoneModelId;
            ViewBag.SelectedPhoneSystemIds = phoneSystemId;
            ViewBag.SelectedProcessorNameIds = processorNameId;
            ViewBag.SelectedRAMIds = RAMId;
            ViewBag.SelectedSearch = search;

            var phones = _context.Phones.Include(x => x.PhoneImages)
                .Include(x => x.Battery)
                .Include(x => x.Color)
                .Include(x => x.Display)
                .Include(x => x.Memory)
                .Include(x => x.PhoneModel)
                .Include(x => x.PhoneSystem)
                .Include(x => x.ProcessorName)
                .Include(x => x.RAM).AsQueryable();

            if (bateryId != null && bateryId.Count > 0)
                phones = phones.Where(x => bateryId.Contains(x.BatteryId));

            if (colorId != null && colorId.Count > 0)
                phones = phones.Where(x => colorId.Contains(x.ColorId));

            if (displayId != null && displayId.Count > 0)
                phones = phones.Where(x => displayId.Contains(x.DisplayId));

            if (memoryId != null && memoryId.Count > 0)
                phones = phones.Where(x => memoryId.Contains(x.MemoryId));

            if (phoneModelId != null && phoneModelId.Count > 0)
                phones = phones.Where(x => phoneModelId.Contains(x.PhoneModelId));

            if (phoneSystemId != null && phoneSystemId.Count > 0)
                phones = phones.Where(x => phoneSystemId.Contains(x.PhoneSystemId));

            if (processorNameId != null && processorNameId.Count > 0)
                phones = phones.Where(x => processorNameId.Contains(x.ProcessorNameId));

            if (RAMId != null && RAMId.Count > 0)
                phones = phones.Where(x => RAMId.Contains(x.RAMId));

            if (search != null)
                phones = phones.Where(x => x.Name.Contains(search));

            if (minPrice != null && maxPrice != null)
                phones = phones.Where(x => x.SalePrice >= minPrice && x.SalePrice <= maxPrice);

            switch (sort)
            {
                case "AToZ":
                    phones = phones.OrderBy(x => x.Name);
                    break;
                case "ZToA":
                    phones = phones.OrderByDescending(x => x.Name);
                    break;
                case "LowToHigh":
                    phones = phones.OrderBy(x => x.SalePrice);
                    break;
                case "HighToLow":
                    phones = phones.OrderByDescending(x => x.SalePrice);
                    break;
                default:
                    break;
            }

            ShopVM model = new ShopVM
            {
                Phones = PaginatedList<Phone>.Create(phones, page, 12),
                Batteries = _context.Batteries.Include(x => x.Phones).Where(x => x.Phones.Count > 0).ToList(),
                Colors = _context.Colors.Include(x => x.Phones).Where(x => x.Phones.Count > 0).ToList(),
                Displays = _context.Displays.Include(x => x.Phones).Where(x => x.Phones.Count > 0).ToList(),
                Memories = _context.Memories.Include(x => x.Phones).Where(x => x.Phones.Count > 0).ToList(),
                PhoneModels = _context.PhoneModels.Include(x => x.Phones).Where(x => x.Phones.Count > 0).ToList(),
                PhoneSystems = _context.PhoneSystems.Include(x => x.Phones).Where(x => x.Phones.Count > 0).ToList(),
                ProcessorNames = _context.ProcessorNames.Include(x => x.Phones).Where(x => x.Phones.Count > 0).ToList(),
                RAMs = _context.RAMs.Where(x => x.Phones.Count > 0).ToList(),

                MinPrice = _context.Phones.Min(x => x.SalePrice),
                MaxPrice = _context.Phones.Max(x => x.SalePrice)
            };

            return View(model);
        }
    }
}
