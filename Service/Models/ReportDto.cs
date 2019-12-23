using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class ReportDto
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public string ReceiverId { get; set; }
    }
}
