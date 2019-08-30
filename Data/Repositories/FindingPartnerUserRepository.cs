using System;
using System.Collections.Generic;
using System.Text;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public interface IFindingPartnerUserRepository : IBaseRepository<FindingPartnerUser>
    {

    }
    public class FindingPartnerUserRepository : BaseRepository<FindingPartnerUser>, IFindingPartnerUserRepository
    {
        public FindingPartnerUserRepository(DbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
