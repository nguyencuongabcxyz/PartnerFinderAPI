using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsViewed { get; set; }
        public int? PostId { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string CreatorAvatar { get; set; }
    }
}
