using FinalProject.DAL;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly EtradeDbContext _context;

        public HomeController(EtradeDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Sliders = _context.Sliders.ToList(),
                BtmPromotions = _context.BtmPromotions.ToList(),
                Settings = _context.Settings.ToDictionary(x => x.Key, x => x.Value),
                IsNewPhones = _context.Phones.Include(x => x.PhoneImages)
                .Include(x => x.PhoneModel).Include(x => x.Memory).Where(x=>x.IsNew).Take(20).ToList(),
                MostSoldPhones = _context.Phones.Include(x => x.PhoneImages)
                .Include(x => x.PhoneModel).Include(x => x.Memory).Where(x => x.MostPopular).Take(6).ToList(),
                DiscountedPhones = _context.Phones.Include(x => x.PhoneImages).Include(x => x.Reviews)
                .Include(x => x.PhoneModel).Include(x => x.Memory).Where(x => x.DiscountPercent>0).Take(20).ToList(),

            };
            return View(homeVM);
        }
    }
}