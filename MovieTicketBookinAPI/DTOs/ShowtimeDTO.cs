namespace MovieTicketBookinAPI.DTOs
{
    public class ShowtimeDTO
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }

        public int MovieId { get; set; }
        public MovieDTO? Movie { get; set; }
        public int CinemaId { get; set; }
    }

    public class ShowtimeDetailsDTO
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public MovieDTO Movie { get; set; }
        public CinemaDTO Cinema { get; set; }
        public List<SeatDTO> Seats { get; set; }
    }
}
