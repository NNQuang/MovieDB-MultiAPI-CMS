using AuthService.Data.Context;
using AuthService.Data.Repositories.Abstract;
using AuthService.Data.Repositories.Concrete;
using AuthService.Data.UnitOfWork.Abstract;
using System.Threading.Tasks;

namespace AuthService.Data.UnitOfWork.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuthServiceContext _context;
        private UserRepository _userRepository;

        public UnitOfWork(AuthServiceContext context)
        {
            _context = context;
        }

        public IUserRepository Users => _userRepository ?? new UserRepository(_context);

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