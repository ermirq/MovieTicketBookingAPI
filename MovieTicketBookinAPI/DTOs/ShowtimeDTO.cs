namespace MovieTicketBookinAPI.DTOs
{
    public class ShowtimeDTO
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }

        public int MovieId { get; set; }
        public int CinemaId { get; set; }
    }
}
