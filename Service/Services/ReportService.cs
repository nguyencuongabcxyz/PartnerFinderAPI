using Data.Models;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public interface IReportService 
    {
        Task AddOne(Report report);
        Task DeleteOne(int id);
        Task<IEnumerable<Report>> GetAll(int index, int size);
    }
    public class ReportService : IReportService
    {
        public readonly IReportRepository _reportRepo;
        public ReportService(IReportRepository reportRepo)
        {
            _reportRepo = reportRepo;
        }
        public async Task AddOne(Report report)
        {
            report.CreatedDate = DateTime.UtcNow;
            await _reportRepo.Add(report);
        }

        public Task DeleteOne(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Report>> GetAll(int index, int size)
        {
            var reports = _reportRepo.OrderAndGetRange(index, size, OrderType.OrderByDescending, r => r.Content, null);

            return reports;
        }
    }
}
