using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PartnerFinder.CustomFilters;
using Service.Services;

namespace PartnerFinder.Controllers
{
    [ServiceFilter(typeof(ObjectExistenceFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("questionPosts")]
        public async Task<IActionResult> GetQuestionPostsForPagination(int index, int size)
        {
            var questionPosts = await _postService.GetQuestionPostsForPagination(index, size);
            var count = await _postService.CountQuestionPosts();
            return Ok(new {questionPosts, count});
        }

        [HttpGet("feedbackPosts")]
        public async Task<IActionResult> GetFeedbackPostsForPagination(int index, int size)
        {
            var feedbackPosts = await _postService.GetFeedbackPostsForPagination(index, size);
            var count = await _postService.CountFeedbackPosts();
            return Ok(new { feedbackPosts, count });
        }
    }
}