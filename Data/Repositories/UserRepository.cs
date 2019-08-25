﻿using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public interface IUserRepository : IBaseRepository<ApplicationUser> { }
    public class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(DbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
