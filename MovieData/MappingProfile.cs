using AutoMapper;
using MovieCore.DTOs;
using MovieCore.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<MovieCreateDto, Movie>();
        CreateMap<MovieUpdateDto, Movie>();
        CreateMap<Movie, MovieDto>()
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre != null ? src.Genre.Name : string.Empty))
            .ForMember(dest => dest.GenreId, opt => opt.MapFrom(src => src.GenreId))
            .ForMember(dest => dest.MovieDetails, opt => opt.MapFrom(src => src.MovieDetails));
        CreateMap<MovieDetails, MovieDetailsDto>();
        CreateMap<MovieDetailsPatchDto, MovieDetails>();
    }
}