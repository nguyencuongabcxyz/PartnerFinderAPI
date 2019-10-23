using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Data.Models;

namespace Service.Models
{
    public class QuestionPostDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int AnswerNumber { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
    }
}
