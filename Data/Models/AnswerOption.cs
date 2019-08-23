using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class AnswerOption
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public bool IsRight { get; set; }
        public bool IsDeleted { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
