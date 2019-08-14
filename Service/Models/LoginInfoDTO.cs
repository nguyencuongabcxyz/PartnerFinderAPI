using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Service.Models
{
    public class LoginInfoDTO
    {
        [Required]
        [MinLength(4)]
        public string UserName { get; set; }
        [Required]
        [MinLength(4)]
        public string Password { get; set; }
    }
}
