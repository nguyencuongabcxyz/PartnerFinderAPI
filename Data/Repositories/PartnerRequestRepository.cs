using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public interface IPartnerRequestRepository : IBaseRepository<PartnerRequest>
    {
    }
    public class PartnerRequestRepository : BaseRepository<PartnerRequest>, IPartnerRequestRepository
    {
        public PartnerRequestRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
