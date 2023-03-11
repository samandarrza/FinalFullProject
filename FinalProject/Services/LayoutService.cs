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

    }
}
