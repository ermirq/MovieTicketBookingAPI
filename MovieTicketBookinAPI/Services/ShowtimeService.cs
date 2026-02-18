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

            var cinema = await _context.Cinemas.FindAsync(showtimeDTO.CinemaId);
            if (cinema == null)
                throw new ArgumentException("Cinema does not exist");

            var showtimeEntity = _mapper.Map<Showtime>(showtimeDTO);

            var seats = await _context.Seats
                .Where(s => s.CinemaId == showtimeDTO.CinemaId)
                .ToListAsync();

            if (!seats.Any())
                throw new InvalidOperationException("No seats configured for this cinema.");

            var showtimeSeats = seats.Select(seat => new ShowtimeSeat
            {
                SeatId = seat.Id,
                Seat = seat,
                IsBooked = false,
                Price = 10.0m 
            }).ToList();

            showtimeEntity.ShowtimeSeats = showtimeSeats;

            await _context.Showtimes.AddAsync(showtimeEntity);
            await _context.SaveChangesAsync();

            return _mapper.Map<ShowtimeDTO>(showtimeEntity);
        }

        public async Task<ShowtimeDetailsDTO?> GetShowtimeDetailsAsync(int showtimeId)
        {
            var showtime = await _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Cinema)
                .Include(s => s.ShowtimeSeats)
                    .ThenInclude(ss => ss.Seat)
                .FirstOrDefaultAsync(s => s.Id == showtimeId);

            if (showtime == null)
                return null;

            return new ShowtimeDetailsDTO
            {
                Id = showtime.Id,
                StartTime = showtime.StartTime,
                Movie = new MovieDTO
                {
                    Title = showtime.Movie?.Title,
                    Genre = showtime.Movie?.Genre,
                    DurationInMinutes = showtime.Movie?.DurationInMinutes ?? 0,
                    Description = showtime.Movie?.Description,
                    PosterUrl = showtime.Movie?.PosterUrl
                },
                Cinema = new CinemaDTO
                {
                    Name = showtime.Cinema?.Name,
                    Location = showtime.Cinema?.Location
                },
                Seats = showtime.ShowtimeSeats.Select(ss => new SeatDTO
                {
                    Id = ss.SeatId,
                    Row = ss.Seat.Row,
                    Number = ss.Seat.Number,
                    IsBooked = ss.IsBooked    
                }).ToList()
            };
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

        public async Task<List<Showtime>> GetShowtimesByMovieAsync(int movieId)
        {
            return await _context.Showtimes
                .Where(s => s.MovieId == movieId)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<ActionResult<ShowtimeDTO>> UpdateAsync(int id, ShowtimeDTO showtimeDTO)
        {
            var existingShowtime = await _context.Showtimes.FirstOrDefaultAsync(d => d.Id == id);
            if (existingShowtime == null)
                throw new ArgumentException("Showtime does not exits");

            existingShowtime.StartTime = showtimeDTO.StartTime;
            existingShowtime.MovieId = showtimeDTO.MovieId;
            existingShowtime.CinemaId = showtimeDTO.CinemaId;

            var movieExists = await _context.Movies.AnyAsync(m => m.Id == showtimeDTO.MovieId);
            var cinemaExists = await _context.Cinemas.AnyAsync(t => t.Id == showtimeDTO.CinemaId);

            if (!movieExists || !cinemaExists)
                throw new ArgumentException("Movie or cinema does not exist");

            _mapper.Map(showtimeDTO, existingShowtime);

            await _context.SaveChangesAsync();

            return _mapper.Map<ShowtimeDTO>(existingShowtime);
        }
    }
}
