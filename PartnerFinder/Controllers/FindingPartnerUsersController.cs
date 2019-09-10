using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PartnerFinder.CustomFilters;
using Service.Models;
using Service.Services;

namespace PartnerFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ObjectExistenceFilter))]
    public class FindingPartnerUsersController : ControllerBase
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
            var count = await _findingPartnerUserService.Count();
            var findingPartnerUsers = await _findingPartnerUserService.GetForPagination(index, size);
            return Ok(new {partnerFinders = findingPartnerUsers, count});
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetWithFilteringCondition([FromQuery]FilteringUserConditionDto filteringCondition, int index, int size)
        {
            var userInfos = await _userInformationService.GetManyWithCondition(_userInformationService.HandleFilterCondition(filteringCondition));

            var findingPartnerUsers = await _findingPartnerUserService
                .GetForPaginationWithGivenUsers(userInfos, index, size);
            var count = await _findingPartnerUserService.CountWithGivenUsers(userInfos);
            return Ok(new {partnerFinders = findingPartnerUsers, count});
        }
    }
}