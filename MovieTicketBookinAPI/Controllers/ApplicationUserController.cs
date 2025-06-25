using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieTicketBookinAPI.DTOs;
using MovieTicketBookinAPI.Models;
using MovieTicketBookinAPI.Models.UserRoles;
using MovieTicketBookinAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieTicketBookinAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly IApplicationUserService _userService;

        public ApplicationUserController(IApplicationUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            var result = await _userService.RegisterAsync(model);

            if (result.Succeeded)
            {
                return Ok(new { Message = "User registered successfully." });
            }

            return BadRequest(result.Errors);
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var token = await _userService.LoginAsync(model);

            if (string.IsNullOrWhiteSpace(token))
            {
                return Unauthorized(new { message = "Invalid credentials." });
            }

            return Ok(new { token });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] string role)
        {
            var result = await _userService.AddRoleAsync(role);

            if (result.Succeeded)
            {
                return Ok(new { message = "Role added successfully." });
            }

            return BadRequest(result.Errors?.Select(e => e.Description) ?? new string[] { "Role already exists." });
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] UserRole model)
        {
            var result = await _userService.AssignRoleAsync(model);

            if (result.Succeeded)
            {
                return Ok(new { message = "Role assigned successfully." });
            }

            return BadRequest(result.Errors?.Select(e => e.Description) ?? new string[] { "User not found or role assignment failed." });
        }
    }
}
