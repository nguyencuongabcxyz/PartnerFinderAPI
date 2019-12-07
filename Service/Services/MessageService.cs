using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Services
{
    public interface IMessageService
    {
        // Task<IEnumerable<Messag>>
    }
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepo;
        public MessageService(IMessageRepository messageRepo)
        {
            _messageRepo = messageRepo;
        }
    }
}
