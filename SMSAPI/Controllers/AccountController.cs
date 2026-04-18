using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMSAPI.Application.Repositories;
using SMSAPI.Domain.Entities;
using SmsWebAPI.Dtos;
using SmsWebAPI.Entities;
using SmsWebAPI.JwtFeatures;

namespace SmsWebAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        private readonly JwtHandler _jwtHandler;
        private readonly ICustomerRepository _customerRepository;

        private static readonly HashSet<string> ValidRoles = new() { "Admin", "StockManager", "Customer" };

        public AccountController(UserManager<User> userManager, IMapper mapper, JwtHandler jwtHandler,
            RoleManager<Role> roleManager, ICustomerRepository customerRepository)
        {
            _mapper = mapper;
            _userManager = userManager;
            _jwtHandler = jwtHandler;
            _roleManager = roleManager;
            _customerRepository = customerRepository;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration is null)
                return BadRequest(new RegistrationResponseDto { Errors = new[] { "Request body is empty." } });
            if (string.IsNullOrWhiteSpace(userForRegistration.FirstName) || string.IsNullOrWhiteSpace(userForRegistration.LastName))
                return BadRequest(new RegistrationResponseDto { Errors = new[] { "First name and last name are required." } });
            if (string.IsNullOrWhiteSpace(userForRegistration.Email))
                return BadRequest(new RegistrationResponseDto { Errors = new[] { "Email is required." } });
            if (string.IsNullOrWhiteSpace(userForRegistration.Password))
                return BadRequest(new RegistrationResponseDto { Errors = new[] { "Password is required." } });
            if (userForRegistration.Password != userForRegistration.ConfirmPassword)
                return BadRequest(new RegistrationResponseDto { Errors = new[] { "Passwords do not match." } });

            var existingUser = await _userManager.FindByEmailAsync(userForRegistration.Email);
            if (existingUser is not null)
                return BadRequest(new RegistrationResponseDto { Errors = new[] { "This email address is already registered." } });

            var user = _mapper.Map<User>(userForRegistration);
            var result = await _userManager.CreateAsync(user, userForRegistration.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new RegistrationResponseDto { Errors = errors });
            }

            await _userManager.AddToRoleAsync(user, "Customer");

            // Mirror to StockDB so Orders can have a real FK to Customer
            await _customerRepository.AddAsync(new Customer
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                CreatedDate = DateTime.Now,
            });

            return Ok(new RegistrationResponseDto { IsSuccessfulRegistration = true });
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto userForAuthentication)
        {
            if (userForAuthentication is null)
                return BadRequest(new AuthenticationResponseDto { ErrorMessage = "Request body is empty." });
            if (string.IsNullOrWhiteSpace(userForAuthentication.Email))
                return BadRequest(new AuthenticationResponseDto { ErrorMessage = "Email is required." });
            if (string.IsNullOrWhiteSpace(userForAuthentication.Password))
                return BadRequest(new AuthenticationResponseDto { ErrorMessage = "Password is required." });

            var user = await _userManager.FindByNameAsync(userForAuthentication.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, userForAuthentication.Password))
                return Unauthorized(new AuthenticationResponseDto { ErrorMessage = "Invalid email or password." });

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtHandler.CreateToken(user, roles);
            return Ok(new AuthenticationResponseDto { IsAuthSuccessfull = true, Token = token });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser([FromBody] UserForLoginDto userForLogin)
        {
            if (userForLogin is null)
                return BadRequest(new LoginResponseDto { ErrorMessage = "Request body is empty." });
            if (string.IsNullOrWhiteSpace(userForLogin.Email))
                return BadRequest(new LoginResponseDto { ErrorMessage = "Email is required." });
            if (string.IsNullOrWhiteSpace(userForLogin.Password))
                return BadRequest(new LoginResponseDto { ErrorMessage = "Password is required." });

            var user = await _userManager.FindByEmailAsync(userForLogin.Email);
            if (user is null)
                return Unauthorized(new LoginResponseDto { ErrorMessage = "Invalid email or password." });

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, userForLogin.Password);
            if (!isPasswordValid)
                return Unauthorized(new LoginResponseDto { ErrorMessage = "Invalid email or password." });

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new LoginResponseDto
            {
                IsSuccessfulLogin = true,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = _jwtHandler.CreateToken(user, roles)
            });
        }

        [HttpGet("Listallusers")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetUsersAsync()
        {
            var users = await _userManager.Users.Select(u => new UserDeleteDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
            }).ToListAsync();

            foreach (var userDto in users)
            {
                var user = await _userManager.FindByIdAsync(userDto.Id);
                var roleNames = await _userManager.GetRolesAsync(user);
                userDto.RoleName = roleNames.FirstOrDefault();
            }

            return Ok(users);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new UserDeleteResponseDto { ErrorMessage = "User id is required." });

            var currentUserId = _userManager.GetUserId(User);
            if (id == currentUserId)
                return BadRequest(new UserDeleteResponseDto { ErrorMessage = "You cannot delete your own account." });

            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound(new UserDeleteResponseDto { ErrorMessage = "User not found." });

            await _userManager.DeleteAsync(user);

            // Soft-delete the Customer copy in StockDB
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer is not null)
                await _customerRepository.DeletteIdAsync(id);

            return Ok();
        }

        [HttpPut("update-profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            if (dto is null)
                return BadRequest("Profile data is required.");

            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return NotFound("User not found.");

            user.FirstName = dto.FirstName ?? user.FirstName;
            user.LastName = dto.LastName ?? user.LastName;
            await _userManager.UpdateAsync(user);

            // Keep Customer copy in sync
            var customer = await _customerRepository.GetByIdAsync(userId);
            if (customer is not null)
            {
                customer.FirstName = user.FirstName;
                customer.LastName = user.LastName;
                await _customerRepository.UpdateAsync(userId, customer);
            }

            return Ok();
        }

        [HttpPost("rolechanger")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> ChangeRole([FromBody] UserRoleChangeDto userRoleChange)
        {
            if (userRoleChange is null)
                return BadRequest(new UserRoleChangeResponseDto { ErrorMessage = "Request body is empty." });
            if (string.IsNullOrWhiteSpace(userRoleChange.Id))
                return BadRequest(new UserRoleChangeResponseDto { ErrorMessage = "User id is required." });
            if (string.IsNullOrWhiteSpace(userRoleChange.RoleName) || !ValidRoles.Contains(userRoleChange.RoleName))
                return BadRequest(new UserRoleChangeResponseDto { ErrorMessage = $"Invalid role. Valid roles are: {string.Join(", ", ValidRoles)}." });

            var user = await _userManager.FindByIdAsync(userRoleChange.Id);
            if (user is null)
                return NotFound(new UserRoleChangeResponseDto { ErrorMessage = "User not found." });

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, userRoleChange.RoleName);

            return Ok(new UserRoleChangeResponseDto { IsRoleChangeSuccessfull = true });
        }
    }
}
