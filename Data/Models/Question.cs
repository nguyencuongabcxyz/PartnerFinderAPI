using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Question
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public QuestionType Type { get; set; }
        public string Audio { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int LevelTestId { get; set; }
        public LevelTest LevelTest { get; set; }
        public ICollection<AnswerOption> AnswerOptions { get; set; }
    }

    public enum QuestionType
    {
        NonAudio,
        Audio
    }
}
