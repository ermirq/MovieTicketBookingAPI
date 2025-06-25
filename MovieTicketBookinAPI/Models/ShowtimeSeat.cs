
using MovieTicketBookinAPI.Models;

namespace MovieTicketBookinAPI.Models
{
    public class ShowtimeSeat
    {
        public int ShowtimeId { get; set; }
        public Showtime Showtime { get; set; } = null!;

        public int SeatId { get; set; }
        public Seat Seat { get; set; } = null!;

        public bool IsBooked { get; set; } = false;
        public decimal Price { get; set; }
        public int? BookingId { get; set; } 
        public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();
    }
}

