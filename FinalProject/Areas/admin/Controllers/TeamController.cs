using FinalProject.DAL;
using FinalProject.Helpers;
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
    public class TeamController : Controller
    {
        private readonly EtradeDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TeamController(EtradeDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var model = _context.Teams.Skip((page - 1) * 5).Take(5).ToList();
            ViewBag.Page = page;
            ViewBag.TotalPage = (int)Math.Ceiling(_context.Teams.Count() / 5d);

            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Team team)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Name", "Error ModelState");
                return View();
            }
            if (_context.Teams.Any(x => x.Name == team.Name))
            {
                ModelState.AddModelError("Name", "Bu ad var");
                return View();
            }
            if (team.ImageFile != null)
            {
                team.Image = FileManager.Save(team.ImageFile, _env.WebRootPath, "uploads/team");
            }

            _context.Teams.Add(team);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {
            Team team = _context.Teams.FirstOrDefault(x => x.Id == id);
            if (team == null)
                return RedirectToAction("error", "dashboard");

            return View(team);
        }
        [HttpPost]
        public IActionResult Edit(Team team)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_context.Teams.Any(x => x.Name == team.Name && x.Id != team.Id))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }
            if (team == null)
            {
                return View();
            }
            Team existTeam = _context.Teams.FirstOrDefault(x => x.Id == team.Id);
            if (existTeam == null)
            {
                return RedirectToAction("Index");
            }
            if (team.ImageFile != null)
            {
                var newImage = FileManager.Save(team.ImageFile, _env.WebRootPath, "uploads/team");
                FileManager.Delete(_env.WebRootPath, "uploads/team", existTeam.Image);
                existTeam.Image = newImage;
            }
            existTeam.Name = team.Name;
            existTeam.Position = team.Position;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Team team = _context.Teams.FirstOrDefault(x => x.Id == id);
            if (team == null)
                return RedirectToAction("error", "dashboard");
            _context.Teams.Remove(team);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
