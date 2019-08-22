using System;
using System.Collections.Generic;
using System.Text;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public interface IRepositoryFactory
    {
        IUserInformationRepository CreateUserInformationRepo();
        IUserRepository CreateUserRepo();
    }

    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly DbContext _dbContext;

        public RepositoryFactory(DbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IUserInformationRepository CreateUserInformationRepo()
        {
            return new UserInformationRepository(_dbContext);
        }

        public IUserRepository CreateUserRepo()
        {
            return new UserRepository(_dbContext);
        }
    }
}
