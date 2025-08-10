namespace SmsWebAPI.Dtos
{
	public class AuthenticationResponseDto
	{
		public bool IsAuthSuccessfull { get; set; }
		public string? ErrorMessage { get; set; }
		public string? Token { get; set; }
	}
}
