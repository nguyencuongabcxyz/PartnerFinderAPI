using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
        Task<Message> GetLastedMessage(int conversationId);
    }
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<Message> GetLastedMessage(int conversationId)
        {
            var message = await  EntitiesSet.Where(m => m.ConversationId == conversationId).OrderByDescending(s => s.CreatedDate).FirstOrDefaultAsync();
            return message;
        }
    }
}
