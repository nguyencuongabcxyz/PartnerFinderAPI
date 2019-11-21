using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartnerFinder.CustomFilters;
using Service.Models;
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

        [HttpPost]
        public async Task<IActionResult> AddOne(ReqPartnerRequestDto reqPartnerRequest)
        {
            var userId = GetUserId();
            reqPartnerRequest.SenderId = userId;
            await _partnerRequestService.AddOne(reqPartnerRequest);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPartnerRequests(int index, int size)
        {
            var userId = GetUserId();
            var partnerRequests = await _partnerRequestService.GetAll(userId, index, size);
            var count = await _partnerRequestService.Count(userId);
            return Ok(new { partnerRequests, count });
        }
    }
}