namespace MovieTicketBookinAPI.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public string? UserId { get; set; } 
        public ApplicationUser? User { get; set; }

        public int ShowtimeId { get; set; }
        public Showtime? ShowTime { get; set; } 

        public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();

        public ICollection<ShowtimeSeat> ShowtimeSeats { get; set; } = new List<ShowtimeSeat>();

        public DateTime BookingTime { get; set; }
        public string Status { get; set; } = "Confirmed"; // e.g., "Confirmed", "Pending", "Cancelled"
    }
}
