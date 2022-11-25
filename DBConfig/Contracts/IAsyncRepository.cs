using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotChocolateExplorer.DBConfig.Contracts
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties);
        Task<T> LastOrDefaultAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> sort, params Expression<Func<T, object>>[] includeProperties);
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetByIdAsync(int id);
        Task<List<T>> ListAllAsync();
        Task<List<T>> ListAllAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> AddManyAsync(IEnumerable<T> entities);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task UpdateManyAsync(IEnumerable<T> entities);
        Task<int> UpdateAsync(T entity, params Expression<Func<T, object>>[] updatedProperties);
        Task DeleteAsync(T entity);
        Task DeleteManyAsync(IEnumerable<T> entities);
        Task<IEnumerable<M>> SelectAsync<M>(Expression<Func<T, bool>> filter, Expression<Func<T, M>> selectFilter, params Expression<Func<T, object>>[] includeProperties) where M : class;
    }
}
