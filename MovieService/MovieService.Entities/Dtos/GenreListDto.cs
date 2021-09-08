using MovieService.Core.Entities.Abstract;
using MovieService.Entities.Concrete;
using System.Collections.Generic;

namespace MovieService.Entities.Dtos
{
    public class GenreListDto : IDto
    {
        public IList<Genre> Genres { get; set; }
    }
}