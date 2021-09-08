using MovieService.Core.Entities.Abstract;
using MovieService.Entities.Concrete;

namespace MovieService.Entities.Dtos
{
    public class MovieDto : IDto
    {
        public Movie Movie { get; set; }
    }
}