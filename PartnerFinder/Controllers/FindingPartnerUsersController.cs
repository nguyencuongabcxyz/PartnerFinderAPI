using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartnerFinder.CustomFilters;
using PartnerFinder.Extensions;
using Service.Models;
using Service.Services;

namespace PartnerFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ObjectExistenceFilter))]
    [Authorize]
    public class FindingPartnerUsersController : CommonBaseController
    {
        private readonly IFindingPartnerUserService _findingPartnerUserService;
        private readonly IUserInformationService _userInformationService;
        public FindingPartnerUsersController(IFindingPartnerUserService findingPartnerUserService, IUserInformationService userInformationService)
        {
            _findingPartnerUserService = findingPartnerUserService;
            _userInformationService = userInformationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetForPagination(int index, int size)
        {
            var userId = GetUserId();
            var count = await _findingPartnerUserService.Count();
            var findingPartnerUsers = await _findingPartnerUserService.GetForPagination(userId, index, size);

            return Ok(new {partnerFinders = findingPartnerUsers, count});
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetWithFilteringCondition([FromQuery]FilteringUserConditionDto filteringCondition, int index, int size)
        {
            var userId = GetUserId();
            Expression<Func<UserInformation, bool>> isAuthorizedUser = (u) => u.UserId != userId;
            var handledCondition = _userInformationService.HandleFilterCondition(filteringCondition);
            var combinedCondition = isAuthorizedUser.AndAlso(handledCondition);
            var userInfos = await _userInformationService.GetManyWithCondition(combinedCondition);
            var userInformations = userInfos.ToList();
            var findingPartnerUsers = await _findingPartnerUserService
                .GetForPaginationWithGivenUsers(userInformations, index, size);
            var count = await _findingPartnerUserService.CountWithGivenUsers(userInformations);

            return Ok(new {partnerFinders = findingPartnerUsers, count});
        }
    }
}