using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartnerFinder.CustomFilters;
using Service;
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
        private readonly IUnitOfWork _unitOfWork;
        public PartnerRequestsController(IPartnerRequestService partnerRequestService, IUnitOfWork unitOfWork)
        {
            _partnerRequestService = partnerRequestService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> AddOne(ReqPartnerRequestDto reqPartnerRequest)
        {
            var userId = GetUserId();
            reqPartnerRequest.SenderId = userId;
            await _partnerRequestService.AddOne(reqPartnerRequest);
            await _unitOfWork.Commit();
            return Ok(new { result = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPartnerRequests()
        {
            var userId = GetUserId();
            var partnerRequests = await _partnerRequestService.GetAll(userId);
            var count = await _partnerRequestService.Count(userId);
            return Ok(new { partnerRequests, count });
        }

        [HttpGet("count")]
        public async Task<IActionResult> Count()
        {
            var userId = GetUserId();
            var count = await _partnerRequestService.Count(userId);
            return Ok(count);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveOne(int id)
        {
            await _partnerRequestService.RemoveOne(id);
            await _unitOfWork.Commit();
            return Ok(new { result = true });
        }

        [HttpDelete("{id}/accept")]
        public async Task<IActionResult> AcceptOne(int id)
        {
            await _partnerRequestService.AcceptOne(id);
            await _unitOfWork.Commit();
            return Ok(new { result = true });
        }

    }
}