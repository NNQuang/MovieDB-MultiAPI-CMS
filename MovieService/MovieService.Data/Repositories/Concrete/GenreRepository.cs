using Microsoft.EntityFrameworkCore;
using MovieService.Core.Repositories.Concrete;
using MovieService.Data.Repositories.Abstract;
using MovieService.Entities.Concrete;

namespace MovieService.Data.Repositories.Concrete
{
    public class GenreRepository : BaseRepository<Genre>, IGenreRepository
    {
        public GenreRepository(DbContext context) : base(context)
        {
        }
    }
}