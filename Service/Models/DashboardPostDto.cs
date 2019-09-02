using System;
using Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Service.Models
{
    public class DashboardPostDto
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public int AnswerNumber { get; set; }
        [Required]
        public PostType Type { get; set; }
        public DateTime UpdatedDate { get; set; }
        //[Key, ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        public string Avatar { get; set; }

    }
}
