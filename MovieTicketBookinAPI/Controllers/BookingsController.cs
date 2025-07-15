using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using MovieTicketBookinAPI.DTOs;
using MovieTicketBookinAPI.Services;
using System.Formats.Asn1;
using System.Security.Claims;

namespace MovieTicketBookinAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost("book")]
        public async Task<IActionResult> BookSeats([FromBody] BookingRequestDTO bookingRequest)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated or ID not found." });
            }
            var result = await _bookingService.BookSeatsAsync(bookingRequest, userId);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingRequestDTO>>> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            if (bookings == null || !bookings.Any())
            {
                return NotFound(new { message = "No bookings found" });
            }
            return Ok(bookings);
        }


        [HttpGet("id")]
        public async Task<ActionResult<BookingRequestDTO>> GetBookingById(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound(new { message = "Booking not found" });
            }
            return Ok(booking);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BookingRequestDTO>> UpdateBooking(int id, [FromBody] BookingRequestDTO request)
        {
            var result = await _bookingService.UpdateBookingAsync(id, request);
            if (result == null)
            {
                return NotFound(new { message = "Booking not found or update failed" });
            }
            return Ok(new { message = "Booking updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var result = await _bookingService.DeleteBookingAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Booking not found" });
            }
            return Ok(new { message = "Booking deleted successfully" });
        }
    }
}
