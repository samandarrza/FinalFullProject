using FinalProject.Areas.admin.ViewModels;
using FinalProject.DAL;
using FinalProject.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            DashboardVM dashboard = new DashboardVM
            {
                SalesMovement = _salesMovemonet(),
                OrderStatus = _orderStatus(),
                Orders = _context.Orders.ToList()
            };
            return View(dashboard);
        }
        public IActionResult Error()
        {
            return View();
        }

        private int[] _salesMovemonet()
        {
            int[] currentmonth = new int[12];
            var year = DateTime.UtcNow.Year;
            for (int i = 1; i < currentmonth.Length; i++)
            {
                var currentsales = _context.Orders.Where(x => x.CreatedAt.Month == i);
                currentmonth[i-1] = currentsales.Count();
            }
            return currentmonth;
        }

        private int[] _orderStatus()
        {
            int[] status = new int[4]; 
            var Pending = _context.Orders.Where(x => x.Status == OrderStatus.Pending);
            status[0] = Pending.Count();
            var Accepted = _context.Orders.Where(x => x.Status == OrderStatus.Accepted);
            status[1] = Accepted.Count();
            var Rejected = _context.Orders.Where(x => x.Status == OrderStatus.Rejected);
            status[2] = Rejected.Count();
            var Delivered = _context.Orders.Where(x => x.Status == OrderStatus.Delivered);
            status[3] = Delivered.Count();
            return status;
        }

    }
}
