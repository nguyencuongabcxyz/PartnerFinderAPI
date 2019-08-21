using System;
using System.Collections.Generic;
using System.Text;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{

    public class UserRepository : BaseRepository<ApplicationUser>
    {
        public UserRepository(DbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
