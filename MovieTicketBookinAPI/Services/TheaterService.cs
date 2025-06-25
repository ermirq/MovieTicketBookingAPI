using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookinAPI.Data;
using MovieTicketBookinAPI.DTOs;
using MovieTicketBookinAPI.Models;

namespace MovieTicketBookinAPI.Services
{
    public class TheaterService : ITheaterService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public TheaterService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TheaterDTO>> GetAllTheatersAsync()
        {
            var theater = await _context.Theaters.ToListAsync();
            return _mapper.Map<IEnumerable<TheaterDTO>>(theater);
        }

        public async Task<TheaterDTO> FindAsync(int id)
        {
           var theater = await _context.Theaters.FirstOrDefaultAsync(t => t.Id == id);
            if (theater == null)
            {
                throw new Exception("Theater not found.");
            }
            return _mapper.Map<TheaterDTO>(theater);
        }

        public async Task<TheaterDTO> AddAsync(TheaterDTO theaterDto)
        {
            var theaterEntity = _mapper.Map<Theater>(theaterDto);

            await _context.Theaters.AddAsync(theaterEntity);
            await _context.SaveChangesAsync();

            return _mapper.Map<TheaterDTO>(theaterEntity);
        }

        public async Task<TheaterDTO> UpdateAsync(int id, TheaterDTO theaterDto)
        {
            var existingTheater = _context.Theaters.FirstOrDefault(t => t.Id == id);

            if (existingTheater == null)
            {
                throw new Exception("Theater not found.");
            }

            existingTheater.Name = theaterDto.Name;
            existingTheater.Location = theaterDto.Location;

            _mapper.Map(theaterDto, existingTheater);

            await _context.SaveChangesAsync();

            return _mapper.Map<TheaterDTO>(existingTheater);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deleteTheater = await _context.Theaters.FirstOrDefaultAsync(t => t.Id == id);
            if (deleteTheater == null)
            {
                throw new Exception("Theater not found.");
            }

            _context.Theaters.Remove(deleteTheater);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<int> AddSeatsAsync(int theaterId, int numRows, int seatsPerRow)
        {
            var theater = await _context.Theaters
                .Include(t => t.Seats)
                .FirstOrDefaultAsync(t => t.Id == theaterId);

            if (theater == null)
            {
                throw new ArgumentException($"Theater with ID {theaterId} not found.");
            }

            var newSeats = new List<Seat>();

            for (int row = 0; row < numRows; row++)
            {
                char rowLetter = (char)('A' + row);
                for (int num = 1; num <= seatsPerRow; num++)
                {
                    if (!theater.Seats.Any(s => s.Row == rowLetter.ToString() && s.Number == num))
                    {
                        newSeats.Add(new Seat
                        {
                            Row = rowLetter.ToString(),
                            Number = num,
                            TheaterId = theaterId
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
    }
}
