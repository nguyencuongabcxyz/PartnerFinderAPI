﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int Like { get; set; }
        public int? PostId { get; set; }
        public Post Post { get; set; }
        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int? ParentId { get; set; }
        public Comment Parent { get; set; }
        public ICollection<Comment> SubComments { get; set; }
        public ICollection<CommentReaction> CommentReactions { get; set; }
    }
}
