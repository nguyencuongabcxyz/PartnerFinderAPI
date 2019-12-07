using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class ConversationDto
    {
        public int Id { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsViewed { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string CreatorAvatar { get; set; }
        public string LastedMessage { get; set; }
    }
}
