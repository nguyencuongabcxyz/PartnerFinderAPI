
using Data;
using Microsoft.EntityFrameworkCore;
using Service.Services;

namespace Service
{
    public interface IServiceFactory
    {
        IUserInformationService CreateUserInformationService();
        IUnitOfWork CreateUnitOfWork();
    }
    public class ServiceFactory : IServiceFactory
    { 
        private readonly DbContext _dbContext;
        private readonly IRepositoryFactory _repositoryFactory;

        public ServiceFactory(DbContext dbContext)
        {
            _dbContext = dbContext;
            _repositoryFactory = new RepositoryFactory(_dbContext);
        }

        public IUserInformationService CreateUserInformationService()
        {
            return new UserInformationService(_repositoryFactory);
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork(_dbContext);
        }

    }
}
