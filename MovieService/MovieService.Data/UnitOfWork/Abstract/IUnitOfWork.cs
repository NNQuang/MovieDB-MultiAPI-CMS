using MovieService.Data.Repositories.Abstract;
using System;
using System.Threading.Tasks;

namespace MovieService.Data.UnitOfWork.Abstract
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IActorRepository Actors { get; }
        IDirectorRepository Directors { get; }
        IGenreRepository Genres { get; }
        IMovieRepository Movies { get; }

        Task<int> SaveAsync();
    }
}