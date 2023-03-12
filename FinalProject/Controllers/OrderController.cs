using FinalProject.DAL;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    public class OrderController : BaseController
    {
        private readonly EtradeDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public OrderController(EtradeDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Checkout()
        {
            var model = await _getCheckoutVM();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            if (!ModelState.IsValid)
            {
                var model = await _getCheckoutVM();
                model.Order = order;
                return View(model);
            }
            List<BasketItem> basketItems = new List<BasketItem>();

            if (User.Identity.IsAuthenticated)
            {
                basketItems = _context.BasketItems.Include(x => x.Phone).Where(x => x.AppUserId == UserId).ToList();
                order.AppUserId = UserId;
                _context.BasketItems.RemoveRange(basketItems);
            }
            else
            {
                basketItems = _mapBasketItems(_getCookieBasketItems());
                Response.Cookies.Delete("basket");
            }

            order.OrderItems = basketItems.Select(ph => new OrderItem
            {
                PhoneId = ph.PhoneId,
                Count = ph.Count,
                DiscountPercent = ph.Phone.DiscountPercent,
                CostPrice = ph.Phone.CostPrice,
                SalePrice = ph.Phone.SalePrice,
                Name = ph.Phone.Name
            }).ToList();


            _context.Orders.Add(order);

            _context.SaveChanges();
            return RedirectToAction("index", "home");
        }



        private async Task<CheckoutVM> _getCheckoutVM()
        {
            List<BasketItem> basketItems = new List<BasketItem>();
            CheckoutVM model = new CheckoutVM();

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                AppUser user = await _userManager.FindByIdAsync(userId);

                model.Order = new Order
                {
                    Fullname = user.FullName,
                    Email = user.Email
                };

                basketItems = _context.BasketItems.Include(x => x.Phone).Where(x => x.AppUserId == userId).ToList();
            }
            else
            {
                basketItems = _mapBasketItems(_getCookieBasketItems());
            }
            model.CheckoutItems = basketItems.Select(item => new CheckoutItemVM
            {
                Count = item.Count,
                Name = item.Phone.Name,
                TotalPrice = (item.Phone.SalePrice * (100 - item.Phone.DiscountPercent) / 100) * item.Count,
            }).ToList();


            model.Total = model.CheckoutItems.Sum(x => x.TotalPrice);
            return model;
        }
        private List<BasketItemCookieVM> _getCookieBasketItems()
        {
            var basketStr = HttpContext.Request.Cookies["basket"];

            List<BasketItemCookieVM> basketCookieItems = new List<BasketItemCookieVM>();

            if (basketStr != null)
            {
                basketCookieItems = JsonConvert.DeserializeObject<List<BasketItemCookieVM>>(basketStr);

            }
            return basketCookieItems;
        }
        private List<BasketItem> _mapBasketItems(List<BasketItemCookieVM> basketCookieItems)
        {
            List<BasketItem> basketItems = new List<BasketItem>();
            foreach (var item in basketCookieItems)
            {
                Phone phone = _context.Phones.FirstOrDefault(x => x.Id == item.PhoneId && x.StockStatus);
                if (phone == null) continue;

                BasketItem basketItem = new BasketItem
                {
                    Count = item.Count,
                    PhoneId = item.PhoneId,
                    Phone = phone,
                };
                basketItems.Add(basketItem);
            }
            return basketItems;
        }
    }
}
