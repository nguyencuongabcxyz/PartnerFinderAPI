using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartnerFinder.Extensions;
using Service.Services;

namespace PartnerFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LevelTestsController : ControllerBase
    {
        private readonly ILevelTestService _levelTestService;

        public LevelTestsController(ILevelTestService levelTestService)
        {
            _levelTestService = levelTestService;
        }

        [HttpGet("random")]
        public async Task<IActionResult> GetRandomTest()
        {
            var levelTests = await _levelTestService.GetAllWithQuestionsAndAnswerOptions();
            var randomTest = levelTests.ToList().GetRandomElement();
            return Ok(randomTest);
        }

    }
}