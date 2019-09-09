using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public interface IFindingPartnerUserRepository : IBaseRepository<FindingPartnerUser>
    {
    }
    public class FindingPartnerUserRepository : BaseRepository<FindingPartnerUser>, IFindingPartnerUserRepository
    {
        public FindingPartnerUserRepository(IDbFactory dbFactory) : base(dbFactory)
        {
            
        }
    }
}
