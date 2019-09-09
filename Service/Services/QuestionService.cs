using System.Threading.Tasks;
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
        private readonly IAnswerOptionRepository _answerOptionRepo;

        public QuestionService(IAnswerOptionRepository answerOptionRepo)
        {
            _answerOptionRepo = answerOptionRepo;
        }

        public async Task<bool> CheckAnswer(QuestionResultDto questionResult)
        {
            var rightAnswer = await _answerOptionRepo.GetOneByCondition(a => a.QuestionId == questionResult.QuestionId && a.IsRight);
            return rightAnswer.Id == questionResult.AnswerId;
        }

    }
}
