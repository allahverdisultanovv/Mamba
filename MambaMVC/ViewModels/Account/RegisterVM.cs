using System.ComponentModel.DataAnnotations;

namespace MambaMVC.ViewModels
{
    public class RegisterVM
    {
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }
        [MinLength(4)]
        [MaxLength(50)]
        public string Surname { get; set; }
        [MinLength(4)]
        [MaxLength(100)]
        public string UserName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
