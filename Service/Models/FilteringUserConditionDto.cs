using System;
using System.Collections.Generic;
using System.Text;
using Data.Models;

namespace Service.Models
{
    public class FilteringUserConditionDto
    {
        public string Location { get; set; }
        public UserLevel Level { get; set; }
    }
}
