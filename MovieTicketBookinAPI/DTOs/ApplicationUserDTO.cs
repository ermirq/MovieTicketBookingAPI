using MovieTicketBookinAPI.Models;

namespace MovieTicketBookinAPI.DTOs
{
    public class ApplicationUserDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class UserBookingDTO
    {
        public Guid Id { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public string CinemaName { get; set; } = string.Empty;
        public DateTime Showtime { get; set; }
        public DateTime BookingTime { get; set; }
        public List<string> Seats { get; set; } = new();
    }
}
