using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Data.Repositories;
using Service.Models;

namespace Service.Services
{
    public interface ILevelTestService
    {
        Task<LevelTest> GetOneWithQuestionsAndAnswerOptions(int id);
        Task<IEnumerable<LevelTest>> GetAllWithQuestionsAndAnswerOptions();
        Task<bool> CheckExistence(int id);
        Task<int> CountAll();
        Task<int> Count();
        Task<TestResultDto> GetResultAfterTest(IEnumerable<QuestionResultDto> questionResult);
    }
    public class LevelTestService : ILevelTestService
    {
        private readonly ILevelTestRepository _levelTestRepo;
        private readonly IQuestionService _questionService;

        public LevelTestService(IRepositoryFactory repositoryFactory, IServiceFactory serviceFactory)
        {
            _levelTestRepo = repositoryFactory.CreateLevelTestRepo();
            _questionService = serviceFactory.CreateQuestionService();
        }

        public async Task<bool> CheckExistence(int id)
        {
            var retrievedTest = await _levelTestRepo.GetOne(id);
            return retrievedTest != null;
        }

        public async Task<int> CountAll()
        {
            return await _levelTestRepo.Count();
        }

        public async Task<int> Count()
        {
            var allTests = await _levelTestRepo.GetManyByCondition(t => t.IsDeleted != true);
            return allTests.Count();
        }

        public async Task<LevelTest> GetOneWithQuestionsAndAnswerOptions(int id)
        {
            return await _levelTestRepo.GetOneWithQuestionsAndAnswerOptions(id);
        }

        public async Task<IEnumerable<LevelTest>> GetAllWithQuestionsAndAnswerOptions()
        {
            return await _levelTestRepo.GetAllWithQuestionsAndOptions();
        }

        private async Task<int> CountRightAnswer(IEnumerable<QuestionResultDto> questionResult)
        {
            var count = 0;
            foreach (var i in questionResult)
            {
                var isRightAnswer = await _questionService.CheckAnswer(i);
                if (isRightAnswer) count++;
            }

            return count;
        }

        public async Task<TestResultDto> GetResultAfterTest(IEnumerable<QuestionResultDto> questionResult)
        {
            var rightAnswerCount = await CountRightAnswer(questionResult);
            if (rightAnswerCount < 5)
            {
                return new TestResultDto()
                {
                    RightAnswerNumber = rightAnswerCount,
                    Level = UserLevel.Beginner
                };
            }
            else if (rightAnswerCount >= 5 && rightAnswerCount < 10)
            {
                return new TestResultDto()
                {
                    RightAnswerNumber = rightAnswerCount,
                    Level = UserLevel.Intermediate
                };
            }
            else
            {
                return new TestResultDto()
                {
                    RightAnswerNumber = rightAnswerCount,
                    Level = UserLevel.Advanced
                };
            }
        }
    }
}
