using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;

namespace MoviesApi.Helpers
{
	public class MappingProfile : Profile
	{
        public MappingProfile()
        {
            CreateMap<Movie, MovieDetailsDto>();
            CreateMap<MovieDto, Movie>()
                .ForMember(m => m.Poster, opt => opt.Ignore());

            CreateMap<RegisterDto, ApplicationUser>();
                //.ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                //.ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
                //.ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
                //.ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
                //.ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
                //.ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
                //.ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore())
                //.ForMember(dest => dest.Id, opt => opt.Ignore())
                //.ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
                //.ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
                //.ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore());

		}
    }
}
