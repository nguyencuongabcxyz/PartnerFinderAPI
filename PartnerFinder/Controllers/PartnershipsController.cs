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
            var partnership = await _partnershipService.AddOne(userId, partnerId);
            return Ok(partnership);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPartners()
        {
            var userId = GetUserId();
            var partnerships = await _partnershipService.GetAll(userId);
            return Ok(partnerships);
        }
    }
}