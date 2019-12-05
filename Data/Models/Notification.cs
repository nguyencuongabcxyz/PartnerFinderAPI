using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Notification
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public NotificationType Type { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsViewed { get; set; }
        public int? PostId { get; set; }
        public Post Post { get; set; }
        [Required]
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }
        [Required]
        public string CreatorId { get; set; }
        public ApplicationUser Creator { get; set; }
    }

    public enum NotificationType
    {
        PostComment,
        CommentReply,
        PostLike,
        CommentLike,
    }
}
