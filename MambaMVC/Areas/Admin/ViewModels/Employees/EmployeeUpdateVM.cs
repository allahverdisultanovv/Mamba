using MambaMVC.Models;

namespace MambaMVC.Areas.Admin.ViewModels
{
    public class EmployeeUpdateVM
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Image { get; set; }
        public IFormFile? Photo { get; set; }
        public int ProfessionId { get; set; }
        public IEnumerable<Profession>? Professions { get; set; }
    }
}
