using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBookinAPI.DTOs;
using MovieTicketBookinAPI.Services;

namespace MovieTicketBookinAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MovieDTO>>> GetMovies()
        {
            var movies = await _movieService.ToListAsync();
            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDTO>> GetMovieById(int id)
        {
            MovieDTO movie = await _movieService.FindAsync(id);
            if (movie == null)
                return NotFound();
            return Ok(movie);  
        }

        [HttpPost]
        public async Task<ActionResult<MovieDTO>> CreateMovie([FromBody] MovieDTO movieDto)
        {
            var movie = await _movieService.AddMovie(movieDto);
            if (movie == null)
                return BadRequest("Failed to create movie. Please check your input.");

            return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MovieDTO>> UpdateMovie(int id, [FromBody] MovieDTO movieDto)
        {
            var existingMovie = await _movieService.UpdateAsync(id, movieDto);
            if (existingMovie == null)
                return NotFound("Movie not found.");
            
            return Ok(existingMovie);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult>  DeleteMovie (int id)
        {
            bool deleted = await _movieService.DeleteMovie(id);

            if (!deleted)
                return NotFound($"Movie with ID {id} not found");

            return NoContent();
        } 
    }
}
