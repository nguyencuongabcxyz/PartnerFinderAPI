using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsViewed { get; set; }
        public bool IsSent { get; set; }
        [Required]
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }
        [Required]
        public string ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; }

    }
}
