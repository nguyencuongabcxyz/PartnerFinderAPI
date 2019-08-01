using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public UserInformation UserInformation { get; set; }
    }
}
