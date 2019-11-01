using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Models
{
    public class PostReaction
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public PostReactionType Type { get; set; }
        public ApplicationUser User { get; set; }
        public int? PostId { get; set; }
        public Post Post { get; set; }
        public string Content { get; set; }
    }

    public enum PostReactionType
    {
        UpVote,
        DownVote
    }

}
