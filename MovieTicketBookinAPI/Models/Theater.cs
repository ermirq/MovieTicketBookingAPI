﻿namespace MovieTicketBookinAPI.Models
{
    public class Theater
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;

        public ICollection<Seat> Seats { get; set; } = new List<Seat>();

        public ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
    }
}
