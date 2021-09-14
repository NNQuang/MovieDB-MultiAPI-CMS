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
            serviceCollection.AddSingleton<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddSingleton<IActorService, ActorManager>();
            serviceCollection.AddSingleton<IGenreService, GenreManager>();
            serviceCollection.AddSingleton<IMovieService, MovieManager>();
            serviceCollection.AddSingleton<IDirectorService, DirectorManager>();

            return serviceCollection;
        }
    }
}