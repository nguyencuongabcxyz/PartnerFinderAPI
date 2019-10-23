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
        public async Task<IActionResult> GetQuestionPostsForPagination(int index, int size)
        {
            var questionPosts = await _postService.GetQuestionPostsForPagination(index, size);
            var count = await _postService.CountQuestionPosts();
            return Ok(new {questionPosts, count});
        }

        [HttpGet("feedback-posts")]
        public async Task<IActionResult> GetFeedbackPostsForPagination(int index, int size)
        {
            var feedbackPosts = await _postService.GetFeedbackPostsForPagination(index, size);
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
    }
}