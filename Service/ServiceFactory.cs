
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

        public ServiceFactory(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IUserInformationService CreateUserInformationService()
        {
            return new UserInformationService(_dbContext);
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork(_dbContext);
        }

    }
}
