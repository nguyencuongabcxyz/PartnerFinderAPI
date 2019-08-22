using System;
using System.Collections.Generic;
using System.Text;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public interface IUserInformationRepository : IBaseRepository<UserInformation> { }
    public class UserInformationRepository : BaseRepository<UserInformation>, IUserInformationRepository
    {
        public UserInformationRepository(DbContext dbContext) : base(dbContext)
        { 
        }
    }
}
