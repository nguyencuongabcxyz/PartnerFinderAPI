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
            var retrievedObject = await EntitiesSet.FindAsync(id);
            if(retrievedObject == null) throw new ObjectNotFoundException("Object can not be found with given id!");
            return await EntitiesSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await EntitiesSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetRange(int index, int size)
        {
            var skipCount = index * size;
            return await EntitiesSet.Skip(skipCount).Take(size).ToListAsync();
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

        public async Task<int> Count()
        {
            return await EntitiesSet.CountAsync();
        }

        public async Task<IEnumerable<T>> GetRangeWithCondition(int index, int size, Expression<Func<T, bool>> expression)
        {
            var skipCount = index * size;
            return await EntitiesSet.Where(expression).Skip(skipCount).Take(size).ToListAsync();
        }

        public async Task<IEnumerable<T>> OrderBeforeGetRange(int index, int size, Expression<Func<T, bool>> expression)
        {
            var skipCount = index * size;
            return await EntitiesSet.Where(expression).Skip(skipCount).Take(size).ToListAsync();
        }
    }
}
