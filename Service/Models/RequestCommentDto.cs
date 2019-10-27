using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class RequestCommentDto
    {
        public int? PostId { get; set; }
        public string UserId { get; set; }
        public int? ParentId { get; set; }
        public string Content { get; set; }
    }
}
