using AutoMapper;
using MovieService.Entities.Concrete;
using MovieService.Entities.Dtos;

namespace MovieService.Business.AutoMapper
{
    public class ActorProfile : Profile
    {
        public ActorProfile()
        {
            CreateMap<ActorAddDto, Actor>().ReverseMap();
            CreateMap<ActorAutoCreateDto, Actor>().ReverseMap();
            CreateMap<ActorUpdateDto, Actor>().ReverseMap();
        }
    }
}