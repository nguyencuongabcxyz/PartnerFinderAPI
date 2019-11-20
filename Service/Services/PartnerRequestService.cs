using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Data.Models;
using Service.Models;
using Service.Extensions;

namespace Service.Services
{
    public interface IPartnerRequestService
    {
        Task AddOne(ReqPartnerRequestDto partnerRequestDto);
        Task<IEnumerable<ResPartnerRequestDto>> GetAll(string userId, int index, int size);
    }
    public class PartnerRequestService : IPartnerRequestService
    {
        private readonly IPartnerRequestRepository _partnerRequestRepo;
        private readonly IUserInformationRepository _userInformationRepo;
        private readonly IMapper _mapper;
        public PartnerRequestService(IPartnerRequestRepository partnerRequestRepo, IMapper mapper, IUserInformationRepository userInformationRepo)
        {
            _partnerRequestRepo = partnerRequestRepo;
            _mapper = mapper;
            _userInformationRepo = userInformationRepo;
        }


        public async Task AddOne(ReqPartnerRequestDto partnerRequestDto)
        {
            var partnerRequest = _mapper.Map<PartnerRequest>(partnerRequestDto);
            partnerRequest.CreatedDate = DateTime.UtcNow;
            await _partnerRequestRepo.Add(partnerRequest);
        }

        private async Task<IEnumerable<ResPartnerRequestDto>> MapModelsToResPartnerRequests(IEnumerable<PartnerRequest> partnerRequests)
        {
            var resPartnerRequests = new List<ResPartnerRequestDto>();
            foreach(var partnerRequest in partnerRequests)
            {
                var user = await _userInformationRepo.GetOne(partnerRequest.SenderId);
                var resPartnerRequest = _mapper.Map<ResPartnerRequestDto>(user)
                                               .Map(partnerRequest, _mapper);
                resPartnerRequests.Add(resPartnerRequest);
            }
            return resPartnerRequests;
        }

        public async Task<IEnumerable<ResPartnerRequestDto>> GetAll(string userId, int index, int size)
        {
            var partnerRequests = await _partnerRequestRepo.OrderAndGetRange(index, size,
                                                                      OrderType.OrderByDescending,
                                                                      p => p.CreatedDate,
                                                                      p => p.IsDeleted != true && p.ReceiverId == userId);
            return await MapModelsToResPartnerRequests(partnerRequests);
        }
    }
}
