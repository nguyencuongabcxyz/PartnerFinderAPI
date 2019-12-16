using AutoMapper;
using Data.Models;
using Data.Repositories;
using Service.Extensions;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Services
{
    public interface IPartnershipService
    {
        Task<PartnershipDto> AddOne(string userId, string partnerId);
        Task<PartnershipDto> RemoveOne(string userId, string partnerId);
        Task<IEnumerable<PartnershipDto>> GetAll(string userId);
    }
    public class PartnershipService : IPartnershipService
    {
        private readonly IPartnershipRepository _partnershipRepo;
        private readonly IUserInformationRepository _userInformationRepo;
        private readonly IConversationRepository _conversationRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PartnershipService(IPartnershipRepository partnershipRepo, IMapper mapper, IUserInformationRepository userInformationRepo, IUnitOfWork unitOfWork, IConversationRepository conversationRepo)
        {
            _partnershipRepo = partnershipRepo;
            _userInformationRepo = userInformationRepo;
            _conversationRepo = conversationRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PartnershipDto> AddOne(string userId, string partnerId)
        {
            var partnershipForAccepter = new Partnership()
            {
                CreatedDate = DateTime.UtcNow,
                PartnerId = partnerId,
                OwnerId = userId,
            };
            var partnershipForSender = new Partnership()
            {
                CreatedDate = DateTime.UtcNow,
                PartnerId = userId,
                OwnerId = partnerId,
            };
            await _partnershipRepo.Add(partnershipForAccepter);
            await _partnershipRepo.Add(partnershipForSender);
            await _unitOfWork.Commit();
            return await MapPartnershipToPartnershipDto(partnershipForAccepter);
        }

        private async Task<PartnershipDto> MapPartnershipToPartnershipDto(Partnership partnership)
        {
            var user = await _userInformationRepo.GetOne(partnership.PartnerId);
            var conversation = await _conversationRepo.GetOneByCondition(c => c.OwnerId == partnership.OwnerId && c.CreatorId == partnership.PartnerId);
            var partnershipDto = _mapper.Map<PartnershipDto>(partnership)
                                        .Map(user, _mapper);
            if (conversation != null)
            {
                partnershipDto.ConversationId = conversation.Id;
            }
            return partnershipDto;
        }

        private async Task<IEnumerable<PartnershipDto>> MapPartnershipsToPartnershipDtos(IEnumerable<Partnership> partnerships)
        {
            var partnershipDtos = new List<PartnershipDto>();
            foreach(var item in partnerships)
            {
                var partnershipDto = await MapPartnershipToPartnershipDto(item);
                partnershipDtos.Add(partnershipDto);
            }
            return partnershipDtos;
        }

        public async Task<IEnumerable<PartnershipDto>> GetAll(string userId)
        {
            var partnerships = await _partnershipRepo.GetManyByCondition(p => p.OwnerId == userId);
            return await MapPartnershipsToPartnershipDtos(partnerships);
        }

        public async Task<PartnershipDto> RemoveOne(string userId, string partnerId)
        {
            var partnershipFromUser = await _partnershipRepo.GetOneByCondition(p => p.OwnerId == userId && p.PartnerId == partnerId);
            var partnershipFromPartner = await _partnershipRepo.GetOneByCondition(p => p.OwnerId == partnerId && p.PartnerId == userId);
            _partnershipRepo.Remove(partnershipFromUser);
            _partnershipRepo.Remove(partnershipFromPartner);
            return await MapPartnershipToPartnershipDto(partnershipFromUser);
        }
    }
}
