using AutoMapper;
using Data.Models;
using Data.Repositories;
using Service.Extensions;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public interface IConversationService
    {
        Task<IEnumerable<ConversationDto>> GetAllWithLastedMessage(string ownerId);
        Task<ConversationItemDto> GetOneWithAllMessage(int id);
        Task<bool> CheckExistence(string senderId, string receiverId);
        Task<List<int>> GetListIdConversation(string senderId, string receiverId);
        Task CreateOne(string ownerId, string creatorId);
        Task<int> Count(string userId);
    }
    public class ConversationService : IConversationService
    {
        private readonly IConversationRepository _conversationRepo;
        private readonly IMessageRepository _messageRepo;
        private readonly IUserInformationRepository _userInformationRepo;
        private readonly IMapper _mapper;
        public ConversationService(IConversationRepository conversationRepo, IMessageRepository messageRepo, IMapper mapper, IUserInformationRepository userInformationRepo)
        {
            _conversationRepo = conversationRepo;
            _messageRepo = messageRepo;
            _mapper = mapper;
            _userInformationRepo = userInformationRepo;
        }

        public async Task CreateOne(string ownerId, string creatorId)
        {
            var conversation = new Conversation()
            {
                UpdatedDate = DateTime.UtcNow,
                OwnerId = ownerId,
                CreatorId = creatorId,
            };
            var conversationForCreator = new Conversation()
            {
                UpdatedDate = DateTime.UtcNow,
                OwnerId = creatorId,
                CreatorId = ownerId,
            };
            await _conversationRepo.Add(conversation);
            await _conversationRepo.Add(conversationForCreator);
        }

        private async Task<ConversationDto> MapConversationToConversationDto(Conversation conversation)
        {
            var user = await _userInformationRepo.GetOne(conversation.CreatorId);
            var lastedMessage = await _messageRepo.GetLastedMessage(conversation.Id);
            var conversationDto = _mapper.Map<ConversationDto>(user)
                                         .Map(conversation, _mapper);
            conversationDto.LastedMessage = lastedMessage.Content;
            return conversationDto;
        }

        private async Task<IEnumerable<ConversationDto>> MapConversationsToConversationDtos(IEnumerable<Conversation> conversations)
        {
            var conversationDtos = new List<ConversationDto>();
            foreach(var item in conversations)
            {
                var conversationDto = await MapConversationToConversationDto(item);
                conversationDtos.Add(conversationDto);
            }
            return conversationDtos;
        }

        public async Task<IEnumerable<ConversationDto>> GetAllWithLastedMessage(string ownerId)
        {
            var conversations = await _conversationRepo.GetManyByCondition(c => c.OwnerId == ownerId);
            return await MapConversationsToConversationDtos(conversations);
        }

        public async Task<ConversationItemDto> GetOneWithAllMessage(int id)
        {
            var conversation = await _conversationRepo.GetOne(id);
            conversation.IsViewed = true;
            var messages = await _messageRepo.GetManyByCondition(m => m.ConversationId == conversation.Id);
            conversation.Messages = messages.OrderByDescending(m => m.CreatedDate).ToList();
            var user = await _userInformationRepo.GetOne(conversation.CreatorId);
            var conversationDto = _mapper.Map<ConversationItemDto>(user)
                                         .Map(conversation, _mapper);
            return conversationDto;
        }

        public async Task<bool> CheckExistence(string senderId, string receiverId)
        {
            var conversation = await _conversationRepo.GetOneByCondition(c => c.OwnerId == senderId && c.CreatorId == receiverId);
            return conversation != null;
        }

        public async Task<List<int>> GetListIdConversation(string senderId, string receiverId)
        {
            List<int> conversationIds = new List<int>();
            var conversationForSender = await _conversationRepo.GetOneByCondition(c => c.OwnerId == senderId && c.CreatorId == receiverId);
            var conversationForReceiver = await _conversationRepo.GetOneByCondition(c => c.OwnerId == receiverId && c.CreatorId == senderId);
            conversationIds.Add(conversationForSender.Id);
            conversationIds.Add(conversationForReceiver.Id);
            return conversationIds;
        }

        public async Task<int> Count(string userId)
        {
            return await _conversationRepo.Count(c => c.OwnerId == userId && c.IsViewed == false);
        }
    }
}
