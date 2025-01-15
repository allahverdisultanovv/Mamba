using MambaMVC.Areas.Admin.ViewModels;
using MambaMVC.DAL;
using MambaMVC.Models;
using MambaMVC.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MambaMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<EmployeeItemVM> itemVMs =
                await _context.Employees
                .Select(e => new EmployeeItemVM { Name = e.Name, Surname = e.Surname, Profession = e.Profession.Name, Image = e.Image, Id = e.Id })
                .ToListAsync();
            return View(itemVMs);
        }

        public async Task<IActionResult> Create()
        {
            EmployeeCreateVM itemVM = new EmployeeCreateVM()
            {
                Professions = await _context.Professions.ToListAsync()
            };
            return View(itemVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateVM createVM)
        {
            createVM.Professions = await _context.Professions.ToListAsync();
            if (!ModelState.IsValid) return View(createVM);

            if (!(createVM.Photo.Length < 2 * 1024 * 1024))
            {
                ModelState.AddModelError(nameof(createVM.Photo), "File size incorrect");
                return View(createVM);
            }
            if (!createVM.Photo.ContentType.Contains("image"))
            {
                ModelState.AddModelError(nameof(createVM.Photo), "File type incorrect");
                return View(createVM);
            }

            string fileName = Guid.NewGuid().ToString().Substring(0, 15) + createVM.Photo.FileName;
            string path = String.Empty;
            string[] roots = { _env.WebRootPath, "admin", "assets", "images" };
            for (int i = 0; i < roots.Length; i++)
            {
                path = Path.Combine(path, roots[i]);
            }
            path = Path.Combine(path, fileName);
            using (FileStream fileStream = new(path, FileMode.Create))
            {
                await createVM.Photo.CopyToAsync(fileStream);
            }

            Employee employee = new Employee()
            {
                Name = createVM.Name,
                Surname = createVM.Surname,
                Image = fileName,
                ProfessionId = createVM.ProfessionId,
            };
            await _context.AddAsync(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id)
        {

            if (id == null || id < 1) return BadRequest();
            Employee employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (employee is null) return NotFound();

            EmployeeUpdateVM updateVM = new EmployeeUpdateVM()
            {
                Image = employee.Image,
                Surname = employee.Surname,
                Name = employee.Name,
                ProfessionId = employee.ProfessionId,
                Professions = await _context.Professions.ToListAsync()
            };

            return View(updateVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, EmployeeUpdateVM updateVM)
        {
            updateVM.Professions = await _context.Professions.ToListAsync();
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(nameof(updateVM.ProfessionId), "Profession is wrong");
                return View(updateVM);
            }
            if (id == null || id < 1) return BadRequest();
            Employee employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (employee is null) return NotFound();
            if (updateVM.Photo is not null)
            {
                if (!(updateVM.Photo.Length < 2 * 1024 * 1024))
                {
                    ModelState.AddModelError(nameof(updateVM.Photo), "File size incorrect");
                    return View(updateVM);
                }
                if (!updateVM.Photo.ContentType.Contains("image"))
                {
                    ModelState.AddModelError(nameof(updateVM.Photo), "File type incorrect");
                    return View(updateVM);
                }
                string fileName = Guid.NewGuid().ToString().Substring(0, 15) + updateVM.Photo.FileName;
                string path = String.Empty;
                string[] roots = { _env.WebRootPath, "admin", "assets", "images" };
                for (int i = 0; i < roots.Length; i++)
                {
                    path = Path.Combine(path, roots[i]);
                }
                path = Path.Combine(path, fileName);
                using (FileStream fileStream = new(path, FileMode.Create))
                {
                    await updateVM.Photo.CopyToAsync(fileStream);
                }
                employee.Image.DeleteFile(roots);
                employee.Image = fileName;
            }
            employee.Name = updateVM.Name;
            employee.Surname = updateVM.Surname;
            employee.ProfessionId = updateVM.ProfessionId;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Employee employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (employee is null) return NotFound();

            string path = String.Empty;
            string[] roots = { _env.WebRootPath, "admin", "assets", "images" };
            for (int i = 0; i < roots.Length; i++)
            {
                path = Path.Combine(path, roots[i]);
            }
            employee.Image.DeleteFile(roots);
            _context.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
