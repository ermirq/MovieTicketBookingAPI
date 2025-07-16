using System.ComponentModel.DataAnnotations;

namespace MovieTicketBookinAPI.DTOs
{
    public class CinemaDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Location { get; set; } = string.Empty;
        public List<ShowtimeDTO> Showtimes { get; set; } = new List<ShowtimeDTO>();
    }

    public class CreateCinemaDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters.")]
        public string Location { get; set; } = string.Empty;

        [Required]
        public int? NumRows { get; set; }
        [Required]
        public int? SeatsPerRow { get; set; }
    }
}
