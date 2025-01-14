using MambaMVC.Areas.Admin.ViewModels;
using MambaMVC.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MambaMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<EmployeeItemVM> itemVMs =
                await _context.Employees
                .Select(e => new EmployeeItemVM { Name = e.Name, Surname = e.Surname, Profession = e.Profession.Name })
                .ToListAsync();
            return View(itemVMs);
        }

        public IActionResult Create()
        {
            return View();
        }

    }
}
