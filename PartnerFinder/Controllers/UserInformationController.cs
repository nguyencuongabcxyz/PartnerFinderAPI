using System;
using System.Linq.Expressions;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace PartnerFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInformationController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;
        //private readonly ICheckingObjectExistenceService _checkingObjectExistence;

        public UserInformationController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }
        [HttpGet("{id}/checkLevel")]
        public IActionResult CheckIfUserHaveLevel(string id)
        {
            var userInformationService = _serviceFactory.CreateUserInformationService();
            var isUserExisting = userInformationService.CheckExistence(id);
            if (!isUserExisting)
            {
                return NotFound();
            }

            Expression<Func<UserInformation, bool>> specification = (m) => m.UserId == id && m.Level != null;
            var result = userInformationService.CheckIfUserHaveSpecification(specification);

            return Ok(result);
        }
    }
}