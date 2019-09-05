using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartnerFinder.CustomFilters;
using Service;

namespace PartnerFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ObjectExistenceFilter))]
    public class FindingPartnerUsersController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;
        public FindingPartnerUsersController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        [HttpGet]
        public async Task<IActionResult> GetForPagination(int index, int size)
        {
            var findingPartnerService = _serviceFactory.CreateFindingPartnerUserService();
            var count = await findingPartnerService.Count();
            var findingPartnerUsers = await findingPartnerService.GetForPagination(index, size);
            return Ok(new {partnerFinders = findingPartnerUsers, count});
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetWithFilteringCondition(int level, string location, int index, int size)
        {
            //Expression<Func<UserInformation, bool>> condition = string.IsNullOrEmpty(location) ? (u) =

            var userInfos = await _serviceFactory.CreateUserInformationService().GetManyWithCondition(u => (int)u.Level == level && u.Location == location);

            var findingPartnerUserService = _serviceFactory.CreateFindingPartnerUserService();
            var findingPartnerUsers = await findingPartnerUserService
                .GetForPaginationWithGivenUsers(userInfos, index, size);
            var count = await findingPartnerUserService.CountWithGivenUsers(userInfos);
            return Ok(new {partnerFinders = findingPartnerUsers, count});
        }
    }
}