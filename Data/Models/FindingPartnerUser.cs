using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
    public class FindingPartnerUser
    {
        [Key, ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }
        public DateTime PostedDate { get; set; }
        [Required]
        public string Description { get; set; }
        public bool? IsDeleted { get; set; }
        public ApplicationUser User { get; set; }
    }
}
