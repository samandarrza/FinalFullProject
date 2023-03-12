using FinalProject.Areas.admin.ViewModels;
using FinalProject.DAL;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FinalProject.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public class OrderController : Controller
    {
        private readonly EtradeDbContext _context;
        public OrderController(EtradeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Orders.Include(x => x.OrderItems);

            var model = PaginatedList<Order>.Create(query, page, 5);

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            Order order = _context.Orders.Include(x => x.OrderItems).FirstOrDefault(x => x.Id == id);

            if (order == null)
                return RedirectToAction("error", "dashboard");

            return View(order);
        }
        [HttpPost]
        public IActionResult Accept(int id)
        {
            Order order = _context.Orders.Include(x => x.OrderItems).FirstOrDefault(x => x.Id == id);

            if (order == null)
                return RedirectToAction("error", "dashboard");

            order.Status = Enums.OrderStatus.Accepted;

            _context.SaveChanges();

            return RedirectToAction("index");
        }
        [HttpPost]
        public IActionResult Reject(int id)
        {
            Order order = _context.Orders.Include(x => x.OrderItems).FirstOrDefault(x => x.Id == id);

            if (order == null)
                return RedirectToAction("error", "dashboard");

            order.Status = Enums.OrderStatus.Rejected;

            _context.SaveChanges();

            return RedirectToAction("index");
        }
        [HttpPost]
        public IActionResult Delivered(int id)
        {
            Order order = _context.Orders.Include(x => x.OrderItems).FirstOrDefault(x => x.Id == id);

            if (order == null)
                return RedirectToAction("error", "dashboard");

            order.Status = Enums.OrderStatus.Delivered;

            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
