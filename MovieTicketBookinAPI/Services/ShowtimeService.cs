using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookinAPI.Data;
using MovieTicketBookinAPI.DTOs;
using MovieTicketBookinAPI.Models;

namespace MovieTicketBookinAPI.Services
{
    public class ShowtimeService: IShowtimeService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public ShowtimeService(AppDbContext context, IMapper mapper)
        { 
            _context = context;
            _mapper = mapper;
        }

        public async Task<ShowtimeDTO> AddAsync(ShowtimeDTO showtimeDTO)
        {
            var movieExists = await _context.Movies.AnyAsync(m => m.Id == showtimeDTO.MovieId);
            if (!movieExists)
                throw new ArgumentException("Movie does not exist");

            var theater = await _context.Theaters.FindAsync(showtimeDTO.TheaterId);
            if (theater == null)
                throw new ArgumentException("Theater does not exist");

            var showtimeEntity = _mapper.Map<Showtime>(showtimeDTO);

            var seats = await _context.Seats
                .Where(s => s.TheaterId == showtimeDTO.TheaterId)
                .ToListAsync();

            if (!seats.Any())
                throw new InvalidOperationException("No seats configured for this theater.");

            var showtimeSeats = seats.Select(seat => new ShowtimeSeat
            {
                SeatId = seat.Id,
                Seat = seat,
                IsBooked = false,
                Price = 10.0m // or whatever default price you want
            }).ToList();

            showtimeEntity.ShowtimeSeats = showtimeSeats;

            await _context.Showtimes.AddAsync(showtimeEntity);
            await _context.SaveChangesAsync();

            return _mapper.Map<ShowtimeDTO>(showtimeEntity);
        }


        public async Task<bool> DeleteShowtime(int id)
        {
            var deletedShowtime = await _context.Showtimes.FirstOrDefaultAsync(i => i.Id == id);
            
            if(deletedShowtime == null)
                throw new Exception ("Showtime Not Found");

            _context.Showtimes.Remove(deletedShowtime);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ShowtimeDTO> FindAsync(int id)
        {
            var showtime = await _context.Showtimes.FirstOrDefaultAsync(t => t.Id == id);
            if (showtime == null)
            {
                throw new Exception("Showtime not found.");
            }
            return _mapper.Map<ShowtimeDTO>(showtime);
        }

        public async Task<IEnumerable<ShowtimeDTO>> GetAllShowtimesAsync()
        {
            var showtime = await _context.Showtimes.ToListAsync();
            return _mapper.Map<IEnumerable<ShowtimeDTO>>(showtime);
        }

        public async Task<ActionResult<ShowtimeDTO>> UpdateAsync(int id, ShowtimeDTO showtimeDTO)
        {
            var existingShowtime = await _context.Showtimes.FirstOrDefaultAsync(d => d.Id == id);
            if (existingShowtime == null)
                throw new ArgumentException("Showtime does not exits");

            existingShowtime.StartTime = showtimeDTO.StartTime;
            existingShowtime.MovieId = showtimeDTO.MovieId;
            existingShowtime.TheaterId = showtimeDTO.TheaterId;

            var movieExists = await _context.Movies.AnyAsync(m => m.Id == showtimeDTO.MovieId);
            var theaterExists = await _context.Theaters.AnyAsync(t => t.Id == showtimeDTO.TheaterId);

            if (!movieExists || !theaterExists)
                throw new ArgumentException("Movie or theater does not exist");

            _mapper.Map(showtimeDTO, existingShowtime);

            await _context.SaveChangesAsync();

            return _mapper.Map<ShowtimeDTO>(existingShowtime);
        }
    }
}
