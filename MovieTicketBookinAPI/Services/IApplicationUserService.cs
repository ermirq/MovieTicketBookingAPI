using Microsoft.AspNetCore.Identity;
using MovieTicketBookinAPI.DTOs;
using MovieTicketBookinAPI.Models;
using MovieTicketBookinAPI.Models.UserRoles;
using System.Threading.Tasks;

namespace MovieTicketBookinAPI.Services
{
    public interface IApplicationUserService
    {
        Task<IdentityResult> AddRoleAsync(string role);
        Task<IdentityResult> AssignRoleAsync(UserRole model);
        Task<string?> LoginAsync(Login model);
        Task<IdentityResult> RegisterAsync(Register model);
    }
}
