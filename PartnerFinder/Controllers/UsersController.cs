using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Models;

namespace PartnerFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;

        public UsersController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
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

            var isHavingInfo = await userInformationService.CheckIfUserHaveSpecification(m => m.UserId == id);
            var isHavingLevel = await userInformationService.CheckIfUserHaveSpecification(m => m.UserId == id && m.Level != null);


            return Ok(new {isHavingInfo, isHavingLevel });
        }

        [HttpPatch("{id}/updateLevel")]
        public async Task<IActionResult> UpdateLevel(string id, List<QuestionResultDto> questionResult)
        {
            var userInformationService = _serviceFactory.CreateUserInformationService();
            var isUserExisting = await userInformationService.CheckExistence(id);
            if (!isUserExisting)
            {
                return NotFound();
            }
            using (var unitOfWork = _serviceFactory.CreateUnitOfWork())
            {
                var userLevel = await _serviceFactory.CreateLevelTestService().GetLevelAfterTest(questionResult);
                await userInformationService.UpdateLevel(id, userLevel);
            }
            return Ok();
        }

    }
}