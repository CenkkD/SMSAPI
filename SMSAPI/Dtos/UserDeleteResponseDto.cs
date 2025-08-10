namespace SmsWebAPI.Dtos
{
	public class UserDeleteResponseDto
	{
		public bool IsDeleteSuccessfull { get; set; }
		public string? ErrorMessage { get; set; }

		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Email { get; set; }

	}
}
