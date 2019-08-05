using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Service.Models
{
    public class ErrorDetailDTO
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
