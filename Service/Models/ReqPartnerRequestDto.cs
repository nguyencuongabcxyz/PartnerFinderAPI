using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Data.Models;

namespace Service.Models
{
    public class ReqPartnerRequestDto
    {
        public string Content { get; set; }
        [Required]
        public string SenderId { get; set; }
        [Required]
        public string ReceiverId { get; set; }

    }
}
