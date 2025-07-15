namespace MovieTicketBookinAPI.DTOs
{
    public class CinemaDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public List<ShowtimeDTO> Showtimes { get; set; } = new List<ShowtimeDTO>();
    }
}
