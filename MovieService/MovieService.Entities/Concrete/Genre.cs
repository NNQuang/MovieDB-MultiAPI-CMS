using MovieService.Core.Entities.Abstract;
using System.Collections.Generic;

namespace MovieService.Entities.Concrete
{
    public class Genre : BaseEntity, IEntity
    {
        public string GenreName { get; set; }
        public ICollection<Movie> Movies { get; set; }
    }
}