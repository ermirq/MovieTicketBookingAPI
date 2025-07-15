using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookinAPI.Data;
using MovieTicketBookinAPI.DTOs;
using MovieTicketBookinAPI.Models;

namespace MovieTicketBookinAPI.Services
{
    public class CinemaService : ICinemaService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public CinemaService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CinemaDTO>> GetAllCinemasAsync()
        {
            var cinemas = await _context.Cinemas
               .Include(c => c.Showtimes)
                   .ThenInclude(s => s.Movie)
               .ToListAsync();
            return _mapper.Map<IEnumerable<CinemaDTO>>(cinemas);
        }

        public async Task<CinemaDTO> FindAsync(int id)
        {
           var cinema = await _context.Cinemas.FirstOrDefaultAsync(t => t.Id == id);
            if (cinema == null)
            {
                throw new Exception("Cinema not found.");
            }
            return _mapper.Map<CinemaDTO>(cinema);
        }

        public async Task<CinemaDTO> AddAsync(CinemaDTO cinemaDto)
        {
            var cinemaEntity = _mapper.Map<Cinema>(cinemaDto);

            await _context.Cinemas.AddAsync(cinemaEntity);
            await _context.SaveChangesAsync();

            return _mapper.Map<CinemaDTO>(cinemaEntity);
        }

        public async Task<CinemaDTO> UpdateAsync(int id, CinemaDTO cinemaDto)
        {
            var existingCinema = _context.Cinemas.FirstOrDefault(t => t.Id == id);

            if (existingCinema == null)
            {
                throw new Exception("Cinema not found.");
            }

            existingCinema.Name = cinemaDto.Name;
            existingCinema.Location = cinemaDto.Location;

            _mapper.Map(cinemaDto, existingCinema);

            await _context.SaveChangesAsync();

            return _mapper.Map<CinemaDTO>(existingCinema);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deleteCinema = await _context.Cinemas.FirstOrDefaultAsync(t => t.Id == id);
            if (deleteCinema == null)
            {
                throw new Exception("Cinema not found.");
            }

            _context.Cinemas.Remove(deleteCinema);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<int> AddSeatsAsync(int cinemaId, int numRows, int seatsPerRow)
        {
            var cinema = await _context.Cinemas
                .Include(t => t.Seats)
                .FirstOrDefaultAsync(t => t.Id == cinemaId);

            if (cinema == null)
            {
                throw new ArgumentException($"Cinema with ID {cinemaId} not found.");
            }

            var newSeats = new List<Seat>();

            for (int row = 0; row < numRows; row++)
            {
                char rowLetter = (char)('A' + row);
                for (int num = 1; num <= seatsPerRow; num++)
                {
                    if (!cinema.Seats.Any(s => s.Row == rowLetter.ToString() && s.Number == num))
                    {
                        newSeats.Add(new Seat
                        {
                            Row = rowLetter.ToString(),
                            Number = num,
                            CinemaId = cinemaId
                        });
                    }
                }
            }

            if (!newSeats.Any())
            {
                throw new InvalidOperationException("All requested seats already exist.");
            }

            _context.Seats.AddRange(newSeats);
            await _context.SaveChangesAsync();

            return newSeats.Count;
        }

        public async Task<List<CinemaDTO>> GetCinemasWithShowtimesAsync()
        {
            var cinemas = await _context.Cinemas
                .Include(c => c.Showtimes)
                    .ThenInclude(s => s.Movie)
                .ToListAsync();

            return _mapper.Map<List<CinemaDTO>>(cinemas);
        }
    }
}
