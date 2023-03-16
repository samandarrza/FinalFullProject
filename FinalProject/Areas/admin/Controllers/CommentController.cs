using FinalProject.DAL;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FinalProject.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public class CommentController : Controller
	{
		private readonly EtradeDbContext _context;
		public CommentController(EtradeDbContext context)
		{
			_context = context;
		}
		public IActionResult Index(int page = 1)
		{
			var model = _context.Contacts.Skip((page - 1) * 5).Take(5).ToList();
			ViewBag.Page = page;
            ViewBag.TotalPage = (int)Math.Ceiling(_context.Contacts.Count() / 5d);

            return View(model);
		}
        public IActionResult Delete(int id)
        {
            Contact contact = _context.Contacts.FirstOrDefault(x => x.Id == id);
            if (contact == null)
                return RedirectToAction("error", "dashboard");
            _context.Contacts.Remove(contact);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
