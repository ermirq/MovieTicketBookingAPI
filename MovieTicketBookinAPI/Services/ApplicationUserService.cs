using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MovieTicketBookinAPI.DTOs;
using MovieTicketBookinAPI.DTOs.UserDTOs;
using MovieTicketBookinAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieTicketBookinAPI.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public ApplicationUserService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> AddRoleAsync(string role)
        {
            if (await _roleManager.RoleExistsAsync(role))
            {
                return IdentityResult.Failed(new IdentityError { Description = "Role already exists." });
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(role));
            return result;
        }

        public async Task<IdentityResult> AssignRoleAsync(UserRole model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            var result = await _userManager.AddToRoleAsync(user, model.Role);
            return result;
        }

        public async Task<AuthResponseDTO?> LoginAsync(Login model, HttpResponse response)
        {
            var user = await _userManager.FindByNameAsync(model.Identifier);

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(model.Identifier);
            }

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

                authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"]!)),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                    SecurityAlgorithms.HmacSha256));

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                response.Cookies.Append("jwt", tokenString, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, 
                    SameSite = SameSiteMode.Strict,
                    Expires = token.ValidTo
                });

                return new AuthResponseDTO
                {
                    Token = tokenString,
                    Roles = userRoles.ToList()
                };
            }

            return null;
        }



        public async Task<IdentityResult> RegisterAsync(Register model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateCreated = DateTime.UtcNow,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            return result;
        }
    }
}
