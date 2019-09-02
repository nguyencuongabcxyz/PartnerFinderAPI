using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {

    }
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(DbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
