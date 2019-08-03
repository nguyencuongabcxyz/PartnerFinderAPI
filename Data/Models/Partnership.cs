using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
    public class Partnership
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        [Required]
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }
        [Required]
        public string PartnerId { get; set; }
        public ApplicationUser Partner { get; set; }
    }
}
