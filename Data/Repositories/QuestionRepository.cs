using System;
using System.Collections.Generic;
using System.Text;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public interface  IQuestionRepository : IBaseRepository<Question> { }
    public class QuestionRepository : BaseRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(IDbFactory dbFactory) : base(dbFactory)
        { 
        }
    }
}
