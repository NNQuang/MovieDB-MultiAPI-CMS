using Microsoft.EntityFrameworkCore;
using MovieService.Core.Repositories.Concrete;
using MovieService.Data.Repositories.Abstract;
using MovieService.Entities.Concrete;

namespace MovieService.Data.Repositories.Concrete
{
    public class ActorRepository : BaseRepository<Actor>, IActorRepository
    {
        public ActorRepository(DbContext context) : base(context)
        {
        }
    }
}