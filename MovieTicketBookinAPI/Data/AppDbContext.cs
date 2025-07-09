using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookinAPI.Models;
using static System.Net.WebRequestMethods;

namespace MovieTicketBookinAPI.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Movie> Movies => Set<Movie>();
        public DbSet<Cinema> Cinemas => Set<Cinema>();
        public DbSet<Showtime> Showtimes => Set<Showtime>();
        public DbSet<Seat> Seats => Set<Seat>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<BookingSeat> BookingSeats => Set<BookingSeat>();
        public DbSet<ShowtimeSeat> ShowtimeSeats => Set<ShowtimeSeat>();
        public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movie>().HasData(
             new Movie()
             {
                 Id = 1,
                 Title = "Inception",
                 Description = "A mind-bending thriller",
                 DurationInMinutes = 148,
                 Genre = "Sci-Fi",
                 PosterUrl = "https://m.media-amazon.com/images/M/MV5BMjAxMzY3NjcxNF5BMl5BanBnXkFtZTcwNTI5OTM0Mw@@._V1_.jpg"
             },

             new Movie()
             {
                 Id = 2,
                 Title = "The Matrix",
                 Description = "A hacker discovers the true nature of reality",
                 DurationInMinutes = 136,
                 Genre = "Action",
                 PosterUrl = "https://m.media-amazon.com/images/I/51oQqzZoZpL._UF1000,1000_QL80_.jpg"
             },

            new Movie()
            {
                Id = 3,
                Title = "Pirates of the Caribbean: The Curse of the Black Pearl",
                Description = "A 2003 fantasy adventure film about a blacksmith, Will Turner, who teams up with the eccentric pirate Captain Jack Sparrow to rescue the kidnapped Governor's daughter, Elizabeth Swann, from the cursed pirate Captain Barbossa and his undead crew.",
                Genre = "Sci-Fi",
                DurationInMinutes = 143,
                PosterUrl = "https://m.media-amazon.com/images/I/916kucr5MCS.jpg"
            },

            new Movie()
            {
                Id = 4,
                Title = "Pirates of the Caribbean: Dead Man's Chest",
                Description = "Follows Captain Jack Sparrow as he tries to escape his debt to Davy Jones, the fearsome captain of the Flying Dutchman, whose heart is locked away in the titular Dead Man's Chest",
                Genre = "Action, Comedy, Adventure",
                DurationInMinutes = 151,
                PosterUrl = "https://s3.us-east-2.amazonaws.com/media.trendsinternational.com/8732-SIL22X34IMAGE1.jpg"
            });

            modelBuilder.Entity<Cinema>().HasData(
                             new Cinema()
             {
                 Id = 1,
                 Name = "Cineplex 1",
                 Location = "Downtown"
             },
             new Cinema()
             {
                 Id = 2,
                 Name = "Cineplex 2",
                 Location = "Uptown"
             });

            modelBuilder.Entity<Showtime>().HasData(
                new Showtime()
                {
                    Id = 1,
                    StartTime = new DateTime(2023, 10, 1, 14, 0, 0),
                    MovieId = 1,
                    CinemaId = 1
                },
                new Showtime()
                {
                    Id = 2,
                    StartTime = new DateTime(2023, 10, 1, 16, 0, 0),
                    MovieId = 2,
                    CinemaId = 2
                });

            // Configure ShowtimeSeat (Many-to-Many between Showtime and Seat)
            // Configure ShowtimeSeat (Primary Key)
            modelBuilder.Entity<ShowtimeSeat>()
                .HasKey(ss => new { ss.ShowtimeId, ss.SeatId }); // Composite PK for ShowtimeSeat

            modelBuilder.Entity<ShowtimeSeat>()
                .HasOne(ss => ss.Showtime)
                .WithMany(s => s.ShowtimeSeats)
                .HasForeignKey(ss => ss.ShowtimeId)
                .OnDelete(DeleteBehavior.Cascade); // Often Cascade here is fine: if Showtime deleted, its ShowtimeSeats are deleted

            modelBuilder.Entity<ShowtimeSeat>()
                .HasOne(ss => ss.Seat)
                .WithMany(s => s.ShowtimeSeats)
                .HasForeignKey(ss => ss.SeatId)
                .OnDelete(DeleteBehavior.Cascade); // Often Cascade here is fine: if physical Seat deleted, its ShowtimeSeats are deleted


            /// Configure BookingSeat (Primary Key)
            modelBuilder.Entity<BookingSeat>()
                .HasKey(bs => new { bs.BookingId, bs.ShowtimeId, bs.SeatId });

            // Configure the relationship from BookingSeat to ShowtimeSeat
            // THIS IS CRUCIAL: Set OnDelete(DeleteBehavior.NoAction)
            modelBuilder.Entity<BookingSeat>()
                .HasOne(bs => bs.ShowtimeSeat)
                .WithMany(ss => ss.BookingSeats)
                .HasForeignKey(bs => new { bs.ShowtimeId, bs.SeatId })
                .OnDelete(DeleteBehavior.NoAction); // <-- MUST BE HERE


            // Configure other relationships (e.g., Showtime to Movie/Cinema, Seat to Cinema)
            modelBuilder.Entity<Showtime>()
                 .HasOne(s => s.Cinema)
                 .WithMany(t => t.Showtimes)
                 .HasForeignKey(s => s.CinemaId)
                 .OnDelete(DeleteBehavior.Restrict); // 👈 change this from Cascade

            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Cinema)
                .WithMany(t => t.Seats)
                .HasForeignKey(s => s.CinemaId)
                .OnDelete(DeleteBehavior.Restrict); // 👈 or leave one with Cascade if preferred


            // Configure Booking relationships
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany() // Or WithMany(u => u.Bookings) if ApplicationUser has collection
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.ShowTime) // Ensure this matches property name `ShowTime` in Booking
                .WithMany() // Or WithMany(s => s.Bookings) if Showtime has a collection
                .HasForeignKey(b => b.ShowtimeId);
            
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Bookings) // Assuming ApplicationUser has a collection of Bookings
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevents cascade delete from ApplicationUser to Booking
        }
    }
}
