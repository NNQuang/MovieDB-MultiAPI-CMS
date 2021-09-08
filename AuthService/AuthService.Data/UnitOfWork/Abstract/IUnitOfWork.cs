using AuthService.Data.Repositories.Abstract;
using System;
using System.Threading.Tasks;

namespace AuthService.Data.UnitOfWork.Abstract
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IUserRepository Users { get; }

        Task<int> SaveAsync();
    }
}