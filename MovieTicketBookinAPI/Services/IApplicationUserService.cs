using Microsoft.AspNetCore.Identity;
using MovieTicketBookinAPI.DTOs;
using MovieTicketBookinAPI.DTOs.UserDTOs;
using MovieTicketBookinAPI.Models;
using System.Threading.Tasks;

namespace MovieTicketBookinAPI.Services
{
    public interface IApplicationUserService
    {
        Task<IdentityResult> AddRoleAsync(string role);
        Task<IdentityResult> AssignRoleAsync(UserRole model);
        Task<AuthResponseDTO?> LoginAsync(Login model, HttpResponse response);
        Task<IdentityResult> RegisterAsync(Register model);
    }
}
