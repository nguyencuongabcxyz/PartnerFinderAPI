using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartnerFinder.CustomFilters;
using Service;
using Service.Models;
using Service.Services;

namespace PartnerFinder.Controllers
{
    [ServiceFilter(typeof(ObjectExistenceFilter))]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostsController : CommonBaseController
    {
        private readonly IPostService _postService;
        private readonly IUnitOfWork _unitOfWork;

        public PostsController(IPostService postService, IUnitOfWork unitOfWork)
        {
            _postService = postService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("question-posts")]
        public async Task<IActionResult> GetQuestionPostsForDashboard(int index, int size)
        {
            var questionPosts = await _postService.GetQuestionPostsForDashboard(index, size);
            var count = await _postService.CountQuestionPosts();
            return Ok(new {questionPosts, count});
        }

        [HttpGet("feedback-posts")]
        public async Task<IActionResult> GetFeedbackPostsForDashboard(int index, int size)
        {
            var feedbackPosts = await _postService.GetFeedbackPostsForDashboard(index, size);
            var count = await _postService.CountFeedbackPosts();
            return Ok(new { feedbackPosts, count });
        }

        [HttpPost("question-post")]
        public async Task<IActionResult> PostQuestionPost(QuestionPostDto questionPost)
        {
            var userId = GetUserId();
            var post = await _postService.AddQuestionPost(questionPost, userId);
            await _unitOfWork.Commit();
            return Ok(post);
        }

        [HttpGet("{id}/question-post")]
        public async Task<IActionResult> GetQuestionPost(int id)
        {
            var questionPost = await _postService.GetQuestionPost(id);
            return Ok(questionPost);
        }
    }
}