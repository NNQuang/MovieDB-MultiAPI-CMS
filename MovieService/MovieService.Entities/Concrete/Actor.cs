using MovieService.Core.Entities.Abstract;
using System.Collections.Generic;

namespace MovieService.Entities.Concrete
{
    public class Actor : BaseEntity, IEntity
    {
        public string FullName { get; set; }
        public IList<Movie> Movies { get; set; }
        public string PictureUrl { get; set; } = "https://yoginihead.com/wp-content/uploads/2021/02/thumb_15951118880user1.png";
    }
}