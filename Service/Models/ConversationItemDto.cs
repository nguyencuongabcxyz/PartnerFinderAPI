using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class ConversationItemDto
    {
        public int Id { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsViewed { get; set; }
        public string OwnerId { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string CreatorAvatar { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
