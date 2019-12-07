using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class ResMessageDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsViewed { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderAvatar { get; set; }
    }
}
