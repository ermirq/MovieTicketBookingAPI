namespace MovieTicketBookinAPI.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public string Row { get; set; } = string.Empty;
        public int Number { get; set; } 

        public string GetSeatNumber() => $"{Number}{Row}";

        public string? SeatNumber
        {
            get => GetSeatNumber();
            set => _ = value;
        }

        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; } = null!; 

        public ICollection<ShowtimeSeat> ShowtimeSeats { get; set; } = new List<ShowtimeSeat>();
        public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();

    }
}
