using System;
using System.Collections.Generic;
using System.Text;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class UserInformationRepository : BaseRepository<UserInformation>
    {
        public UserInformationRepository(DbContext dbContext) : base(dbContext)
        { 
        }
    }
}
