using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IUserInformationRepository : IBaseRepository<UserInformation> 
    {
    }
    public class UserInformationRepository : BaseRepository<UserInformation>, IUserInformationRepository
    {
        public UserInformationRepository(IDbFactory dbFactory) : base(dbFactory)
        { 
        }
    }
}
