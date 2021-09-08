using MovieService.Core.Repositories.Abstract;
using MovieService.Entities.Concrete;

namespace MovieService.Data.Repositories.Abstract
{
    public interface IMovieRepository : IRepository<Movie>
    {
    }
}