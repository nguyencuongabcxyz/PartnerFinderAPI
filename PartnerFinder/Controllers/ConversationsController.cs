using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class ConversationsController : CommonBaseController
    {
        private readonly IConversationService _conversationService;
        private readonly IUnitOfWork _unitOfWork;
        public ConversationsController(IConversationService conversationService, IUnitOfWork unitOfWork)
        {
            _conversationService = conversationService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();
            var conversations = await _conversationService.GetAllWithLastedMessage(userId);
            return Ok(conversations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var conversation = await _conversationService.GetOneWithAllMessage(id);
            await _unitOfWork.Commit();
            return Ok(conversation);
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            var userId = GetUserId();
            var count = await _conversationService.Count(userId);
            return Ok(count);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOne(string creatorId)
        {
            var userId = GetUserId();
            await _conversationService.CreateOne(userId, creatorId);
            await _unitOfWork.Commit();
            return Ok();
        }
    }
}