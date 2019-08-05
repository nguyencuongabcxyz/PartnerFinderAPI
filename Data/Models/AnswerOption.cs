using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Models
{
    public class AnswerOption
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public bool IsDeleted { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
