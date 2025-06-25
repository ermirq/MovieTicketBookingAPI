
using Microsoft.AspNetCore.Mvc;
using MovieTicketBookinAPI.DTOs;
using System.Collections;

namespace MovieTicketBookinAPI.Services
{
    public interface IShowtimeService
    {
        Task<ShowtimeDTO> AddAsync(ShowtimeDTO showtimeDTO);
        Task<bool> DeleteShowtime(int id);
        Task<ShowtimeDTO> FindAsync(int id);
        Task<IEnumerable<ShowtimeDTO>> GetAllShowtimesAsync();
        Task<ActionResult<ShowtimeDTO>> UpdateAsync(int id, ShowtimeDTO showtimeDTO);
    }
}
