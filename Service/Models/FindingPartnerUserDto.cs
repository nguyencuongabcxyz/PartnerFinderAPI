using System;
using System.ComponentModel.DataAnnotations;
using Data.Models;

namespace Service.Models
{
    public class FindingPartnerUserDto
    {
        public string UserId { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        public string Avatar { get; set; }
        public int? Age { get; set; }
        public UserLevel? Level { get; set; }
        public string Location { get; set; }
        [Required]
        [MaxLength(300)]
        public string Description { get; set; }
        public DateTime PostedDate { get; set; }
    }
}
