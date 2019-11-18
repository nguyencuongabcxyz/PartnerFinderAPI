using System.Threading.Tasks;
using Data.Models;
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
    public class PartnerFindersController : CommonBaseController
    {
        private readonly IFindingPartnerUserService _findingPartnerUserService;
        private readonly IUserInformationService _userInformationService;
        public PartnerFindersController(IFindingPartnerUserService findingPartnerUserService, IUserInformationService userInformationService)
        {
            _findingPartnerUserService = findingPartnerUserService;
            _userInformationService = userInformationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetForPagination(string location,UserLevel level, int index, int size)
        {
            var userId = GetUserId();
            var count = await _userInformationService.CountPartnerFinders(userId, location, level);
            var partnerFinders = await _userInformationService.GetPartnerFinders(userId, location, level, index, size);

            return Ok(new {partnerFinders, count});
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchPartnerFinders(string location)
        {
            var userId = GetUserId();
            var partnerFinders = await _findingPartnerUserService.SearchByLocation(userId, location);
            return Ok(partnerFinders);
        }
    }
}