using MovieService.Core.Entities.Abstract;
using System.Collections.Generic;

namespace MovieService.Entities.Concrete
{
    public class Director : BaseEntity, IEntity
    {
        public string FullName { get; set; }
        public string PictureUrl { get; set; } = "https://yoginihead.com/wp-content/uploads/2021/02/thumb_15951118880user1.png";
        public ICollection<Movie> Movies { get; set; }
    }
}