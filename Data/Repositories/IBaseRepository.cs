using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<int> Count(Expression<Func<T, bool>> condition = null);
        Task Add(T entity);
        Task AddRange(IEnumerable<T> entities);
        Task<T> GetOne<TU>(TU id);
        Task<IEnumerable<T>> GetRange(int index, int size, Expression<Func<T, bool>> condition = null);

        Task<IEnumerable<T>> OrderAndGetRange<TU>(int index, int size, OrderType orderType,
            Expression<Func<T, TU>> orderedKey, Expression<Func<T, bool>> condition);
        Task<IEnumerable<T>> GetAll();
        Task<T> GetOneByCondition(Expression<Func<T, bool>> condition);
        Task<IEnumerable<T>> GetManyByCondition(Expression<Func<T, bool>> condition, int? limit = null);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

    }
}
