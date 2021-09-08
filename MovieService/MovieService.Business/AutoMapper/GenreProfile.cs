using AutoMapper;
using MovieService.Entities.Concrete;
using MovieService.Entities.Dtos;

namespace MovieService.Business.AutoMapper
{
    public class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<GenreAddDto, Genre>().ReverseMap();
            CreateMap<GenreUpdateDto, Genre>().ReverseMap();
        }
    }
}