using Microsoft.AspNetCore.Identity;

namespace MovieTicketBookinAPI.Models
{
    public class ApplicationUser : IdentityUser 
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public string Password { get; set; } = string.Empty;
        
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
