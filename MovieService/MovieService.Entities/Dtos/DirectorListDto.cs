using MovieService.Core.Entities.Abstract;
using MovieService.Entities.Concrete;
using System.Collections.Generic;

namespace MovieService.Entities.Dtos
{
    public class DirectorListDto : IDto
    {
        public IList<Director> Directors { get; set; }
    }
}