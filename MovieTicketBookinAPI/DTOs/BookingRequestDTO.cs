using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieTicketBookinAPI.DTOs
{
    public class BookingRequestDTO
    {
        public int ShowtimeId { get; set; }
        public List<int> SeatIds { get; set; } = new();
        public List<string> SeatNumbers { get; set; } = new();  
    }

    public class BookingResponseDTO
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}