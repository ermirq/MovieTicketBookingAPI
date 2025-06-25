using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBookinAPI.DTOs;
using MovieTicketBookinAPI.Services;

namespace MovieTicketBookinAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowtimesController : ControllerBase
    {
        private readonly IShowtimeService _showtimeService;
        public ShowtimesController(IShowtimeService showtimeService)
        {
            _showtimeService = showtimeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShowtimeDTO>>> GetShowtimes()
        {
            var showtime = await _showtimeService.GetAllShowtimesAsync();
            return Ok(showtime);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShowtimeDTO>> GetShowtimeById(int id)
        {
            var showtime = await _showtimeService.FindAsync(id);
            return Ok(showtime);
        }

        [HttpPost]
        public async Task<ActionResult<ShowtimeDTO>> AddShowtime([FromBody] ShowtimeDTO showtimeDTO)
        {
            if (showtimeDTO == null)
            {
                return BadRequest("Invalid showtime data");
            }

            try
            {
                var createdShowtime = await _showtimeService.AddAsync(showtimeDTO);

                if (createdShowtime == null)
                {
                    return StatusCode(500, "An error occurred while creating the showtime.");
                }

                return Ok(createdShowtime);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<ShowtimeDTO>> UpdateShowtime(int id, [FromBody] ShowtimeDTO showtimeDTO)
        {
            var updatedShowtime = await _showtimeService.UpdateAsync(id, showtimeDTO);
            if (updatedShowtime == null)
                return BadRequest("Invalid data");

            return updatedShowtime;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShowtime(int id)
        {
            bool deletedShowtime = await _showtimeService.DeleteShowtime(id);
            if (!deletedShowtime)
                return NotFound("Showtime Not Found");

            return NoContent();
        }
    }
}
