using AutoMapper;
using Data;
using Microsoft.EntityFrameworkCore;
using Service.Services;

namespace Service
{
    public interface IServiceFactory
    {
        IUserInformationService CreateUserInformationService();
        ILevelTestService CreateLevelTestService();
        IQuestionService CreateQuestionService();
        IFindingPartnerUserService CreateFindingPartnerUserService();
        IUnitOfWork CreateUnitOfWork();
    }
    public class ServiceFactory : IServiceFactory
    { 
        private readonly DbContext _dbContext;
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IMapper _mapper;

        public ServiceFactory(DbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
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

        public ILevelTestService CreateLevelTestService()
        {
            return new LevelTestService(_repositoryFactory, this);
        }

        public IQuestionService CreateQuestionService()
        {
            return new QuestionService(_repositoryFactory);
        }

        public IFindingPartnerUserService CreateFindingPartnerUserService()
        {
            return new FindingPartnerUserService(_repositoryFactory, _mapper);
        }
    }
}
