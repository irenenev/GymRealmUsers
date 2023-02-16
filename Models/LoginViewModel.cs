using System.ComponentModel.DataAnnotations;

namespace GymRealmUsers.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is not specified")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The password is not specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
