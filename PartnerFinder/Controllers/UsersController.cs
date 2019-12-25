using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
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
    public class UsersController : CommonBaseController
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

        [HttpGet("check-info")]
        public async Task<IActionResult> CheckIfUserUpdateInfo()
        {
            var userId = GetUserId();
            try
            {
                await _userInformationService.CheckInitializedInfo(userId);
            }
            catch (ObjectNotFoundException e)
            {
                await _userInformationService.AddWithEmptyInfo(userId, "");
                await _unitOfWork.Commit();
            }
            var completedInfoPercentage = await _userInformationService.GetPercentageOfCompletedInfo(userId);
            var isHavingLevel = await _userInformationService.CheckIfUserHaveSpecification(m => m.UserId == userId && m.Level != null);
            return Ok(new { completedInfoPercentage, isHavingLevel });
        }

        [HttpPatch("update-level")]
        public async Task<IActionResult> UpdateLevel(List<QuestionResultDto> questionResult)
        {
            var userId = GetUserId();
            var testResult = await _levelTestService.GetResultAfterTest(questionResult);
            await _userInformationService.UpdateLevel(userId, testResult.Level);
            await _unitOfWork.Commit();
            return Ok(testResult);
        }

        [HttpPut]
        public async Task<IActionResult> PostUserInfo(UserInfoDto userInfoDto)
        {
            var userId = GetUserId();
            var updatedUserInfoDto = await _userInformationService.Update(userId, userInfoDto);
            await _unitOfWork.Commit();
            return Ok(updatedUserInfoDto);
        }

        [HttpPatch("update-media")]
        public async Task<IActionResult> UpdateMediaProfile(MediaProfileDto mediaProfile)
        {
            var userId = GetUserId();
            var updatedUserInfoDto = await _userInformationService.UpdateMediaProfile(userId, mediaProfile);
            await _unitOfWork.Commit();
            return Ok(updatedUserInfoDto);
        }

        //Admin API
        [HttpGet("admin")]
        public async Task<IActionResult> GetAdminUsers(int index, int size)
        {
            var userId = GetUserId();
            var users = await _userInformationService.GetAdminUsers(userId, index, size);
            var count = await _userInformationService.CountAdminUsers(userId);
            return Ok(new { users, count });
        }

        //Admin API
        [HttpGet("admin/search")]
        public async Task<IActionResult> SearchAdminUsers(string pattern)
        {
            var userId = GetUserId();
            var users = await _userInformationService.SearchAdminUsers(userId, pattern);
            return Ok(users);
        }

        //Admin API
        [HttpPut("block")]
        public async Task<IActionResult> BlockUser(string userId)
        {
            var user = await _userInformationService.BlockUser(userId);
            return Ok(user);
        }

        //Admin API
        [HttpPut("active")]
        public async Task<IActionResult> ActiveUser(string userId)
        {
            var user = await _userInformationService.ActiveUser(userId);
            return Ok(user);
        }
    }
}