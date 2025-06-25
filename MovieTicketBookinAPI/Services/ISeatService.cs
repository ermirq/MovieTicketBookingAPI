
using MovieTicketBookinAPI.DTOs;

namespace MovieTicketBookinAPI.Services
{
    public interface ISeatService
    {
        //Task<bool> BookSeatsAsync(List<int> seatIds);
        Task<IEnumerable<SeatDTO>> CreateSeatsAsync(int showtimeId, int rows, int seatPerRow);
        Task<IEnumerable<SeatDTO>> GetSeatsByShowtimeAsync(int showtimeId);
    }
}
