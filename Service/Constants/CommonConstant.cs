using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Constants
{
    public static class CommonConstant
    {
        public static readonly int SearchLimit = 3;
        public static readonly Dictionary<NotificationType, string> NotificationContentDic = new Dictionary<NotificationType, string>()
        {
            {NotificationType.PostComment, NotificationContent.PostComment},
            {NotificationType.CommentReply, NotificationContent.CommentReply},
            {NotificationType.PostLike, NotificationContent.PostLike},
            {NotificationType.CommentLike, NotificationContent.CommentLike},
        };
    }

    public static class NotificationContent
    {
        public static readonly string PostComment = "has commented on your post.";
        public static readonly string CommentReply = "has replyed your comment.";
        public static readonly string PostLike = "has liked your post.";
        public static readonly string CommentLike = "has liked your comment.";
    }

}
