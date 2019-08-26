using System.Threading.Tasks;
using Data;
using Data.Repositories;
using Service.Models;

namespace Service.Services
{
    public interface IQuestionService
    {
        Task<bool> CheckAnswer(QuestionResultDto questionResult);
    }

    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepo;
        private readonly IAnswerOptionRepository _answerOptionRepo;

        public QuestionService(IRepositoryFactory repositoryFactory)
        {
            _questionRepo = repositoryFactory.CreateQuestionRepo();
            _answerOptionRepo = repositoryFactory.CreateAnswerOptionRepo();
        }

        public async Task<bool> CheckAnswer(QuestionResultDto questionResult)
        {
            var rightAnswer = await _answerOptionRepo.GetOneByCondition(a => a.QuestionId == questionResult.QuestionId && a.IsRight);
            return rightAnswer.Id == questionResult.AnswerId;
        }

    }
}
