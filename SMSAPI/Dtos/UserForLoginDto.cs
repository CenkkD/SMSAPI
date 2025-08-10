using System.ComponentModel.DataAnnotations;

namespace SmsWebAPI.Dtos
{
	public class UserForLoginDto
	{


		[EmailAddress(ErrorMessage = "Not a valid email")]
		public string? Email { get; set; }
		[Required(ErrorMessage = "Password is Required")]
		public string? Password { get; set; }

	}
}
