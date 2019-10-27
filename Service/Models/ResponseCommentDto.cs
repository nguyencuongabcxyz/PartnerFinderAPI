using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace Service.Models
{
    public class ResponseCommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? PostId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public int? ParentId { get; set; }
        public List<ResponseCommentDto> SubComments { get; set; }
    }
}
