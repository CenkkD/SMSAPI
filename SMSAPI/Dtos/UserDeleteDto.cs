using System.ComponentModel.DataAnnotations;

namespace SmsWebAPI.Dtos
{
	public class UserDeleteDto
	{
		[Required(ErrorMessage = "User Id is Required")]
		public string? Id { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Email { get; set; }

		public string? RoleName { get; set; }



	}
}
