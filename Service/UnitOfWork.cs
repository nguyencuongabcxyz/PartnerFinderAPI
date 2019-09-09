using System;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public interface IUnitOfWork : IDisposable
    {
        Task Commit();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;

        public UnitOfWork(IDbFactory dbFactory)
        {
            _dbContext = dbFactory.Initialize();
        }

        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
