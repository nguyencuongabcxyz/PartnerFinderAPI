using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class AdminUserDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public bool IsBlocked { get; set; }
    }
}
