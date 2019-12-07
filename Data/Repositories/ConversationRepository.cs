using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public interface IConversationRepository : IBaseRepository<Conversation>
    {

    }
    public class ConversationRepository : BaseRepository<Conversation>, IConversationRepository
    {
        public ConversationRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
