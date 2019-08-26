using System;
using System.Collections.Generic;
using System.Text;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public interface IAnswerOptionRepository : IBaseRepository<AnswerOption> { }
    public class AnswerOptionRepository : BaseRepository<AnswerOption> , IAnswerOptionRepository
    {
        public AnswerOptionRepository(DbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
