using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmsWebAPI.Dtos;
using SmsWebAPI.Entities;
using SmsWebAPI.JwtFeatures;
using System.Data;

namespace SmsWebAPI.Controllers
{
	[Route("api/account")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signinManager;
		private readonly RoleManager<Role> _roleManager;
		private readonly IMapper _mapper;
		private readonly JwtHandler _jwtHandler;

		public AccountController(UserManager<User> userManager, IMapper mapper, JwtHandler jwtHandler, SignInManager<User> signinManager, RoleManager<Role> roleManager)
		{
			_mapper = mapper;
			_userManager = userManager;
			_jwtHandler = jwtHandler;
			_signinManager = signinManager;
			_roleManager = roleManager;
		}
		[HttpPost("register")]
		public async Task<RegistrationResponseDto> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
		{
			if (userForRegistration is null)
				return new RegistrationResponseDto() { Errors = null, IsSuccessfulRegistration = false };

			var user = _mapper.Map<User>(userForRegistration);
			var result = await _userManager.CreateAsync(user, userForRegistration.Password);

			if (!result.Succeeded)
			{
				var errors = result.Errors.Select(e => e.Description);
				new RegistrationResponseDto() { Errors = errors, IsSuccessfulRegistration = false };
			}
			await _userManager.AddToRoleAsync(user, "Customer");
			return new RegistrationResponseDto() { Errors = null, IsSuccessfulRegistration = true };
		}


		[HttpPost("authenticate")]
		public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto userForAuthentication)
		{
			var user = await _userManager.FindByNameAsync(userForAuthentication.Email!);

			if (user is null || !await _userManager.CheckPasswordAsync(user, userForAuthentication.Password!))
				return Unauthorized(new AuthenticationResponseDto { ErrorMessage = "Invalid Authentication" });

			var roles = await _userManager.GetRolesAsync(user);
			var token = _jwtHandler.CreateToken(user, roles);
			return Ok(new AuthenticationResponseDto { IsAuthSuccessfull = true, Token = token });

		}

		[HttpPost("login")]
		public async Task<IActionResult> LoginUser([FromBody] UserForLoginDto userForLogin)
		{
			if (userForLogin is null)
				return BadRequest();

			var user = _mapper.Map<User>(userForLogin);
			var result = await _userManager.FindByEmailAsync(userForLogin.Email!);
			if (result is null)
				return BadRequest(new LoginResponseDto { ErrorMessage = "Invalid Email" });

			var login = await _signinManager.CheckPasswordSignInAsync(user, userForLogin.Password!, false);
			if (login is null)
				return BadRequest(new LoginResponseDto { ErrorMessage = "Invalid Password" });
			var roles = await _userManager.GetRolesAsync(user);
			return Ok(new LoginResponseDto
			{
				IsSuccessfulLogin = true,
				Email = userForLogin.Email,
				FirstName = result.FirstName,
				LastName = result.LastName,
				Token = _jwtHandler.CreateToken(user, roles)

			});
		}

		[HttpGet("Listallusers")]
		public async Task<List<UserDeleteDto>> GetUsersAsync()
		{
			var users = await _userManager.Users.Select(u => new UserDeleteDto
			{
				Id = u.Id,
				FirstName = u.FirstName,
				LastName = u.LastName,
				Email = u.Email,


			}).ToListAsync();


			var ids = await _userManager.Users.Select(u => u.Id).ToListAsync();
			foreach (var id in ids)
			{
				var user = await _userManager.FindByIdAsync(id);
				var roleNames = await _userManager.GetRolesAsync(user);
				foreach (var roleName in roleNames)
				{
					users.Where(x => x.Id == user.Id).First()
							.RoleName = roleName;
				}
			}
			return users;
		}



		[HttpPost("delete")]
		public async Task<IActionResult> DeleteUser([FromBody] string id)
		{
			var result = await _userManager.FindByIdAsync(id);
			if (result is null)
				return BadRequest(new UserDeleteResponseDto { ErrorMessage = "Invalid Id" });

			await _userManager.DeleteAsync(result);
			return Ok();
		}

		[HttpPost("rolechanger")]
		public async Task<IActionResult> ChangeRole([FromBody] UserRoleChangeDto userRoleChange)
		{
			if (userRoleChange is null)
				return BadRequest(new UserRoleChangeResponseDto { ErrorMessage = "Event not Compeleted" });

			var id = userRoleChange.Id;
			string rolename = userRoleChange.RoleName;

			var result = await _userManager.FindByIdAsync(id);

			if (result is null)
				return BadRequest(new UserRoleChangeResponseDto { ErrorMessage = "Invalid Id" });

			await _userManager.RemoveFromRoleAsync(result, "Customer");
			await _userManager.RemoveFromRoleAsync(result, "StockManager");
			if (rolename == "Stock Manager")
			{
				await _userManager.AddToRoleAsync(result, "StockManager");
			}
			if (rolename == "Admin")
			{
				await _userManager.AddToRoleAsync(result, "Admin");
			}
			return Ok(new UserRoleChangeResponseDto
			{
				IsRoleChangeSuccessfull = true,
			});
		}
	}
}