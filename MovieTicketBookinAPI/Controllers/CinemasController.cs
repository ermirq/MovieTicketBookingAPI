using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookinAPI.DTOs;
using MovieTicketBookinAPI.Models;
using MovieTicketBookinAPI.Services;

namespace MovieTicketBookinAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinemasController : ControllerBase
    {
        private readonly ICinemaService _cinemaService;
        public CinemasController(ICinemaService cinemaService)
        {
            _cinemaService = cinemaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CinemaDTO>>> GetCinemas()
        {
            var cinemas = await _cinemaService.GetAllCinemasAsync();
            return Ok(cinemas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CinemaDTO>> GetCinemaById(int id)
        {
            var cinema = await _cinemaService.FindAsync(id);

            if (cinema == null)
                return NotFound("Cinema not found.");

            return Ok(cinema);
        }

        [HttpGet("with-showtimes")]
        public async Task<ActionResult<List<CinemaDTO>>> GetCinemasWithShowtimes()
        {
            var result = await _cinemaService.GetCinemasWithShowtimesAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CreateCinemaDTO>> CreateCinema([FromBody] CreateCinemaDTO cinemaDto)
        {
            if (cinemaDto == null)
                return BadRequest("Invalid cinema data.");
            var createdCinema = await _cinemaService.AddAsync(cinemaDto);
            if (createdCinema == null)
                return BadRequest("Failed to create cinema. Please check your input.");
            return Ok("Cinema Created Successfully");
        }

        [HttpPost("{cinemaId}/add-seats")]
        public async Task<IActionResult> AddSeats(int cinemaId, [FromBody] SeatGenerationRequestDTO request)
        {
            if (request == null || request.NumRows <= 0 || request.SeatsPerRow <= 0)
            {
                return BadRequest("Invalid seat configuration.");
            }

            try
            {
                var addedCount = await _cinemaService.AddSeatsAsync(cinemaId, request.NumRows, request.SeatsPerRow);

                return Ok($"Successfully added {addedCount} seats to cinema with ID {cinemaId}.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log ex
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CinemaDTO>> UpdateCinema(int id, [FromBody] CinemaDTO cinemaDto)
        {
            var updatedCinema = await _cinemaService.UpdateAsync(id, cinemaDto);
            if (updatedCinema == null)
                return NotFound("Cinema not found.");
            return Ok(updatedCinema);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCinema(int id)
        {
           bool deleted = await _cinemaService.DeleteAsync(id);
            if (!deleted)
                return NotFound("Cinema not found.");

            return NoContent();
        }
    }
}
