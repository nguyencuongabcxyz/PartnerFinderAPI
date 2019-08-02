using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        public bool? IsDeleted { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
