namespace MovieTicketBookinAPI.Models
{
    public class BookingSeat
    {
        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

        public int ShowtimeId { get; set; }

        public int SeatId { get; set; }

        public ShowtimeSeat ShowtimeSeat { get; set; } = null!;
        public string? SeatNumber { get; set; }
    }
}
