using System;

namespace Service.Models
{
    public class ResPartnerRequestDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsViewed { get; set; }
        public string SenderId { get; set; }
        public string SenderAvatar { get; set; }
        public string SenderName { get; set; }
    }
}
