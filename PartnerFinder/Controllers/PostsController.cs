using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PartnerFinder.CustomFilters;
using Service;

namespace PartnerFinder.Controllers
{
    [ServiceFilter(typeof(ObjectExistenceFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;

        public PostsController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok(new {message = "abc"});
        }

        [HttpGet("questionPosts/{index}/{size}")]
        public async Task<IActionResult> GetQuestionPostsForPagination(int index, int size)
        {
            var postService = _serviceFactory.CreatePostService();
            var questionPosts = await postService.GetQuestionPostsForPagination(index, size);
            var count = await postService.CountQuestionPosts();
            return Ok(new {questionPosts, count});
        }

        [HttpGet("feedbackPosts/{index}/{size}")]
        public async Task<IActionResult> GetFeedbackPostsForPagination(int index, int size)
        {
            var postService = _serviceFactory.CreatePostService();
            var feedbackPosts = await postService.GetFeedbackPostsForPagination(index, size);
            var count = await postService.CountFeedbackPosts();
            return Ok(new { feedbackPosts, count });
        }
    }
}