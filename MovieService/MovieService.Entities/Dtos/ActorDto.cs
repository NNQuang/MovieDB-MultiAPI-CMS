using MovieService.Core.Entities.Abstract;
using MovieService.Entities.Concrete;

namespace MovieService.Entities.Dtos
{
    public class ActorDto : IDto
    {
        public Actor Actor { get; set; }
    }
}