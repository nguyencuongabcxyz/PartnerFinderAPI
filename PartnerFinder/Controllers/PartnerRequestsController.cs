using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartnerFinder.CustomFilters;
using Service.Services;
using System.Threading.Tasks;

namespace PartnerFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ObjectExistenceFilter))]
    [Authorize]
    public class PartnerRequestsController : CommonBaseController
    {
        private readonly IPartnerRequestService _partnerRequestService;
        public PartnerRequestsController(IPartnerRequestService partnerRequestService)
        {
            _partnerRequestService = partnerRequestService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPartnerRequests(int index, int size)
        {
            var userId = GetUserId();
            var partnerRequests = await _partnerRequestService.GetAll(userId, index, size);
            return Ok(partnerRequests);
        }
    }
}