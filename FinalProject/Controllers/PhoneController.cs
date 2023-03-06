using FinalProject.DAL;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class PhoneController : Controller
    {
        private readonly EtradeDbContext _context;
        public PhoneController(EtradeDbContext context)
        {
            _context = context;
        }
        public IActionResult GetPhone(int id)
        {
            Phone phone = _context.Phones.Include(x=>x.PhoneImages).Include(x=>x.Memory).Include(x=>x.PhoneModel).FirstOrDefault(x=>x.Id == id);

            return PartialView("_PhoneModelPartial", phone);
        }
    }
}
