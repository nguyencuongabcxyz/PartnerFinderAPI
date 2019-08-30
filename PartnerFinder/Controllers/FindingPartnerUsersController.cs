using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace PartnerFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FindingPartnerUsersController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;
        public FindingPartnerUsersController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        [HttpGet("{index}/{size}")]
        public async Task<IActionResult> GetAll(int index, int size)
        {
            var findingPartnerService = _serviceFactory.CreateFindingPartnerUserService();
            var count = await findingPartnerService.Count();
            var findingPartnerUsers = await findingPartnerService.GetForPagination(index, size);
            return Ok(new {partnerFinders = findingPartnerUsers, count});
        }
    }
}