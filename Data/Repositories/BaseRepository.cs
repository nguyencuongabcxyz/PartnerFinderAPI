using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;

namespace Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected DbSet<T> EntitiesSet;

        public BaseRepository(DbContext appDbContext)
        {
            EntitiesSet = appDbContext.Set<T>();
        }

        public async Task Add(T entity)
        {
            await EntitiesSet.AddAsync(entity);
        }

        public async Task AddRange(IEnumerable<T> entities)
        {
            await EntitiesSet.AddRangeAsync(entities);
        }

        public virtual async Task<T> GetOne<TU>(TU id)
        {
            return await EntitiesSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await EntitiesSet.ToListAsync();
        }

        public virtual async Task<T> GetOneByCondition(Expression<Func<T, bool>> expression)
        {
            return await EntitiesSet.Where(expression).FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<T>> GetManyByCondition(Expression<Func<T, bool>> expression)
        {
            return await EntitiesSet.Where(expression).ToListAsync();
        }
        public void Remove(T entity)
        {
            EntitiesSet.Remove(entity);
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            EntitiesSet.RemoveRange(entities);
        }
    }
}
