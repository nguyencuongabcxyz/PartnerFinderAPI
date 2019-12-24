using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class ResReport
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderAvatar { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverAvatar { get; set; }
        public int? PostId { get; set; }
        public PostType PostType { get; set; }
        public ReportType Type { get; set; }
    }
}
