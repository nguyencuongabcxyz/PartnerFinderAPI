using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
    public class BlockedRelation
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        [ForeignKey("Owner")]
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }
        [ForeignKey("BlockedUser")]
        public string BlockedUserId { get; set; }
        public ApplicationUser BlockedUser { get; set; }

    }
}
