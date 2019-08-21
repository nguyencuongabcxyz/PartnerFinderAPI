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
        public string Name { get; set; }
        public string Avatar { get; set; }
        public int Age { get; set; }
        public UserLevel? Level { get; set; }
        [Required]
        public string Location { get; set; }
        public string Hobbies { get; set; }
        public string Introduction { get; set; }
        public string VoiceAudio { get; set; }
        public string Video { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ApplicationUser User { get; set; }
    }

    public enum UserLevel
    {
        Beginner,
        Intermediate,
        Advanced
    }
}
