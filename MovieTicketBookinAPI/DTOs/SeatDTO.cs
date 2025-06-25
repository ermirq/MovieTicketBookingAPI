using MovieTicketBookinAPI.Models;

namespace MovieTicketBookinAPI.DTOs
{
    public class SeatDTO
    {
        public int Id { get; set; }
        public string Row { get; set; } = string.Empty;
        public int Number { get; set; }
        public decimal Price { get; set; }

        public int TheaterId { get; set; }

    }
}
