using Microsoft.EntityFrameworkCore;
using MovieService.Core.Repositories.Concrete;
using MovieService.Data.Repositories.Abstract;
using MovieService.Entities.Concrete;

namespace MovieService.Data.Repositories.Concrete
{
    public class DirectorRepository : BaseRepository<Director>, IDirectorRepository
    {
        public DirectorRepository(DbContext context) : base(context)
        {
        }
    }
}