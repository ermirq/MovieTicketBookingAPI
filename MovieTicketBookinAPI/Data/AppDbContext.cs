using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookinAPI.Models;

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

            // ✅ Seeding
            modelBuilder.Entity<Movie>().HasData(
                new Movie ()
                { 
                    Id = 1,
                    Title = "Inception",
                    Description = "A mind-bending thriller",
                    DurationInMinutes = 148, Genre = "Sci-Fi",
                    PosterUrl = "https://m.media-amazon.com/images/M/MV5BMjAxMzY3NjcxNF5BMl5BanBnXkFtZTcwNTI5OTM0Mw@@._V1_.jpg" 
                },
                new Movie ()
                {
                    Id = 2,
                    Title = "The Matrix",
                    Description = "A hacker discovers the true nature of reality",
                    DurationInMinutes = 136, Genre = "Action",
                    PosterUrl = "https://m.media-amazon.com/images/I/51oQqzZoZpL._UF1000,1000_QL80_.jpg" 
                },
                new Movie ()
                {
                    Id = 3,
                    Title = "Pirates of the Caribbean: The Curse of the Black Pearl",
                    Description = "A 2003 fantasy adventure film about a blacksmith, Will Turner, who teams up with the eccentric pirate Captain Jack Sparrow to rescue the kidnapped Governor's daughter, Elizabeth Swann, from the cursed pirate Captain Barbossa and his undead crew.",
                    DurationInMinutes = 143,
                    Genre = "Sci-Fi",
                    PosterUrl = "https://m.media-amazon.com/images/I/916kucr5MCS.jpg" 
                },
                new Movie ()
                { 
                    Id = 4,
                    Title = "Pirates of the Caribbean: Dead Man's Chest",
                    Description = "Follows Captain Jack Sparrow as he tries to escape his debt to Davy Jones, the fearsome captain of the Flying Dutchman, whose heart is locked away in the titular Dead Man's Chest",
                    DurationInMinutes = 151,
                    Genre = "Action, Comedy, Adventure",
                    PosterUrl = "https://s3.us-east-2.amazonaws.com/media.trendsinternational.com/8732-SIL22X34IMAGE1.jpg" }
            );

            modelBuilder.Entity<Cinema>().HasData(
                new Cinema { Id = 1, Name = "Cineplex 1", Location = "Downtown" },
                new Cinema { Id = 2, Name = "Cineplex 2", Location = "Uptown" }
            );

            // ✅ Relationships

            // 🎬 Showtime - Movie & Cinema
            modelBuilder.Entity<Showtime>()
                .HasOne(s => s.Movie)
                .WithMany(m => m.Showtimes)
                .HasForeignKey(s => s.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Showtime>()
                .HasOne(s => s.Cinema)
                .WithMany(c => c.Showtimes)
                .HasForeignKey(s => s.CinemaId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🪑 Seat - Cinema
            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Cinema)
                .WithMany(c => c.Seats)
                .HasForeignKey(s => s.CinemaId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🎟 ShowtimeSeat - Composite PK and FK
            modelBuilder.Entity<ShowtimeSeat>()
                .HasKey(ss => new { ss.ShowtimeId, ss.SeatId });

            modelBuilder.Entity<ShowtimeSeat>()
                .HasOne(ss => ss.Showtime)
                .WithMany(s => s.ShowtimeSeats)
                .HasForeignKey(ss => ss.ShowtimeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ShowtimeSeat>()
                .HasOne(ss => ss.Seat)
                .WithMany(s => s.ShowtimeSeats)
                .HasForeignKey(ss => ss.SeatId)
                .OnDelete(DeleteBehavior.Cascade);

            // 📘 Booking - User & Showtime
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.ShowTime)
                .WithMany()
                .HasForeignKey(b => b.ShowtimeId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🎫 BookingSeat - Composite Key and relations
            modelBuilder.Entity<BookingSeat>()
                .HasKey(bs => new { bs.BookingId, bs.ShowtimeId, bs.SeatId });

            modelBuilder.Entity<BookingSeat>()
                .HasOne(bs => bs.Booking)
                .WithMany(b => b.BookingSeats)
                .HasForeignKey(bs => bs.BookingId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<BookingSeat>()
                .HasOne(bs => bs.ShowtimeSeat)
                .WithMany(ss => ss.BookingSeats)
                .HasForeignKey(bs => new { bs.ShowtimeId, bs.SeatId })
                .OnDelete(DeleteBehavior.NoAction); // Avoid composite FK conflicts
        }
    }
}
