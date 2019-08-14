using System.ComponentModel.DataAnnotations;

namespace Service.Models
{
    public class LoginInfoDto
    {
        [Required]
        [MinLength(4)]
        public string UserName { get; set; }
        [Required]
        [MinLength(4)]
        public string Password { get; set; }
    }
}
