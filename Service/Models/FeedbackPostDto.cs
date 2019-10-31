using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Data.Models;

namespace Service.Models
{
    public class FeedbackPostDto
    {

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        [Required]
        [MaxLength(2000)]
        public string Content { get; set; }
        public string Video { get; set; }
        [MaxLength(1500)]
        public string Script { get; set; }
        [Required]
        public PostType Type { get; set; }
    }
}
