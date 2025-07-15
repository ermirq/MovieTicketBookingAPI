
using MovieTicketBookinAPI.DTOs;
using MovieTicketBookinAPI.Models;

namespace MovieTicketBookinAPI.Services
{
    public interface ICinemaService
    {
        Task<IEnumerable<CinemaDTO>> GetAllCinemasAsync();
        Task<CinemaDTO> FindAsync(int id);
        Task<CinemaDTO> UpdateAsync(int id, CinemaDTO cinemaDto);
        Task<CinemaDTO> AddAsync(CinemaDTO cinemaDto);
        Task<bool> DeleteAsync(int id);
        Task<int> AddSeatsAsync(int cinemaId, int numRows, int seatsPerRow);
        Task<List<CinemaDTO>> GetCinemasWithShowtimesAsync();
    }
}
