﻿using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public interface IPostRepository : IBaseRepository<Post>
    {
    }
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(IDbFactory dbFactory) : base(dbFactory)
        {
            
        }
    }
}
