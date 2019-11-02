using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Data.Models;

namespace Data.Repositories
{
    public interface ICommentReactionRepository : IBaseRepository<CommentReaction>
    {

    }
    public class CommentReactionRepository : BaseRepository<CommentReaction>, ICommentReactionRepository
    {
        public CommentReactionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
