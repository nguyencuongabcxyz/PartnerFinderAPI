using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class UserInformation
    {
        [Key, ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        public string Avatar { get; set; }
        public int? Age { get; set; }
        public UserLevel? Level { get; set; }
        public string Location { get; set; }
        public string Hobbies { get; set; }
        public string Introduction { get; set; }
        public string VoiceAudio { get; set; }
        public string Video { get; set; }
        public string EnglishSkill { get; set; }
        public string LearningSkill { get; set; }
        public string Expectation { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ApplicationUser User { get; set; }
        public bool IsBlocked { get; set; }
    }

    public enum UserLevel
    {
        Beginner,
        Intermediate,
        Advanced,
        Undefined
    }
}
