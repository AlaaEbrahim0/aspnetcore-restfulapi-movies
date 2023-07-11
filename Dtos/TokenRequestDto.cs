using System.ComponentModel;

namespace MoviesApi.Dtos
{
	public class TokenRequestDto
	{
        [Required, EmailAddress]
        public string Email { get; set; }

		[Required, PasswordPropertyText]
		public string Password { get; set; }
    }
}
