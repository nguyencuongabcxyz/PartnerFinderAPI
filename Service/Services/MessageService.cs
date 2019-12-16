using Data.Models;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public interface IMessageService
    {
        Task AddOne(string senderId, string receiverId, string content);
    }
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepo;
        private readonly IConversationRepository _conversationRepo;
        private readonly IConversationService _conversationService;
        public MessageService(IMessageRepository messageRepo, IConversationRepository conversationRepo, IConversationService conversationService)
        {
            _messageRepo = messageRepo;
            _conversationRepo = conversationRepo;
            _conversationService = conversationService;
        }

        public async Task AddOne(string senderId, string receiverId,string content)
        {
            var listIds = await _conversationService.GetListIdConversation(senderId, receiverId);
            var conversation1 = await _conversationRepo.GetOne(listIds[0]);
            var conversation2 = await _conversationRepo.GetOne(listIds[1]);
            conversation1.UpdatedDate = DateTime.UtcNow;
            conversation2.UpdatedDate = DateTime.UtcNow;
            var messageForSender = new Message()
            {
                Content = content,
                CreatedDate = DateTime.UtcNow,
                SenderId = senderId,
                ConversationId = listIds[0],
            };
            var messageForReceiver = new Message()
            {
                Content = content,
                CreatedDate = DateTime.UtcNow,
                SenderId = senderId,
                ConversationId = listIds[1],
            };
            await _messageRepo.Add(messageForSender);
            await _messageRepo.Add(messageForReceiver);
        }
    }
}
