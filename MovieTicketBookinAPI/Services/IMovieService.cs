
using MovieTicketBookinAPI.DTOs;

namespace MovieTicketBookinAPI.Services
{
    public interface IMovieService
    {
        Task<MovieDTO> AddMovie(MovieDTO movieDto);
        Task<bool> DeleteMovie(int id);
        Task<MovieDTO> FindAsync(int id);
        Task<List<MovieDTO>> ToListAsync();
        Task<MovieDTO> UpdateAsync(int id, MovieDTO movieDto);
    }
}
