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

        public int TheaterId { get; set; }
        public Theater Theater { get; set; } = null!; 

        public ICollection<ShowtimeSeat> ShowtimeSeats { get; set; } = new List<ShowtimeSeat>();
        public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();

    }
}
