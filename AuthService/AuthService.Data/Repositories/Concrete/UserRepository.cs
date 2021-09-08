using Microsoft.EntityFrameworkCore;
using AuthService.Core.Entities.Concrete;
using AuthService.Core.Repositories.Concrete;
using AuthService.Data.Repositories.Abstract;

namespace AuthService.Data.Repositories.Concrete
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }
    }
}