using System.ComponentModel.DataAnnotations;

namespace Service.Models
{
    public class QuestionPostDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        [Required]
        [MaxLength(2000)]
        public string Content { get; set; }
    }
}
