namespace SmsWebAPI.Dtos

{
	public class LoginResponseDto
	{
		public bool IsSuccessfulLogin { get; set; }
		public string? ErrorMessage { get; set; }

		public string? Email { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Token { get; set; }

	}
}
