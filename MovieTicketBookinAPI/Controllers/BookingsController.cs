using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using MovieTicketBookinAPI.DTOs;
using MovieTicketBookinAPI.Services;
using System.Formats.Asn1;
using System.Security.Claims;
using System.Text.Json;

namespace MovieTicketBookinAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly SseService _sseService;
        public BookingsController(IBookingService bookingService, SseService sseService)
        {
            _bookingService = bookingService;
            _sseService = sseService;
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


        [HttpGet("user-bookings")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BookingHistoryDTO>>> GetUserBookings()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated or ID not found." });
            }

            var bookings = await _bookingService.GetUserBookingsAsync(userId);
            return Ok(bookings);
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
