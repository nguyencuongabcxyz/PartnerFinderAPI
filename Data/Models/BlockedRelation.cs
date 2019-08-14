using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class BlockedRelation
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }
        [Required]
        public string BlockedUserId { get; set; }
        public ApplicationUser BlockedUser { get; set; }

    }
}
