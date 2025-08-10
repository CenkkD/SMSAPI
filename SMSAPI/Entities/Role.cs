using Microsoft.AspNetCore.Identity;

namespace SmsWebAPI.Entities
{
	public class Role : IdentityRole
	{
		public string? Description { get; set; }
	}
}
