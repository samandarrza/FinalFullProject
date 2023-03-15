using FinalProject.Areas.admin.ViewModels;
using FinalProject.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FinalProject.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]

    public class DashboardController : Controller
    {
        private readonly EtradeDbContext _context;
        public DashboardController(EtradeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
           
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
