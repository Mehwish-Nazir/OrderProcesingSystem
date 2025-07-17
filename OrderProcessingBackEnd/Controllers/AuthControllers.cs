using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using OrderProcessingBackEnd.Controllers;
using OrderProcessingBackEnd.DTO;
using OrderProcessingBackEnd.Models;
using OrderProcessingBackEnd.Repository;
using OrderProcessingBackEnd.Services;
using Route=Microsoft.AspNetCore.Mvc.RouteAttribute;
using Serilog;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace OrderProcessingBackEnd.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthControllers:ControllerBase
    {
        private readonly ILogger<AuthControllers> _logger;


        private readonly IUserService _userService;
        public AuthControllers(ILogger<AuthControllers> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerDto)
        {
            //if user not eneter value 
            if (registerDto == null)
            {
                return BadRequest("Invalid User Data");
            }
            try
            {
                await _userService.RegsiterUserAsync(registerDto);
                //return Ok("User Registered Successfully");   it will show  'mesage' plain text 'user registed succsefuly' which will generate issue in front end 
                return Ok(new { message = "User Registered Successfully" });
                /*this stamnet wilm retue meegae in 'Jason form '
                //{
                     "message": "User Registered Successfully"
                  }
                */

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginDto)
        {
            try
            {
                var token = await _userService.AuthenticateUserAsync(loginDto);

                if (token == null)
                {
                    return Unauthorized(new { message = "Invalid username or password" });
                }

                // You may consider removing the username from the response message for security
                  return Ok(new { message = $"{loginDto.Username} Login successful", token = token });
               
            }
            catch (Exception ex)
            {
                // Log the exception (important for debugging and monitoring)
              //  _logger.LogError(ex, "Error during login attempt for user: {Username}", loginDto.Username);

                // Return a generic error message to the frontend
                return StatusCode(500, new { message = "An unexpected error occurred. Please try again later.", ex });
            }
        }
        //Get Profile 
        [HttpGet("getProfile")]
        [Authorize]
        public async Task<ActionResult<UserProfileDTO>> GetProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID claim missing.");
            }

            int userId = int.Parse(userIdClaim.Value);

            var profileDto = await _userService.GetProfileAsync(userId);
            return Ok(profileDto);
        }


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // No action is needed in this simple case as we are just relying on token expiration
            // and invalidating it on the frontend by clearing it out.
            return Ok(new { message = "Successfully logged out" });
        }
        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            //Enum.GetValues(typeof(Role))	Gets values as objects (cast to enum)	[Role.Customer, Role.Admin, Role.Employee]
            //Enum.GetNames(typeof(Role))	Gets names as strings	["Customer", "Admin", "Employee"]
            var roles = Enum.GetNames(typeof(Role)).ToList();  // Convert enum to string list
            return Ok(roles);
        }
    }
}
