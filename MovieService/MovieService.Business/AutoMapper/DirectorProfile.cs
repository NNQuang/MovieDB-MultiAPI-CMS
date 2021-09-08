using AutoMapper;
using MovieService.Entities.Concrete;
using MovieService.Entities.Dtos;

namespace MovieService.Business.AutoMapper
{
    public class DirectorProfile : Profile
    {
        public DirectorProfile()
        {
            CreateMap<DirectorAddDto, Director>().ReverseMap();
            CreateMap<DirectorAutoCreateDto, Director>().ReverseMap();
            CreateMap<DirectorUpdateDto, Director>().ReverseMap();
        }
    }
}