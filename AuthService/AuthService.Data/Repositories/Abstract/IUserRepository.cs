using AuthService.Core.Entities.Concrete;
using AuthService.Core.Repositories.Abstract;

namespace AuthService.Data.Repositories.Abstract
{
    public interface IUserRepository : IBaseRepository<User>
    {
    }
}