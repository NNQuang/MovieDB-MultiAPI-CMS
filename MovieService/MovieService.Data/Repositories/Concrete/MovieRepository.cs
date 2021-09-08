using Microsoft.EntityFrameworkCore;
using MovieService.Core.Repositories.Concrete;
using MovieService.Data.Repositories.Abstract;
using MovieService.Entities.Concrete;

namespace MovieService.Data.Repositories.Concrete
{
    public class MovieRepository : BaseRepository<Movie>, IMovieRepository
    {
        public MovieRepository(DbContext context) : base(context)
        {
        }
    }
}