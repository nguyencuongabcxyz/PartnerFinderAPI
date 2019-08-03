using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
    public class PartnerRequest
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsAccepted { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsViewed { get; set; }
        [Required]
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }
        [Required]
        public string ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; }


    }
}
