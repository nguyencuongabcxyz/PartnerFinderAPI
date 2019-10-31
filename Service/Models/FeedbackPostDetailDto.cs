using System;
using Data.Models;

namespace Service.Models
{
    public class FeedbackPostDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public PostType Type { get; set; }
        public string Video { get; set; }
        public string Script { get; set; }
        public int AnswerNumber { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
    }
}
