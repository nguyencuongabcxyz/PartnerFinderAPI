using AutoMapper;
using Data.Models;
using Data.Repositories;
using Service.Models;
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
        Task<int> Count();
        Task<IEnumerable<ResReport>> GetAll(int index, int size);
    }
    public class ReportService : IReportService
    {
        public readonly IReportRepository _reportRepo;
        public readonly IUserInformationRepository _userInformationRepo;
        public readonly IPostRepository _postRepo;
        public readonly IMapper _mapper;
        public ReportService(IReportRepository reportRepo, IUserInformationRepository userInformationRepo, IMapper mapper, IPostRepository postRepo)
        {
            _reportRepo = reportRepo;
            _userInformationRepo = userInformationRepo;
            _mapper = mapper;
            _postRepo = postRepo;
        }
        public async Task AddOne(Report report)
        {
            report.CreatedDate = DateTime.UtcNow;
            await _reportRepo.Add(report);
        }

        public async Task<int> Count()
        {
            return await _reportRepo.Count();
        }

        public async Task DeleteOne(int id)
        {
            var report = await _reportRepo.GetOne(id);
            _reportRepo.Remove(report);
        }

        public async Task<IEnumerable<ResReport>> GetAll(int index, int size)
        {
            var reports = await _reportRepo.OrderAndGetRange(index, size, OrderType.OrderByDescending, r => r.Content, null);
            var reportDtos = new List<ResReport>();
            foreach(var report in reports)
            {
                var reportDto = _mapper.Map<ResReport>(report);
                var sender = await _userInformationRepo.GetOne(report.SenderId);
                var receiver = await _userInformationRepo.GetOne(report.ReceiverId);
                if(report.PostId != null)
                {
                    var post = await _postRepo.GetOne(report.PostId);
                    reportDto.PostType = post.Type; reportDto.PostType = post.Type;
                }
                reportDto.SenderName = sender.Name;
                reportDto.SenderAvatar = sender.Avatar;
                reportDto.ReceiverName = receiver.Name;
                reportDto.ReceiverAvatar = receiver.Avatar;
                reportDtos.Add(reportDto);
            }
            return reportDtos;
        }
    }
}
