using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinalProject.Controllers
{
    public class BaseController : Controller
    {
		protected string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
