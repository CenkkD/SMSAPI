using System.ComponentModel.DataAnnotations;

namespace SmsWebAPI.Dtos
{
	public class UserRoleChangeDto
	{
		[Required(ErrorMessage = "User Id is Required")]
		public string? Id { get; set; }

		public string RoleName { get; set; }
	}
}
