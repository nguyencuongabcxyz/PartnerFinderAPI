using Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private AppDbContext _appDbContext;
        protected DbSet<T> EntitiesSet;

        public BaseRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            EntitiesSet = _appDbContext.Set<T>();
        }

        public async Task Add(T entity)
        {
            entity.GetType().GetProperty("IsDeleted").SetValue(entity, true);
            await EntitiesSet.AddAsync(entity);
            //entity.GetType().GetProperty("IsDeleted").SetValue(entity, false);
        }

        public async Task AddRange(IEnumerable<T> entities)
        {
            await EntitiesSet.AddRangeAsync(entities);
        }

        public virtual async Task<T> GetOne<U>(U id)
        {
            return await EntitiesSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await EntitiesSet.ToListAsync();
        }

        public virtual async Task<T> GetOneByCondition(Expression<Func<T, bool>> expression)
        {
            return await EntitiesSet.Where<T>(expression).FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<T>> GetManyByCondition(Expression<Func<T, bool>> expression)
        {
            return await EntitiesSet.Where<T>(expression).ToListAsync();
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
