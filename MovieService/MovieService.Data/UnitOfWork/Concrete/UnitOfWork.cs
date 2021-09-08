using MovieService.Data.Context;
using MovieService.Data.Repositories.Abstract;
using MovieService.Data.Repositories.Concrete;
using MovieService.Data.UnitOfWork.Abstract;
using System.Threading.Tasks;

namespace MovieService.Data.UnitOfWork.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MovieDbContext _context;
        private ActorRepository _actorRepository;
        private DirectorRepository _directorRepository;
        private GenreRepository _genreRepository;
        private MovieRepository _movieRepository;

        public UnitOfWork(MovieDbContext context)
        {
            _context = context;
        }

        public IActorRepository Actors => _actorRepository ?? new ActorRepository(_context);

        public IDirectorRepository Directors => _directorRepository ?? new DirectorRepository(_context);

        public IGenreRepository Genres => _genreRepository ?? new GenreRepository(_context);

        public IMovieRepository Movies => _movieRepository ?? new MovieRepository(_context);

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}