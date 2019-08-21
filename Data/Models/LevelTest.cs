using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class LevelTest
    {
        public int Id { get; set; }
        [Required]
        public int QuestionNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        public string AdminId { get; set; }
        public ApplicationUser Admin { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
