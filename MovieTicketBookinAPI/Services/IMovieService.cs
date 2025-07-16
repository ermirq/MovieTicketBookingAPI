
using MovieTicketBookinAPI.DTOs;
using MovieTicketBookinAPI.Models;

namespace MovieTicketBookinAPI.Services
{
    public interface IMovieService
    {
        Task<CreateMovieDTO> AddMovie(CreateMovieDTO movieDto);
        Task<bool> DeleteMovie(int id);
        Task<MovieDTO> FindAsync(int id);
        Task<List<MovieDTO>> SearchMoviesByNameAsync(string name);
        Task<List<MovieDTO>> ToListAsync();
        Task<MovieDTO> UpdateAsync(int id, MovieDTO movieDto);
    }
}
