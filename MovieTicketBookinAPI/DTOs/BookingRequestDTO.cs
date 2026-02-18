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
    public class BookingHistoryDTO
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public DateTime Showtime { get; set; } 
        public List<string> Seats { get; set; } = new List<string>(); 
        public string CinemaName { get; set; } = string.Empty;
        public DateTime BookingTime { get; set; }
        public string Status { get; set; } = string.Empty; 
    }
}