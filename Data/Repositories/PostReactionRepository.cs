using System;
using System.Collections.Generic;
using System.Text;
using Data.Models;

namespace Data.Repositories
{
    public interface IPostReactionRepository : IBaseRepository<PostReaction>
    {

    }
    public class PostReactionRepository : BaseRepository<PostReaction>, IPostReactionRepository
    {
        public PostReactionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

    }
}
