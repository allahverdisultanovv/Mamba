using MambaMVC.Areas.Admin.ViewModels;
using MambaMVC.DAL;
using MambaMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MambaMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProfessionController : Controller
    {
        private readonly AppDbContext _context;

        public ProfessionController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<ProfessionItemVM> itemVMs = await _context.Professions.Select(p => new ProfessionItemVM { Name = p.Name, Id = p.Id }).ToListAsync();
            return View(itemVMs);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProfessionCreateVM createVM)
        {
            if (!ModelState.IsValid) return View(createVM);
            if (await _context.Professions.AnyAsync(p => p.Name == createVM.Name))
            {
                ModelState.AddModelError(nameof(createVM.Name), "Profession already exists");
                return View(createVM);
            }
            Profession profession = new Profession()
            {
                Name = createVM.Name,
            };
            await _context.Professions.AddAsync(profession);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            Profession profession = await _context.Professions.FirstOrDefaultAsync(p => p.Id == id);
            if (profession == null) return NotFound();

            ProfessionUpdateVM updateVM = new ProfessionUpdateVM()
            {
                Name = profession.Name,
            };
            return View(updateVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, ProfessionItemVM updateVM)
        {
            if (id == null) return BadRequest();
            Profession profession = await _context.Professions.FirstOrDefaultAsync(p => p.Id == id);
            if (profession == null) return NotFound();

            if (await _context.Professions.AnyAsync(p => p.Name == updateVM.Name && p.Id != id))
            {
                ModelState.AddModelError(nameof(updateVM.Name), "Profession already Exists");
                return View(updateVM);
            }

            profession.Name = updateVM.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            Profession profession = await _context.Professions.FirstOrDefaultAsync(p => p.Id == id);
            if (profession == null) return NotFound();

            _context.Professions.Remove(profession);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
