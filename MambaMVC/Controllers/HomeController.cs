using MambaMVC.DAL;
using MambaMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MambaMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<HomeVM> homeVMs = await _context.Employees.Select(e => new HomeVM
            {
                Image = e.Image,
                Surname = e.Surname,
                Name = e.Name,
                Profession = e.Profession.Name,
                FB = "www.facebook.com",
                Insta = "www.instagram.com",
                X = "www.twitter.com",
                Linkedin = "www.linkedin.com"
            }).ToListAsync();
            return View(homeVMs);
        }


    }
}
