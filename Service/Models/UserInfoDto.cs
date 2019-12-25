using Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Service.Models
{
    public class UserInfoDto
    {
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
        public bool IsBlocked { get; set; }
    }
}
