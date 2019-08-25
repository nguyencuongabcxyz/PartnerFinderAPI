using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Data.Repositories;

namespace Service.Services
{
    public interface ILevelTestService
    {
        Task<LevelTest> GetOneWithQuestionsAndAnswerOptions(int id);
        Task<IEnumerable<LevelTest>> GetAllWithQuestionsAndAnswerOptions();
        Task<bool> CheckExistence(int id);
        Task<int> CountAll();
        Task<int> Count();
    }
    public class LevelTestService : ILevelTestService
    {
        private readonly ILevelTestRepository _levelTestRepo;

        public LevelTestService(IRepositoryFactory repositoryFactory)
        {
            _levelTestRepo = repositoryFactory.CreateLevelTestRepo();
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
    }
}
