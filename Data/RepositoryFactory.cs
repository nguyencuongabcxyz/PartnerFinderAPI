//using Data.Repositories;
//using Microsoft.EntityFrameworkCore;

//namespace Data
//{
//    public interface IRepositoryFactory
//    {
//        IUserInformationRepository CreateUserInformationRepo();
//        IUserRepository CreateUserRepo();
//        ILevelTestRepository CreateLevelTestRepo();
//        IQuestionRepository CreateQuestionRepo();
//        IAnswerOptionRepository CreateAnswerOptionRepo();
//        IFindingPartnerUserRepository CreateFindingPartnerUserRepo();
//        IPostRepository CreatePostRepo();
//        ICommentRepository CreateCommentRepo();
//    }

//    public class RepositoryFactory : IRepositoryFactory
//    {
//        private readonly DbContext _dbContext;

//        public RepositoryFactory(DbContext dbContext)
//        {
//            _dbContext = dbContext;
//        }


//        public IUserInformationRepository CreateUserInformationRepo()
//        {
//            return new UserInformationRepository(_dbContext);
//        }

//        public IUserRepository CreateUserRepo()
//        {
//            return new UserRepository(_dbContext);
//        }

//        public ILevelTestRepository CreateLevelTestRepo()
//        {
//            return new LevelTestRepository(_dbContext);
//        }

//        public IQuestionRepository CreateQuestionRepo()
//        {
//            return new QuestionRepository(_dbContext);
//        }

//        public IAnswerOptionRepository CreateAnswerOptionRepo()
//        {
//            return new AnswerOptionRepository(_dbContext);
//        }

//        public IFindingPartnerUserRepository CreateFindingPartnerUserRepo()
//        {
//            return new FindingPartnerUserRepository(_dbContext);
//        }

//        public IPostRepository CreatePostRepo()
//        {
//            return new PostRepository(_dbContext);
//        }

//        public ICommentRepository CreateCommentRepo()
//        {
//            return new CommentRepository(_dbContext);
//        }
//    }
//}
