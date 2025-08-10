using System.ComponentModel.DataAnnotations;

namespace SmsWebAPI.Dtos
{
	public class UserForRegistrationDto
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		[EmailAddress(ErrorMessage = "Not a valid email")]
		public string? Email { get; set; }
		[Required(ErrorMessage = "Password is Required")]
		public string? Password { get; set; }
		[Compare("Password", ErrorMessage = "Passsword and Confirmation Password are not match")]
		public string? ConfirmPassword { get; set; }


	}
}
