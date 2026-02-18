using System.ComponentModel.DataAnnotations;

namespace MovieTicketBookinAPI.DTOs.UserDTOs
{
    public class UserRole
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
