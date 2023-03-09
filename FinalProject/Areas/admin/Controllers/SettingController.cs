using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FinalProject.Areas.admin.Controllers
{
    public class SettingController : Controller
    {
        [Area("admin")]
        [Authorize(Roles = "SuperAdmin,Admin,Editor")]

        public IActionResult Index()
        {
            return View();
        }
    }
}
