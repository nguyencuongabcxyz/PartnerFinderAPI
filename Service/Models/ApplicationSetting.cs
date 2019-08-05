using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class ApplicationSetting
    {
        public string Jwt_Secret { get; set; }
        public string Client_Url { get; set; }
    }
}
