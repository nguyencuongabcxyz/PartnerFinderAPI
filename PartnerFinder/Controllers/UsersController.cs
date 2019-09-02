using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartnerFinder.CustomFilters;
using Service;
using Service.Models;

namespace PartnerFinder.Controllers
{
    [ServiceFilter(typeof(ObjectExistenceFilter))]
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
            using (_serviceFactory.CreateUnitOfWork())
            {
                var userInformationService = _serviceFactory.CreateUserInformationService();
                try
                {
                    await userInformationService.CheckInitializedInfo(id);
                }
                catch (ObjectNotFoundException e)
                {
                    await userInformationService.AddWithEmptyInfo(id, "");
                }
                var completedInfoPercentage = await userInformationService.GetPercentageOfCompletedInfo(id);
                var isHavingLevel = await userInformationService.CheckIfUserHaveSpecification(m => m.UserId == id && m.Level != null);
                return Ok(new { completedInfoPercentage, isHavingLevel });
            }
        }

        [HttpPatch("{id}/updateLevel")]
        public async Task<IActionResult> UpdateLevel(string id, List<QuestionResultDto> questionResult)
        {
            var userInformationService = _serviceFactory.CreateUserInformationService();

            var testResult = await _serviceFactory.CreateLevelTestService().GetResultAfterTest(questionResult);
            using (_serviceFactory.CreateUnitOfWork())
            {
                await userInformationService.UpdateLevel(id, testResult.Level);
            }
             
            return Ok(testResult);
        }

    }
}