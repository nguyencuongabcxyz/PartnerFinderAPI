using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartnerFinder.CustomFilters;
using Service;
using Service.Services;

namespace PartnerFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ObjectExistenceFilter))]
    [Authorize]
    public class PartnershipsController : CommonBaseController
    {
        private readonly IPartnershipService _partnershipService;
        private readonly IUnitOfWork _unitOfWork;
        public PartnershipsController(IPartnershipService partnershipService, IUnitOfWork unitOfWork)
        {
            _partnershipService = partnershipService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> AddPartnership(string partnerId)
        {
            var userId = GetUserId();
            await _partnershipService.AddOne(userId, partnerId);
            await _unitOfWork.Commit();
            return Ok(new { result = true });
        }
    }
}