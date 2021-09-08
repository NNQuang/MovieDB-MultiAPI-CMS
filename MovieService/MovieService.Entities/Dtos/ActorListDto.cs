using MovieService.Core.Entities.Abstract;
using MovieService.Entities.Concrete;
using System.Collections.Generic;

namespace MovieService.Entities.Dtos
{
    public class ActorListDto : IDto
    {
        public IList<Actor> Actors { get; set; }
    }
}