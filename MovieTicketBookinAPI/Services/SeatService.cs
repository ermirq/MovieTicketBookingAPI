using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookinAPI.Data;
using MovieTicketBookinAPI.DTOs;
using MovieTicketBookinAPI.Models;

namespace MovieTicketBookinAPI.Services
{
    public class SeatService : ISeatService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public SeatService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SeatDTO>> CreateSeatsAsync(int showtimeId, int rows, int seatPerRow)
        {

            var showtime = await _context.Showtimes
                                         .Include(s => s.Theater)
                                         .ThenInclude(t => t.Seats)
                                         .FirstOrDefaultAsync(s => s.Id == showtimeId);

            if (showtime == null)
            {
                throw new ArgumentException($"Showtime with ID {showtimeId} does not exist.");
            }


            var existingShowtimeSeatsCount = await _context.ShowtimeSeats.CountAsync(ss => ss.ShowtimeId == showtimeId);
            if (existingShowtimeSeatsCount > 0)
            {
                throw new ArgumentException($"Seats for showtime {showtimeId} already exist. Please delete them before generating new seats.");
            }

            var allPhysicalSeatsInTheater = showtime.Theater.Seats
                                                        .OrderBy(s => s.Row)
                                                        .ThenBy(s => s.Number)
                                                        .ToList();

            char maxRowLetter = (char)('A' + rows - 1);
            var selectedPhysicalSeats = allPhysicalSeatsInTheater
                .Where(s => s.Row.Length == 1 && s.Row[0] >= 'A' && s.Row[0] <= maxRowLetter &&
                            s.Number >= 1 && s.Number <= seatPerRow)
                .ToList();

            if (!selectedPhysicalSeats.Any())
            {
                throw new InvalidOperationException($"No physical seats found in theater '{showtime.Theater.Name}' matching the specified dimensions ({rows} rows, {seatPerRow} seats per row). Please ensure theater seats are correctly configured.");
            }


            var newShowtimeSeats = new List<ShowtimeSeat>();
            foreach (var physicalSeat in selectedPhysicalSeats)
            {
                newShowtimeSeats.Add(new ShowtimeSeat
                {
                    ShowtimeId = showtimeId,
                    SeatId = physicalSeat.Id,
                    IsBooked = false
                    //Price = physicalSeat.Price 
                });
            }


            await _context.ShowtimeSeats.AddRangeAsync(newShowtimeSeats);
            await _context.SaveChangesAsync();

            var createdShowtimeSeatsWithPhysicalSeats = await _context.ShowtimeSeats
                .Where(ss => ss.ShowtimeId == showtimeId)
                .Include(ss => ss.Seat)
                .ToListAsync();

            return _mapper.Map<IEnumerable<SeatDTO>>(createdShowtimeSeatsWithPhysicalSeats);
        }

        public async Task<IEnumerable<SeatDTO>> GetSeatsByShowtimeAsync(int showtimeId)
        {
            var showtimeSeats = await _context.ShowtimeSeats
                 .Where(ss => ss.ShowtimeId == showtimeId)
                 .Include(ss => ss.Seat)
                 .ThenInclude(seat => seat.Theater)
                 .Include(ss => ss.Showtime)
                 .ToListAsync();


            if (showtimeSeats == null || !showtimeSeats.Any())
            {
                return null;
            }

            return showtimeSeats.Select(ss => _mapper.Map<SeatDTO>(ss.Seat));
        }
    }
}
