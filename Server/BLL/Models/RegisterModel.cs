using System.ComponentModel.DataAnnotations;
using DAL.Enums;

namespace BLL.Models
{
    public class RegisterModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public RoleType? Role { get; set; } 
    }
}
