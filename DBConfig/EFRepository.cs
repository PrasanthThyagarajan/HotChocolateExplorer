using HotChocolateExplorer.DBConfig.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotChocolateExplorer.DBConfig
{
    /// <summary>
    /// "There's some repetition here - couldn't we have some the sync methods call the async?"
    /// https://blogs.msdn.microsoft.com/pfxteam/2012/04/13/should-i-expose-synchronous-wrappers-for-asynchronous-methods/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EfRepository<T> : IAsyncRepository<T> where T : class
    {
        protected readonly SchoolContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public EfRepository(SchoolContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet.AsQueryable();
            query = includeProperties.Aggregate(query, (current, include) => current.Include(include));

            return await query.Where(filter).ToListAsync();
        }

        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet.AsQueryable();
            query = includeProperties.Aggregate(query, (current, include) => current.Include(include));

            return await query.Where(filter).FirstOrDefaultAsync();
        }

        public virtual async Task<T> LastOrDefaultAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> sort, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet.AsQueryable();
            query = includeProperties.Aggregate(query, (current, include) => current.Include(include));

            return await query.Where(filter).OrderByDescending(sort).FirstOrDefaultAsync();
        }

        public virtual async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet.AsQueryable();
            query = includeProperties.Aggregate(query, (current, include) => current.Include(include));

            return await query.Where(filter).SingleOrDefaultAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<List<T>> ListAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<List<T>> ListAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet.AsQueryable();
            query = includeProperties.Aggregate(query, (current, include) => current.Include(include));

            return await query.ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            _dbSet.Add(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<T>> AddManyAsync(IEnumerable<T> entities)
        {
            if (entities.Any())
            {
                _dbSet.AddRange(entities);
                await _dbContext.SaveChangesAsync();
            }

            return entities;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        public async Task<int> UpdateAsync(T entity, params Expression<Func<T, object>>[] updatedProperties)
        {
            if (updatedProperties != null && updatedProperties.Any())
            {
                var dbEntry = _dbContext.Entry(entity);

                foreach (var property in updatedProperties)
                {
                    dbEntry.Property(property).IsModified = true;
                }
            }
            else
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
            }

            return await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateManyAsync(IEnumerable<T> entities)
        {
            _dbContext.UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteManyAsync(IEnumerable<T> entities)
        {
            if (entities.Any())
            {
                _dbContext.RemoveRange(entities);
                await _dbContext.SaveChangesAsync();
            }
        }
        public virtual async Task<IEnumerable<M>> SelectAsync<M>(Expression<Func<T, bool>> filter, Expression<Func<T, M>> selectFilter, params Expression<Func<T, object>>[] includeProperties) where M : class
        {
            IQueryable<T> query = _dbSet.AsQueryable();
            query = includeProperties.Aggregate(query, (current, include) => current.Include(include));



            return await query.Where(filter).Select(selectFilter).ToListAsync();
        }
    }
}
