using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public interface IReportRepository : IBaseRepository<Report> { }
    public class ReportRepository : BaseRepository<Report>, IReportRepository
    {
        public ReportRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
