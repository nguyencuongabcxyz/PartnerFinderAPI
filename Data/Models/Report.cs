using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required]
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }
        [Required]
        public string ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; }
        public int? PostId { get; set; }
        public Post Post { get; set; }
        public ReportType Type { get; set; }
    }

    public enum ReportType
    {
        UserReport,
        PostReport,
    }
}
