using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace PartnerFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;

        public UsersController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        [HttpGet("{id}/checkInfo")]
        public async Task<IActionResult> CheckIfUserUpdateInfo(string id)
        {
            var userInformationService = _serviceFactory.CreateUserInformationService();
            var isUserExisting = await userInformationService.CheckExistence(id);
            if (!isUserExisting)
            {
                return NotFound();
            }

            var isHavingInfo = await userInformationService.CheckIfUserHaveSpecification(m => m.UserId == id);
            var isHavingLevel = await userInformationService.CheckIfUserHaveSpecification(m => m.UserId == id && m.Level != null);


            return Ok(new {isHavingInfo, isHavingLevel });
        }

    }
}