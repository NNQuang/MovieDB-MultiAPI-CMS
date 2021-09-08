using MovieService.Core.Entities.Abstract;
using MovieService.Entities.Concrete;
using System.Collections.Generic;

namespace MovieService.Entities.Dtos
{
    public class MovieListDto : IDto
    {
        public IList<Movie> Movies { get; set; }
        public string Note { get; set; } // server'dan gelen dataya note olarak veri ekliyoruz (director, genre, actor)
    }
}