using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class PartnerFinderDto
    {
        public string UserId { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public UserLevel? Level { get; set; }
        public string Introduction { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
