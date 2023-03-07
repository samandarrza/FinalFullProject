using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Areas.admin.Controllers
{
    public class UserController : Controller
    {
        [Area("admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
