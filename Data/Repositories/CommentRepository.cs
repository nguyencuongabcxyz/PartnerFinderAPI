using Data.Models;

namespace Data.Repositories
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {

    }
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(IDbFactory dbFactory) : base(dbFactory)
        {
            
        }
    }
}
