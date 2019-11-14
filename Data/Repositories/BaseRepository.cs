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

        public BaseRepository(IDbFactory dbFactory)
        {
            EntitiesSet = dbFactory.Initialize().Set<T>();
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

        public async Task<IEnumerable<T>> GetRange(int index, int size, Expression<Func<T, bool>> condition = null)
        {
            var skipCount = index * size;
            if (condition == null)
            {
                return await EntitiesSet.Skip(skipCount).Take(size).ToListAsync();
            }
            return await EntitiesSet.Where(condition).Skip(skipCount).Take(size).ToListAsync();
        }

        public async Task<IEnumerable<T>> OrderAndGetRange<TU>(int index, int size, OrderType orderType, Expression<Func<T, TU>> orderedKey, Expression<Func<T, bool>> condition)
        {
            var skipCount = index * size;
            if (condition == null)
            {
                if (orderType == OrderType.OrderByAscending)
                {
                    return await EntitiesSet.OrderBy(orderedKey).Skip(skipCount).Take(size).ToListAsync();
                }
                return await EntitiesSet.OrderByDescending(orderedKey).Skip(skipCount).Take(size).ToListAsync();
            }

            if (orderType == OrderType.OrderByAscending)
            {
                return await EntitiesSet.Where(condition).OrderBy(orderedKey).Skip(skipCount).Take(size).ToListAsync();
            }
            return await EntitiesSet.Where(condition).OrderByDescending(orderedKey).Skip(skipCount).Take(size).ToListAsync();
        }


        public virtual async Task<T> GetOneByCondition(Expression<Func<T, bool>> condition)
        {
            return await EntitiesSet.Where(condition).FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<T>> GetManyByCondition(Expression<Func<T, bool>> condition, int? limit = null)
        {
            if (limit == null)
                return await EntitiesSet.Where(condition).ToListAsync();
            return await EntitiesSet.Where(condition).Take((int)limit).ToListAsync();
        }
        public void Remove(T entity)
        {
            EntitiesSet.Remove(entity);
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            EntitiesSet.RemoveRange(entities);
        }

        public virtual async Task<int> Count(Expression<Func<T, bool>> condition = null)
        {
            if (condition == null)
            {
                return await EntitiesSet.CountAsync();
            }
            return await EntitiesSet.Where(condition).CountAsync();
        }
    }

    public enum OrderType
    {
        OrderByDescending,
        OrderByAscending
    }
}
