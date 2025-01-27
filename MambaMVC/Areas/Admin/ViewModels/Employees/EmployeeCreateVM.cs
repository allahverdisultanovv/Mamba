﻿using MambaMVC.Models;

namespace MambaMVC.Areas.Admin.ViewModels
{
    public class EmployeeCreateVM
    {
        public IFormFile Photo { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int ProfessionId { get; set; }
        public IEnumerable<Profession>? Professions { get; set; }

    }
}
