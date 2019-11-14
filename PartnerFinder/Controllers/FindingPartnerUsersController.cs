using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartnerFinder.CustomFilters;
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
        public FindingPartnerUsersController(IFindingPartnerUserService findingPartnerUserService)
        {
            _findingPartnerUserService = findingPartnerUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetForPagination(int index, int size)
        {
            var userId = GetUserId();
            var count = await _findingPartnerUserService.Count(userId);
            var findingPartnerUsers = await _findingPartnerUserService.GetForPagination(userId, index, size);

            return Ok(new {partnerFinders = findingPartnerUsers, count});
        }
    }
}