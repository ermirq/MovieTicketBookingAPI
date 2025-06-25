using MovieTicketBookinAPI.DTOs;
using MovieTicketBookinAPI.Models;

namespace MovieTicketBookinAPI.Services
{
    public interface IBookingService
    {
        Task<(bool Success, string Message, Booking? Booking)> BookSeatsAsync(BookingRequestDTO bookingRequest);
        Task<bool> DeleteBookingAsync(int id);
        Task<IEnumerable<BookingRequestDTO>> GetAllBookingsAsync();
        Task<BookingRequestDTO> GetBookingByIdAsync(int id);
        Task<BookingRequestDTO> UpdateBookingAsync(int id, BookingRequestDTO request);
    }
}
