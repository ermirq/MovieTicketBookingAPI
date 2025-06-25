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
        public DbSet<Theater> Theaters => Set<Theater>();
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
                 Genre = "Sci-Fi"
             },


             new Movie()
             {
                 Id =2,
                 Title = "The Matrix",
                 Description = "A hacker discovers the true nature of reality",
                 DurationInMinutes = 136,
                 Genre = "Action"
             });

            modelBuilder.Entity<Theater>().HasData(
                             new Theater()
             {
                 Id = 1,
                 Name = "Cineplex 1",
                 Location = "Downtown"
             },
             new Theater()
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
                    TheaterId = 1
                },
                new Showtime()
                {
                    Id = 2,
                    StartTime = new DateTime(2023, 10, 1, 16, 0, 0),
                    MovieId = 2,
                    TheaterId = 2
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


            // Configure other relationships (e.g., Showtime to Movie/Theater, Seat to Theater)
            modelBuilder.Entity<Showtime>()
                 .HasOne(s => s.Theater)
                 .WithMany(t => t.Showtimes)
                 .HasForeignKey(s => s.TheaterId)
                 .OnDelete(DeleteBehavior.Restrict); // 👈 change this from Cascade

            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Theater)
                .WithMany(t => t.Seats)
                .HasForeignKey(s => s.TheaterId)
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
