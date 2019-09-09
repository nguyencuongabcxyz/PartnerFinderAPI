using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public interface ILevelTestRepository : IBaseRepository<LevelTest>
    {
        Task<LevelTest> GetOneWithQuestionsAndAnswerOptions(int id);
        Task<IEnumerable<LevelTest>> GetAllWithQuestionsAndOptions();
    }

    public class LevelTestRepository : BaseRepository<LevelTest>, ILevelTestRepository
    {
        public LevelTestRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public async Task<IEnumerable<LevelTest>> GetAllWithQuestionsAndOptions()
        {
            var levelTests = await EntitiesSet.Where(t => t.IsDeleted != true)
                .Include(t => t.Questions)
                    .ThenInclude(q => q.AnswerOptions).ToListAsync();
            return levelTests;
        }

        public async Task<LevelTest> GetOneWithQuestionsAndAnswerOptions(int id)
        {
            var levelTest = EntitiesSet.Where(t => t.Id == id && t.IsDeleted != true)
                .Include(t => t.Questions)
                    .ThenInclude(q => q.AnswerOptions).FirstOrDefaultAsync();
            return await levelTest;
        }

    }
}
