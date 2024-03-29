﻿using AutoMapper;
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
            
		}
    }
}
