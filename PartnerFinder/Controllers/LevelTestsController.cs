using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PartnerFinder.Extensions;
using Service;

namespace PartnerFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LevelTestsController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;

        public LevelTestsController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        [HttpGet("random")]
        public async Task<IActionResult> GetRandomTest()
        {
            var levelTests = await _serviceFactory.CreateLevelTestService().GetAllWithQuestionsAndAnswerOptions();
            var randomTest = levelTests.ToList().GetRandomElement();
            return Ok(randomTest);
        }
    }
}