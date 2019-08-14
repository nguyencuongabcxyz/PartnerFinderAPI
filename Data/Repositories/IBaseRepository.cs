using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task Add(T entity);
        Task AddRange(IEnumerable<T> entities);
        Task<T> GetOne<TU>(TU id);
        Task<IEnumerable<T>> GetAll();
        Task<T> GetOneByCondition(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetManyByCondition(Expression<Func<T, bool>> expression);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

    }
}
