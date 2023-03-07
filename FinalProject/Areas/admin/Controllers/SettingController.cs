using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Areas.admin.Controllers
{
    public class SettingController : Controller
    {
        [Area("admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
