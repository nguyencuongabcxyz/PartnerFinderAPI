using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public interface IRepositoryFactory
    {
        IUserInformationRepository CreateUserInformationRepo();
        IUserRepository CreateUserRepo();
        ILevelTestRepository CreateLevelTestRepo();
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

        public ILevelTestRepository CreateLevelTestRepo()
        {
            return new LevelTestRepository(_dbContext);
        }
    }
}
