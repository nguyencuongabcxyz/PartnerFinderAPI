using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public interface IPartnershipRepository : IBaseRepository<Partnership>
    {

    }
    public class PartnershipRepository : BaseRepository<Partnership>, IPartnershipRepository
    {
        public PartnershipRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
