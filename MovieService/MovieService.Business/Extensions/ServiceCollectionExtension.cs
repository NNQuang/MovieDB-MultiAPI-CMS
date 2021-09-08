using Microsoft.Extensions.DependencyInjection;
using MovieService.Business.Abstract;
using MovieService.Business.Concrete;
using MovieService.Data.UnitOfWork.Abstract;
using MovieService.Data.UnitOfWork.Concrete;

namespace MovieService.Business.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection LoadMyServices(this IServiceCollection serviceCollection)
        {
            //serviceCollection.AddDbContext<MovieDbContext>(options => options.UseSqlServer(connectionString));
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<IActorService, ActorManager>();
            serviceCollection.AddScoped<IGenreService, GenreManager>();
            serviceCollection.AddScoped<IMovieService, MovieManager>();
            serviceCollection.AddScoped<IDirectorService, DirectorManager>();

            return serviceCollection;
        }
    }
}