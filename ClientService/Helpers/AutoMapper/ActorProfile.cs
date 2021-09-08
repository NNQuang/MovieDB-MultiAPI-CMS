using AutoMapper;
using ClientService.Areas.Admin.Models;
using ClientService.Areas.Movie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientService.Helpers.AutoMapper
{
    public class ActorProfile : Profile
    {
        public ActorProfile()
        {
            CreateMap<ActorUpdateDto, ActorModel>().ReverseMap();
        }
    }
}
