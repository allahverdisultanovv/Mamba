using MambaMVC.Models.Base;

namespace MambaMVC.Models
{
    public class Profession:BaseEntity
    {
        public string Name { get; set; }
        //relational
        public IEnumerable<Employee> Employees { get; set; }
    }
}
