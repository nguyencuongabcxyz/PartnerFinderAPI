using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Data.Models;

namespace Service.Models
{
    public class ReqPartnerRequestDto
    {
        [Required]
        [MaxLength(500)]
        public string Content { get; set; }
        public string SenderId { get; set; }
        [Required]
        public string ReceiverId { get; set; }

    }
}
