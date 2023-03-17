using FinalProject.DAL;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace FinalProject.Services
{
    public class LayoutService
    {
        private readonly EtradeDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;


        public LayoutService(EtradeDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }
        public Dictionary<string, string> GetSettings()
        {
            return _context.Settings.ToDictionary(x => x.Key, x => x.Value);
        }
        public BasketVM GetBasket()
        {
            BasketVM basket = new BasketVM();

            if (_contextAccessor.HttpContext.User.Identity.IsAuthenticated && _contextAccessor.HttpContext.User.IsInRole("Member"))
            {
                string userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                var model = _context.BasketItems.Include(x => x.Phone).ThenInclude(x => x.PhoneImages).Where(x => x.AppUserId == userId).ToList();
                foreach (var item in model)
                {
                    BasketItemVM itemVM = new BasketItemVM
                    {
                        Phone = item.Phone,
                        Count = item.Count,
                        Id = item.Id
                    };
                    basket.Items.Add(itemVM);
                    basket.TotalPrice += item.Count * (item.Phone.SalePrice * (100 - item.Phone.DiscountPercent) / 100);
                }
            }
            else
            {
                var basketStr = _contextAccessor.HttpContext.Request.Cookies["basket"];
                List<BasketItemCookieVM> basketItemsCookie = new List<BasketItemCookieVM>();
                if (basketStr != null)
                {
                    basketItemsCookie = JsonConvert.DeserializeObject<List<BasketItemCookieVM>>(basketStr);
                }

                foreach (var item in basketItemsCookie)
                {
                    Phone phone = _context.Phones.Include(x => x.PhoneImages).FirstOrDefault(x => x.Id == item.PhoneId);
                    BasketItemVM itemVM = new BasketItemVM
                    {
                        Phone = phone,
                        Count = item.Count,
                        Id = 0
                    };
                    basket.Items.Add(itemVM);
                    basket.TotalPrice += item.Count * (itemVM.Phone.SalePrice * (100 - itemVM.Phone.DiscountPercent) / 100);
                }
            }



            return basket;
        }
        public WishlistVM GetWistlist()
        {
            WishlistVM wishlist = new WishlistVM();

            if (_contextAccessor.HttpContext.User.Identity.IsAuthenticated && _contextAccessor.HttpContext.User.IsInRole("Member"))
            {
                string userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                var model = _context.wishlistItems.Include(x => x.Phone).ThenInclude(x => x.PhoneImages).Where(x => x.AppUserId == userId).ToList();
                foreach (var item in model)
                {
                    WishlistItemVM itemVM = new WishlistItemVM
                    {
                        Phone = item.Phone,
                        Id = item.Id
                    };
                    wishlist.Items.Add(itemVM);
                    wishlist.totalPrice += (item.Phone.SalePrice * (100 - item.Phone.DiscountPercent) / 100);
                }
            }
            else
            {
                var wishlistStr = _contextAccessor.HttpContext.Request.Cookies["wishlist"];
                List<WishlistItemCookieVM> wishlistItemsCookie = new List<WishlistItemCookieVM>();
                if (wishlistStr != null)
                {
                    wishlistItemsCookie = JsonConvert.DeserializeObject<List<WishlistItemCookieVM>>(wishlistStr);
                }

                foreach (var item in wishlistItemsCookie)
                {
                    Phone phone = _context.Phones.Include(x => x.PhoneImages).FirstOrDefault(x => x.Id == item.PhoneId);
                    WishlistItemVM itemVM = new WishlistItemVM
                    {
                        Phone = phone,
                        Id = 0
                    };
                    wishlist.Items.Add(itemVM);
                    wishlist.totalPrice += (itemVM.Phone.SalePrice * (100 - itemVM.Phone.DiscountPercent) / 100);
                }
            }

            return wishlist;
        }
        public double GetTotalPrice()
        {
            double count = (double)_context.OrderItems.Sum(x=>x.SalePrice * (100 - x.DiscountPercent)/100 * x.Count);
            return count;
        }
        public int GetTotalUser()
        {
            int count = (int)_context.AppUsers.Count();
            return count;
        }
        public int TotalOrder()
        {
            int count = (int)_context.Orders.Count();
            return count;
        }
        public int TotalSale()
        {
            int count = (int)_context.OrderItems.Sum(x=>x.Count);
            return count;
        }

        public List<Contact> Contacts()
        {
            var contacts = _context.Contacts.ToList();
            return contacts;
        }

        public List<Order> GetOrders()
        {
            var orders = _context.Orders.Include(x=>x.OrderItems).ToList();
            return orders;
        }

    }
}
