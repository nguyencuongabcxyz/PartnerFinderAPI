using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace PartnerFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;

        public UserController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }
        [HttpGet("{id}/checkLevel")]
        public async Task<IActionResult> CheckIfUserHaveLevel(string id)
        {
            var userInformationService = _serviceFactory.CreateUserInformationService();
            var isUserExisting =  await userInformationService.CheckExistence(id);
            if (!isUserExisting)
            {
                return NotFound();
            }

            Expression<Func<UserInformation, bool>> specification = (m) => m.UserId == id && m.Level != null;
            var result = await userInformationService.CheckIfUserHaveSpecification(specification);

            return result ? Ok(new {level = "set"}) : Ok(new {level = "unset"});
        }

        [HttpGet("{id}/checkInfo")]
        public async Task<IActionResult> CheckIfUserUpdateInfo(string id)
        {
            var userInformationService = _serviceFactory.CreateUserInformationService();
            var isUserExisting = await userInformationService.CheckExistence(id);
            if (!isUserExisting)
            {
                return NotFound();
            }
            Expression<Func<UserInformation, bool>> specification = (m) => m.UserId == id;
            var result = await userInformationService.CheckIfUserHaveSpecification(specification);

            return result ? Ok(new { info = "set" }) : Ok(new { info = "unset" });
        }

    }
}