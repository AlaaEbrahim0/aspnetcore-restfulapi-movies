using System.ComponentModel;

namespace MoviesApi.Dtos
{
    public class RegisterDto
    {
        [Required, MaxLength(30)]
        public string FirstName { get; set; }

        [Required, MaxLength(30)]
        public string LastName { get; set; }

        [Required, MaxLength(50)]
        public string UserName { get; set; }

        [Required, MaxLength(128)]
        public string Email { get; set; }

        [Required, MaxLength(256)]
        public string Password { get; set; }
    }
}
