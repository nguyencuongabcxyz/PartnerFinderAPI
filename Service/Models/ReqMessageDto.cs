using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class ReqMessageDto
    {
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
    }
}
