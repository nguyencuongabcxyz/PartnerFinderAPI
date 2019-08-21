using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Data.Models;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public interface IUnitOfWork : IDisposable
    {
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Dispose()
        {
            _dbContext.SaveChanges();
            _dbContext.Dispose();
        }
    }
}
