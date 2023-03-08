using FinalProject.Areas.admin.ViewModels;
using FinalProject.DAL;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.admin.Controllers
{
    [Area("admin")]
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
    }
}
