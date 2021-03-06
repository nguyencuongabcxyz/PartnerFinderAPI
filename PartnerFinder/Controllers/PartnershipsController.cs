﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartnerFinder.CustomFilters;
using Service;
using Service.Services;

namespace PartnerFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ObjectExistenceFilter))]
    [Authorize]
    public class PartnershipsController : CommonBaseController
    {
        private readonly IPartnershipService _partnershipService;
        private readonly IConversationService _conversationService;
        private readonly IUnitOfWork _unitOfWork;
        public PartnershipsController(IPartnershipService partnershipService, IUnitOfWork unitOfWork, IConversationService conversationService)
        {
            _partnershipService = partnershipService;
            _conversationService = conversationService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> AddPartnership(string partnerId)
        {
            var userId = GetUserId();
            var partnership = await _partnershipService.AddOne(userId, partnerId);
            await _conversationService.CreateOne(userId, partnerId);
            await _unitOfWork.Commit();
            var conversation = await _conversationService.GetOneByCondition(userId, partnerId);
            partnership.ConversationId = conversation.Id;
            return Ok(partnership);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPartners()
        {
            var userId = GetUserId();
            var partnerships = await _partnershipService.GetAll(userId);
            return Ok(partnerships);
        }

        [HttpDelete("{partnerId}")]
        public async Task<IActionResult> RemovePartnership(string partnerId)
        {
            var userId = GetUserId();            
            var partnership = await _partnershipService.RemoveOne(userId, partnerId);
            await _unitOfWork.Commit();
            return Ok(partnership);
        }
    }
}