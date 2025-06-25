using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBookinAPI.DTOs;
using MovieTicketBookinAPI.Services;

namespace MovieTicketBookinAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheatersController : ControllerBase
    {
        private readonly ITheaterService _theaterService;
        public TheatersController(ITheaterService theaterService)
        {
            _theaterService = theaterService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TheaterDTO>>> GetTheaters()
        {
            var theaters = await _theaterService.GetAllTheatersAsync();
            return Ok(theaters);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TheaterDTO>> GetTheaterById(int id)
        {
            var theater = await _theaterService.FindAsync(id);

            if (theater == null)
                return NotFound("Theater not found.");

            return Ok(theater);
        }

        [HttpPost]
        public async Task<ActionResult<TheaterDTO>> CreateTheater([FromBody] TheaterDTO theaterDto)
        {
            if (theaterDto == null)
                return BadRequest("Invalid theater data.");
            var createdTheater = await _theaterService.AddAsync(theaterDto);
            if (createdTheater == null)
                return BadRequest("Failed to create theater. Please check your input.");
            return CreatedAtAction(nameof(GetTheaterById), new { id = createdTheater.Id }, createdTheater);
        }

        [HttpPost("{theaterId}/add-seats")]
        public async Task<IActionResult> AddSeats(int theaterId, [FromBody] SeatGenerationRequestDTO request)
        {
            if (request == null || request.NumRows <= 0 || request.SeatsPerRow <= 0)
            {
                return BadRequest("Invalid seat configuration.");
            }

            try
            {
                var addedCount = await _theaterService.AddSeatsAsync(theaterId, request.NumRows, request.SeatsPerRow);

                return Ok($"Successfully added {addedCount} seats to theater with ID {theaterId}.");
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
        public async Task<ActionResult<TheaterDTO>> UpdateTheater(int id, [FromBody] TheaterDTO theaterDto)
        {
            var updatedTheater = await _theaterService.UpdateAsync(id, theaterDto);
            if (updatedTheater == null)
                return NotFound("Theater not found.");
            return Ok(updatedTheater);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTheater(int id)
        {
           bool deleted = await _theaterService.DeleteAsync(id);
            if (!deleted)
                return NotFound("Theater not found.");

            return NoContent();
        }
    }
}
