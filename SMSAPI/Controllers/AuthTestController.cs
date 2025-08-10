using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

namespace SmsWebAPI.Controllers
{
	[Authorize]
	[Route("api/controller")]
	[ApiController]
	public class AuthTestController : ControllerBase
	{
		[HttpGet("Test")]
		[Authorize (Roles ="Admin")]
		public IActionResult TestAction()
		{
			return Ok("test message");
		}
	}
}
