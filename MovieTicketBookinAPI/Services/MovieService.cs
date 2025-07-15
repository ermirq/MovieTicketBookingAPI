using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookinAPI.Data;
using MovieTicketBookinAPI.DTOs;
using MovieTicketBookinAPI.Mappings;
using MovieTicketBookinAPI.Models;

namespace MovieTicketBookinAPI.Services
{
    public class MovieService : IMovieService
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public MovieService(AppDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<MovieDTO> AddMovie(MovieDTO movieDto)
        {
            var movieEtity = mapper.Map<Movie>(movieDto);

            await context.Movies.AddAsync(movieEtity);
            await context.SaveChangesAsync();

            var createdMovieDto = mapper.Map<MovieDTO>(movieEtity);
            return createdMovieDto;

        }

        public async Task<bool> DeleteMovie(int id)
        {
            var movieToDelete = await context.Movies.FirstOrDefaultAsync(m=>m.Id == id);

            if (movieToDelete == null)
                throw new Exception("NotFound");

            context.Movies.Remove(movieToDelete);
            await context.SaveChangesAsync();

            return true;

        }

        public async Task<MovieDTO> FindAsync(int id)
        {
            var movie = await context.Movies.FirstAsync(x => x.Id == id);
            if (movie == null)
            {
                throw new Exception("NotFound");
            }
             
            return mapper.Map<MovieDTO>(movie);
        }

        public async Task<List<MovieDTO>> SearchMoviesByNameAsync(string name)
        {
            var movies = await context.Movies
                .Where(m => m.Title.Contains(name))
                .ToListAsync();

            if (movies == null)
                return null;

            return mapper.Map<List<MovieDTO>>(movies);
        }


        public async Task<List<MovieDTO>> ToListAsync()
        {
            var movies = await context.Movies.ToListAsync();
            return mapper.Map<List<MovieDTO>>(movies);
        }

        public async Task<MovieDTO> UpdateAsync(int id, MovieDTO movieDto)
        {
            var existingMovie = context.Movies.FirstOrDefault(x => x.Id == id);

            if (existingMovie == null)
            {
                throw new Exception("NotFound");
            }

            existingMovie.Title = movieDto.Title;
            existingMovie.Description = movieDto.Description;
            existingMovie.DurationInMinutes = movieDto.DurationInMinutes;
            existingMovie.Genre = movieDto.Genre;

            mapper.Map(movieDto, existingMovie);

            await context.SaveChangesAsync();

            var updatedMovieDto = mapper.Map<MovieDTO>(existingMovie);
            return updatedMovieDto;
        }
    }
}
