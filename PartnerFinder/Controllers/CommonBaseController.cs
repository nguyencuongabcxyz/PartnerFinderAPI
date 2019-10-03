using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PartnerFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonBaseController : ControllerBase
    {
        protected string GetUserId()
        {
            return User.Claims.First(c => c.Type == "userId").Value;
        }
    }
}