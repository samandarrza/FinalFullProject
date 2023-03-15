using FinalProject.DAL;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinalProject.Controllers
{
    public class BlogController : Controller
    {
        private readonly EtradeDbContext _context;
        public BlogController(EtradeDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1)
        {
            var model = _context.Blogs.Skip((page - 1) * 6).Take(6).ToList();
            ViewBag.Page = page;
            ViewBag.TotalPage = (int)Math.Ceiling(_context.Blogs.Count() / 6d);

            return View(model);
        }
        public IActionResult Detail(int id)
        {
            Blog blog = _context.Blogs.FirstOrDefault(x => x.Id == id);
            return View(blog);
        }
    }
}
