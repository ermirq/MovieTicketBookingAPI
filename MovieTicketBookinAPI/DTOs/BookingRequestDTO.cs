using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieTicketBookinAPI.DTOs
{
    public class BookingRequestDTO
    {
        public string UserId { get; set; } = null!;
        public int ShowtimeId { get; set; }
        public List<string> SeatNumbers { get; set; } = new();  
    }

}