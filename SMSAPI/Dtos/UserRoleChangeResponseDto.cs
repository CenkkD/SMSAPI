namespace SmsWebAPI.Dtos
{
	public class UserRoleChangeResponseDto
	{
		public bool IsRoleChangeSuccessfull { get; set; }
		public string? ErrorMessage { get; set; }
		public string? Id { get; set; }
	}
}
