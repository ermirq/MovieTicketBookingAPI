using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBookinAPI.DTOs;
using MovieTicketBookinAPI.Services;

namespace MovieTicketBookinAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        [HttpGet("Showtimes/{showtimeId}")]
        public async Task<ActionResult<IEnumerable<SeatDTO>>> GetSeatsByShowtimeId(int showtimeId)
        {
            var seats = await _seatService.GetSeatsByShowtimeAsync(showtimeId);
            if (seats == null || !seats.Any())
                return NotFound("No seats found for this showtime.");
            return Ok(seats);
        }

        //[HttpPost("book")]
        //public async Task<ActionResult> BookSeats([FromBody] List<int> seatIds)
        //{
        //    var success = await _seatService.BookSeatsAsync(seatIds);
        //    if (!success)
        //        return BadRequest("Failed to book seats. Please check the seat IDs and try again.");

        //    return Ok("Seats booked successfully.");
        //}

        [HttpPost("generate-for-showtime/{showtimeId}")]
        public async Task<ActionResult<IEnumerable<SeatDTO>>> GenerateSeatsForShowtime(int showtimeId, int rows, int seatPerRow)
        {
            if (rows <= 0 || seatPerRow <= 0)
            {
                return BadRequest("Rows and seats per row must be positive values.");
            }

            try
            {
                var createdSeats = await _seatService.CreateSeatsAsync(showtimeId, rows, seatPerRow);
                
                return Ok(createdSeats);
            }
            catch (ArgumentException ex) 
            {
                
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
               
                if (ex.Message.Contains("Showtime with ID") && ex.Message.Contains("does not exist"))
                {
                   
                    return NotFound(ex.Message);
                }
                
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
