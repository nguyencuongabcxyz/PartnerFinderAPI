using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartnerFinder.CustomFilters;
using Service;
using Service.Models;
using Service.Services;

namespace PartnerFinder.Controllers
{
    [ServiceFilter(typeof(ObjectExistenceFilter))]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserInformationService _userInformationService;
        private readonly ILevelTestService _levelTestService;
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUserInformationService userInformationService, ILevelTestService levelTestService, IUnitOfWork unitOfWork)
        {
            _userInformationService = userInformationService;
            _levelTestService = levelTestService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            var userInfo = await _userInformationService.GetOne(id);
            return Ok(userInfo);
        }

        [HttpGet("{id}/checkInfo")]
        public async Task<IActionResult> CheckIfUserUpdateInfo(string id)
        {
            try
            {
                await _userInformationService.CheckInitializedInfo(id);
            }
            catch (ObjectNotFoundException e)
            {
                await _userInformationService.AddWithEmptyInfo(id, "");
                await _unitOfWork.Commit();
            }
            var completedInfoPercentage = await _userInformationService.GetPercentageOfCompletedInfo(id);
            var isHavingLevel = await _userInformationService.CheckIfUserHaveSpecification(m => m.UserId == id && m.Level != null);
            return Ok(new { completedInfoPercentage, isHavingLevel });
        }

        [HttpPatch("{id}/updateLevel")]
        public async Task<IActionResult> UpdateLevel(string id, List<QuestionResultDto> questionResult)
        { 
            var testResult = await _levelTestService.GetResultAfterTest(questionResult);
            await _userInformationService.UpdateLevel(id, testResult.Level);
            await _unitOfWork.Commit();
            return Ok(testResult);
        }
    }
}