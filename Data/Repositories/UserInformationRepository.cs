using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public interface IUserInformationRepository : IBaseRepository<UserInformation> { }
    public class UserInformationRepository : BaseRepository<UserInformation>, IUserInformationRepository
    {
        public UserInformationRepository(IDbFactory dbFactory) : base(dbFactory)
        { 
        }
    }
}
