using Data.Models;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public interface IPartnershipService
    {
        Task AddOne(string userId, string partnerId);
    }
    public class PartnershipService : IPartnershipService
    {
        private readonly IPartnershipRepository _partnershipRepo;
        public PartnershipService(IPartnershipRepository partnershipRepo)
        {
            _partnershipRepo = partnershipRepo;
        }
        public async Task AddOne(string userId, string partnerId)
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
        }
    }
}
