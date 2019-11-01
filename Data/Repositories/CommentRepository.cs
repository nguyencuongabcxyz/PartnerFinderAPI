using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetManyWithSubComment(Expression<Func<Comment, bool>> condition);
    }
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(IDbFactory dbFactory) : base(dbFactory)
        {
            
        }

        public override async Task<int> Count(Expression<Func<Comment, bool>> condition = null)
        {
            var comments = await GetManyWithSubComment(condition);
            var enumerable = comments.ToList();
            var subCount = enumerable.Sum(item => item.SubComments.Count);
            return subCount + enumerable.Count();
        }

        public async Task<IEnumerable<Comment>> GetManyWithSubComment(Expression<Func<Comment, bool>> condition)
        {
            var comments = await EntitiesSet.Where(condition)
                .Include(c => c.SubComments)
                .ToListAsync();
            return comments;
        }
    }
}
