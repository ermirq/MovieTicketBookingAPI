
using MovieTicketBookinAPI.DTOs;
using MovieTicketBookinAPI.Models;

namespace MovieTicketBookinAPI.Services
{
    public interface ITheaterService
    {
        Task<IEnumerable<TheaterDTO>> GetAllTheatersAsync();
        Task<TheaterDTO> FindAsync(int id);
        Task<TheaterDTO> UpdateAsync(int id, TheaterDTO theaterDto);
        Task<TheaterDTO> AddAsync(TheaterDTO theaterDto);
        Task<bool> DeleteAsync(int id);
        Task<int> AddSeatsAsync(int theaterId, int numRows, int seatsPerRow);
    }
}
