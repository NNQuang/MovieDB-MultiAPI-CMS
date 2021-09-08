using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AuthService.Core.Entities.Abstract;

namespace AuthService.Core.Repositories.Abstract
{
    public interface IBaseRepository<T> where T: IBaseEntity
    {
        Task<T> GetAsync(Expression<Func<T, bool>> filter,params Expression<Func<T, object>>[] include);
        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> filter=null, params Expression<Func<T, object>>[] include);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);

    }
}
