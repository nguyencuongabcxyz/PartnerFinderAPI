using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class CommentReaction
    {
        public int Id { get; set; }
        public CommentReactionType Type { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
    }

    public enum CommentReactionType
    {
        Like
    }

}
