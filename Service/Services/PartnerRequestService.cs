﻿using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Data.Models;
using Service.Models;
using Service.Extensions;
using System.Linq.Expressions;
using System.Linq;

namespace Service.Services
{
    public interface IPartnerRequestService
    {
        Task AddOne(ReqPartnerRequestDto partnerRequestDto);
        Task RemoveOne(int id);
        Task AcceptOne(int id);
        Task<IEnumerable<ResPartnerRequestDto>> GetAll(string userId);
        Task<int> Count(string userId);
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

        public async Task<IEnumerable<ResPartnerRequestDto>> GetAll(string userId)
        {
            var partnerRequests = await _partnerRequestRepo.GetManyByCondition(p => p.IsDeleted != true && p.ReceiverId == userId);
            var partnerRequestList = partnerRequests.ToList().OrderByDescending(p => p.CreatedDate);
            return await MapModelsToResPartnerRequests(partnerRequestList);
        }

        public async Task<int> Count(string userId)
        {
            return await _partnerRequestRepo.Count(p => p.IsDeleted != true && p.ReceiverId == userId);
        }

        public async Task RemoveOne(int id)
        {
            var partnerRequest = await _partnerRequestRepo.GetOne(id);
            _partnerRequestRepo.Remove(partnerRequest);
        }

        public async Task AcceptOne(int id)
        {
            var requestFromRequestingUser = await _partnerRequestRepo.GetOne(id);
            var requestFromSenderUser = await _partnerRequestRepo.GetOneByCondition(p =>
            p.SenderId == requestFromRequestingUser.ReceiverId
            && p.ReceiverId == requestFromRequestingUser.SenderId);
            _partnerRequestRepo.Remove(requestFromRequestingUser);
            if(requestFromSenderUser != null)
            _partnerRequestRepo.Remove(requestFromSenderUser);
        }
    }
}
