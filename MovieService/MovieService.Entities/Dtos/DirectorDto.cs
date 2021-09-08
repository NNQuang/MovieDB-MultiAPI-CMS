using MovieService.Core.Entities.Abstract;
using MovieService.Entities.Concrete;

namespace MovieService.Entities.Dtos
{
    public class DirectorDto : IDto
    {
        public Director Director { get; set; }
    }
}