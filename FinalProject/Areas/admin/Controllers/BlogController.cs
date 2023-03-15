using FinalProject.DAL;
using FinalProject.Helpers;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace FinalProject.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public class BlogController : Controller
    {
        private readonly EtradeDbContext _context;
        private readonly IWebHostEnvironment _env;
        public BlogController(EtradeDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int page = 1)
        {
            var model = _context.Blogs.Skip((page - 1) * 5).Take(5).ToList();
            ViewBag.Page = page;
            ViewBag.TotalPage = (int)Math.Ceiling(_context.Blogs.Count() / 5d);

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Blog blog)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Name", "Error ModelState");
                return View();
            }
            if (_context.Teams.Any(x => x.Name == blog.Name))
            {
                ModelState.AddModelError("Name", "Bu adda blog var");
                return View();
            }
            if (blog.ImageFile != null)
            {
                blog.Image = FileManager.Save(blog.ImageFile, _env.WebRootPath, "uploads/blog");
            }

            _context.Blogs.Add(blog);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            Blog blog = _context.Blogs.FirstOrDefault(x => x.Id == id);
            
            if (blog == null)
                return RedirectToAction("error", "dashboard");

            return View(blog);
        }
        [HttpPost]
        public IActionResult Edit(Blog blog)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_context.Blogs.Any(x => x.Name == blog.Name && x.Id != blog.Id))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }
            if (blog == null)
            {
                return View();
            }
            Blog existBlog = _context.Blogs.FirstOrDefault(x => x.Id == blog.Id);

            if (existBlog == null)
            {
                return RedirectToAction("Index");
            }
            if (blog.ImageFile != null)
            {
                var newImage = FileManager.Save(blog.ImageFile, _env.WebRootPath, "uploads/blog");
                FileManager.Delete(_env.WebRootPath, "uploads/blog", existBlog.Image);
                existBlog.Image = newImage;
            }
            existBlog.Name = blog.Name;
            existBlog.Tag = blog.Tag;
            existBlog.Description = blog.Description;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Blog blog = _context.Blogs.FirstOrDefault(x => x.Id == id);
            if (blog == null)
                return RedirectToAction("error", "dashboard");
            _context.Blogs.Remove(blog);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
