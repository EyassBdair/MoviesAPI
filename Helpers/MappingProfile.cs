﻿using AutoMapper;

namespace MoviesAPI.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, MovieDetailsDto>();
               
            CreateMap<MovesDto, Movie>()
               .ForMember(src => src.Poster, opt => opt.Ignore());

        }
    }
}