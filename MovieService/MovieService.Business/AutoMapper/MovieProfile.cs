using AutoMapper;
using MovieService.Entities.Concrete;
using MovieService.Entities.Dtos;
using System;

namespace MovieService.Business.AutoMapper
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<MovieAddDto, Movie>().ReverseMap();
            CreateMap<MovieUpdateDto, Movie>().ReverseMap();
        }
    }
}