using MambaMVC.Models.Base;

namespace MambaMVC.Models
{
    public class Employee:BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Image {  get; set; }
        //relational
        public Profession Profession { get; set; }
        public int ProfessionId { get; set; }
    }
}
