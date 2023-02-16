using System.ComponentModel.DataAnnotations;

namespace GymRealmUsers.Data
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is not specified")]
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        [Required(ErrorMessage = "Telephone is not specified")]
        public string Telephone { get; set; }

        public string City { get; set; }
    }
}
