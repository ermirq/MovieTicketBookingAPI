namespace MovieTicketBookinAPI.Models
{
    public class Showtime
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; } = null!;

        public int TheaterId { get; set; }
        public Theater Theater { get; set; } = null!;

        public ICollection<ShowtimeSeat> ShowtimeSeats { get; set; } = new List<ShowtimeSeat>();
    }
}
