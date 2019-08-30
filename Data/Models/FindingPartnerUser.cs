using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class FindingPartnerUser
    {
        [Key, ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }
        public DateTime PostedDate { get; set; }
        [Required]
        [MaxLength(300)]
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public ApplicationUser User { get; set; }
    }
}
