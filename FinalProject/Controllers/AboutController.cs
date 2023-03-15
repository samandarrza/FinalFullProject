using FinalProject.DAL;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class AboutController : Controller
    {
        private readonly EtradeDbContext _context;
        public AboutController(EtradeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            AboutVM aboutVM = new AboutVM
            {
                Teams = _context.Teams.ToList(),
                Settings = _context.Settings.ToDictionary(x => x.Key, x => x.Value),
            };
            return View(aboutVM);
        }
    }
}
