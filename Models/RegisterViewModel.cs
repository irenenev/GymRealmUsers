using System.ComponentModel.DataAnnotations;

namespace GymRealmUsers.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name is not specified")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is not specified")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "The password is not specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "The repeat password is not specified")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Incorrect repeat password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Telephone is not specified")]
        public string Telephone { get; set; }

        public string City { get; set; }
    }
}
