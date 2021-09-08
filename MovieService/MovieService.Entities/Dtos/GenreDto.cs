using MovieService.Core.Entities.Abstract;
using MovieService.Entities.Concrete;

namespace MovieService.Entities.Dtos
{
    public class GenreDto : IDto
    {
        public Genre Genre { get; set; }
    }
}