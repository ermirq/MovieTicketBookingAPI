using AutoMapper;
using MovieTicketBookinAPI.DTOs;
using MovieTicketBookinAPI.Models;
using System.Runtime;

namespace MovieTicketBookinAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserDTO>().ReverseMap().ForMember(dest => dest.Id, opt => opt.Ignore());  
            CreateMap<Movie, MovieDTO>().ReverseMap().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Theater, TheaterDTO>().ReverseMap().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Showtime, ShowtimeDTO>().ReverseMap().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Seat, SeatDTO>().ReverseMap().ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<ShowtimeSeat, SeatDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Seat.Id))
                .ForMember(dest => dest.Row, opt => opt.MapFrom(src => src.Seat.Row))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Seat.Number));
            //.ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));

            CreateMap<Booking, BookingRequestDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.ShowtimeId, opt => opt.MapFrom(src => src.ShowtimeId))
                .ForMember(dest => dest.SeatNumbers, opt =>
                    opt.MapFrom(src => src.BookingSeats.Select(bs => bs.SeatNumber)));          

            CreateMap<BookingRequestDTO, Booking>();
        }
    }
}
