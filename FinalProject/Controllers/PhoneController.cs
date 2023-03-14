using FinalProject.Areas.admin.ViewModels;
using FinalProject.DAL;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;

namespace FinalProject.Controllers
{
    public class PhoneController : Controller
    {
        private readonly EtradeDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public PhoneController(EtradeDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult GetPhone(int id)
        {
            Phone phone = _context.Phones.Include(x=>x.PhoneImages).Include(x=>x.Memory).Include(x=>x.PhoneModel).FirstOrDefault(x=>x.Id == id);

            return PartialView("_PhoneModelPartial", phone);
        }

        public async Task<IActionResult> Detail(int id)
        {

            Phone phone = await _context.Phones
            .Include(x => x.Battery)
            .Include(x => x.Color)
            .Include(x => x.Display)
            .Include(x => x.Memory)
            .Include(x => x.PhoneModel)
            .Include(x => x.PhoneSystem)
            .Include(x => x.ProcessorName)
            .Include(x => x.RAM)
            .Include(x => x.Reviews).ThenInclude(x => x.AppUser)
            .Include(x => x.PhoneImages)
            .FirstOrDefaultAsync(x => x.Id == id);

            PhoneDetailVM detailVM = new PhoneDetailVM
            {
                Phone = phone,
                ReviewVM = new ReviewCreateVM { PhoneId = phone.Id },

                RelatedPhone = _context.Phones.Include(x => x.Battery).Include(x => x.Color)
                .Include(x => x.Display).Include(x => x.Memory).Include(x => x.PhoneModel)
                .Include(x => x.PhoneSystem).Include(x => x.ProcessorName).Include(x => x.RAM).Include(x =>x.PhoneTag)
                .Include(x => x.PhoneImages).Where(x => x.PhoneModel == phone.PhoneModel || x.Memory == phone.Memory)
                .Take(4).ToList(),
            };

            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

                if (user != null)
                {
                    detailVM.HasReview = phone.Reviews.Any(x => x.AppUserId == user.Id);
                }
            }



            if (phone == null)
            {
                TempData["error"] = "Phone not found";
                return RedirectToAction("index", "home");
            }

            return View(detailVM);
        }

        [Authorize(Roles = "Member")]
        [HttpPost]
        public async Task<IActionResult> Review(ReviewCreateVM reviewVM)
        {

            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            Phone phone = _context.Phones
            .Include(x => x.Battery)
            .Include(x => x.Color)
            .Include(x => x.Display)
            .Include(x => x.Memory)
            .Include(x => x.PhoneModel)
            .Include(x => x.PhoneSystem)
            .Include(x => x.ProcessorName)
            .Include(x => x.RAM)
            .Include(x => x.PhoneImages)
            .Include(x => x.Reviews).ThenInclude(x => x.AppUser)
            .FirstOrDefault(x => x.Id == reviewVM.PhoneId);

            if (phone == null)
                return NotFound();


            if (!ModelState.IsValid)
            {

                PhoneDetailVM phoneVM = new PhoneDetailVM
                {
                    Phone = phone,
                    RelatedPhone = _context.Phones.Where(x => x.RAMId == phone.RAMId
                    || x.PhoneModel == phone.PhoneModel).Take(8).ToList(),
                    ReviewVM = reviewVM,
                    HasReview = phone.Reviews.Any(x => x.AppUserId == user.Id)
                };
                return View("detail", phoneVM);
            }

            Review newReview = new Review
            {
                Rate = reviewVM.Rate,
                Text = reviewVM.Text,
                AppUserId = user.Id,
                CreatedAt = DateTime.UtcNow.AddHours(4)
            };
            phone.Reviews.Add(newReview);
            phone.AvgRate = (byte)Math.Ceiling(phone.Reviews.Average(x => x.Rate));
            _context.SaveChanges();
            return RedirectToAction("detail", new { id = phone.Id });
        }

        public async Task<IActionResult> AddToBasket(int phoneId)
        {
            AppUser user = null;

            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }

            BasketVM basket = new BasketVM();

            if (!_context.Phones.Any(x => x.Id == phoneId && x.StockStatus))
            {
                return NotFound();
            }

            if (user != null)
            {
                BasketItem basketItem = _context.BasketItems.FirstOrDefault(x => x.PhoneId == phoneId && x.AppUserId == user.Id);

                if (basketItem == null)
                {
                    basketItem = new BasketItem
                    {
                        AppUserId = user.Id,
                        PhoneId = phoneId,
                        Count = 1,
                        CreatedAt = DateTime.UtcNow.AddHours(4)
                    };
                    _context.BasketItems.Add(basketItem);
                }
                else
                {
                    basketItem.Count++;
                }

                _context.SaveChanges();

                var model = _context.BasketItems.Include(x => x.Phone).ThenInclude(x => x.PhoneImages)
                    .Where(x => x.AppUserId == user.Id).ToList();

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
                var basketStr = HttpContext.Request.Cookies["basket"];
                List<BasketItemCookieVM> basketItemsCookie = null;

                if (basketStr == null)
                {
                    basketItemsCookie = new List<BasketItemCookieVM>();
                }
                else
                {
                    basketItemsCookie = JsonConvert.DeserializeObject<List<BasketItemCookieVM>>(basketStr);
                }

                BasketItemCookieVM basketCookieItem = basketItemsCookie.FirstOrDefault(x => x.PhoneId == phoneId);

                if (basketCookieItem == null)
                {
                    basketCookieItem = new BasketItemCookieVM
                    {
                        PhoneId = phoneId,
                        Count = 1
                    };

                    basketItemsCookie.Add(basketCookieItem);
                }
                else
                {
                    basketCookieItem.Count++;
                }

                var jsonStr = JsonConvert.SerializeObject(basketItemsCookie);
                HttpContext.Response.Cookies.Append("basket", jsonStr);

                foreach (var item in basketItemsCookie)
                {
                    Phone Phone = _context.Phones.Include(x => x.PhoneImages).FirstOrDefault(x => x.Id == item.PhoneId);
                    BasketItemVM itemVM = new BasketItemVM
                    {
                        Phone = Phone,
                        Count = item.Count,
                        Id = 0
                    };
                    basket.Items.Add(itemVM);
                    basket.TotalPrice += item.Count * (itemVM.Phone.SalePrice * (100 - itemVM.Phone.DiscountPercent) / 100);
                }
            }
            return PartialView("_BasketPartial", basket);
        }
        public IActionResult GetSearch(string search)
        {
            var model = _context.Phones.Include(x=>x.PhoneModel).Include(x=>x.Memory)
                .Include(x => x.PhoneImages)
                .Where(x => x.Name.Contains(search)).Take(4).ToList();
            return PartialView("_SearchPartial", model);
        }
    }
}
