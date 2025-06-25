using Microsoft.AspNetCore.Identity;

namespace MovieTicketBookinAPI.Models
{
    public class ApplicationUser : IdentityUser 
    {
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; 
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
