using MovieTicketBookinAPI.DTOs;
using MovieTicketBookinAPI.Models;

namespace MovieTicketBookinAPI.Services
{
    public interface IBookingService
    {
        Task<BookingResponseDTO> BookSeatsAsync(BookingRequestDTO bookingRequest, string userId);
        Task<bool> DeleteBookingAsync(int id);
        Task<IEnumerable<BookingRequestDTO>> GetAllBookingsAsync();
        Task<BookingRequestDTO> GetBookingByIdAsync(int id);
        Task<IEnumerable<BookingHistoryDTO>> GetUserBookingsAsync(string userId);
        Task<BookingRequestDTO> UpdateBookingAsync(int id, BookingRequestDTO request);
    }
}
