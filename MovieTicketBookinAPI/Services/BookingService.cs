using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookinAPI.Data;
using MovieTicketBookinAPI.DTOs;
using MovieTicketBookinAPI.Models;

namespace MovieTicketBookinAPI.Services
{
    public class BookingService : IBookingService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BookingService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BookingResponseDTO> BookSeatsAsync(BookingRequestDTO bookingRequest, string userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var showtime = await _context.Showtimes
                .Include(s => s.ShowtimeSeats)
                .ThenInclude(ss => ss.Seat)
                .FirstOrDefaultAsync(s => s.Id == bookingRequest.ShowtimeId);

            if (showtime == null)
                return new BookingResponseDTO { Success = false, Message = "Showtime not found." };
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return new BookingResponseDTO { Success = false, Message = "User not found." };
            var requestedShowtimeSeats = showtime.ShowtimeSeats
                .Where(ss => bookingRequest.SeatIds
                    .Contains(ss.SeatId))
                .ToList();

            if (requestedShowtimeSeats.Count != bookingRequest.SeatIds.Count)
                return new BookingResponseDTO { Success = false, Message = "One or more seat numbers are invalid."};
            if (requestedShowtimeSeats.Any(ss => ss.IsBooked))
            {
                var alreadyBookedSeatNumbers = requestedShowtimeSeats
                    .Where(ss => ss.IsBooked)
                    .Select(ss => $"{ss.Seat.Row}{ss.Seat.Number}")
                    .ToList();
                return new BookingResponseDTO { Success = false, Message = "One or more seats are already booked."};
            }

            var booking = new Booking
            {
                UserId = userId,
                ShowtimeId = bookingRequest.ShowtimeId,
                SeatIds = bookingRequest.SeatIds,
                BookingTime = DateTime.UtcNow,
                Status = "Confirmed"
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            foreach (var ss in requestedShowtimeSeats)
            {
                var bookingSeat = new BookingSeat
                {
                    BookingId = booking.Id,
                    ShowtimeId = ss.ShowtimeId,
                    SeatId = ss.SeatId,
                    SeatNumber = $"{ss.Seat.Number}{ss.Seat.Row}"
                };

                _context.BookingSeats.Add(bookingSeat);
                ss.IsBooked = true;
                ss.BookingId = booking.Id;
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new BookingResponseDTO
            {
                Success = true,
                Message = "Booking successful."
            };
        }

        public async Task<IEnumerable<BookingRequestDTO>> GetAllBookingsAsync()
        {
            var bookings = await _context.Bookings
                .Include(b => b.BookingSeats)
                .ToListAsync();

            return _mapper.Map<IEnumerable<BookingRequestDTO>>(bookings);
        }

        public async Task<BookingRequestDTO> GetBookingByIdAsync(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.BookingSeats)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (booking == null)
                return null;

            return _mapper.Map<BookingRequestDTO>(booking);
        }

        public async Task<BookingRequestDTO> UpdateBookingAsync(int id, BookingRequestDTO request)
        {
            var booking = _context.Bookings
                .Include(b => b.BookingSeats)
                .FirstOrDefault(b => b.Id == id);

            if (booking == null)
                return null;

            booking.ShowtimeId = request.ShowtimeId;

            _context.BookingSeats.RemoveRange(booking.BookingSeats);

            var oldShowtimeSeats = await _context.ShowtimeSeats
               .Where(ss => ss.BookingId == booking.Id)
               .ToListAsync();

            foreach (var ss in oldShowtimeSeats)
            {
                ss.BookingId = null;
                ss.IsBooked = false;
            }

            foreach (var seatNumber in request.SeatNumbers)
            {
                var seatEntity = await _context.Seats
                    .Where(s => s.SeatNumber == seatNumber)
                    .FirstOrDefaultAsync();

                if (seatEntity == null)
                    throw new InvalidOperationException($"Seat {seatNumber} not found.");

                booking.BookingSeats.Add(new BookingSeat
                {
                    SeatNumber = seatEntity.SeatNumber,
                    BookingId = booking.Id,
                    ShowtimeId = booking.ShowtimeId,
                    SeatId = seatEntity.Id
                });

                var showtimeSeat = await _context.ShowtimeSeats
                    .Where(ss => ss.SeatId == seatEntity.Id && ss.ShowtimeId == booking.ShowtimeId)
                    .FirstOrDefaultAsync();

                if (showtimeSeat != null)
                {
                    showtimeSeat.BookingId = booking.Id;
                    showtimeSeat.IsBooked = true;
                }
            }

            await _context.SaveChangesAsync();
            return _mapper.Map<BookingRequestDTO>(booking);
        }

        public async Task<bool> DeleteBookingAsync(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.BookingSeats)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null)
                return false;
            var relatedShowtimeSeats = await _context.ShowtimeSeats
                .Where(ss => ss.BookingId == booking.Id)
                .ToListAsync();

            foreach (var ss in relatedShowtimeSeats)
            {
                ss.BookingId = null;
                ss.IsBooked = false;
            }

            _context.BookingSeats.RemoveRange(booking.BookingSeats);
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
