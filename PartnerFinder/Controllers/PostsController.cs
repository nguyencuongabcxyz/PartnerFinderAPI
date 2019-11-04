using System.Threading.Tasks;
using Data.Models;
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
        private readonly ICommentService _commentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostReactionService _postReactionService;

        public PostsController(IPostService postService, IUnitOfWork unitOfWork, 
            ICommentService commentService, IPostReactionService postReactionService)
        {
            _postService = postService;
            _unitOfWork = unitOfWork;
            _commentService = commentService;
            _postReactionService = postReactionService;
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
            var questionPostDetail = await _postService.MapPostToQuestionPostDetail(post);
            return Ok(questionPostDetail);
        }

        [HttpPost("feedback-post")]
        public async Task<IActionResult> PostFeedbackPost(FeedbackPostDto feedbackPost)
        {
            var userId = GetUserId();
            var post = await _postService.AddFeedbackPost(feedbackPost, userId);
            await _unitOfWork.Commit();
            var feedbackPostDetail = await _postService.MapPostToFeedbackPostDetail(post);
            return Ok(feedbackPostDetail);
        }

        [HttpGet("{id}/question-post")]
        public async Task<IActionResult> GetQuestionPost(int id)
        {
            var questionPostDetail = await _postService.GetQuestionPost(id);
            return Ok(questionPostDetail);
        }

        [HttpGet("{id}/comments")]
        public async Task<IActionResult> GetPostComments(int id)
        {
            var userId = GetUserId();
            var comments = await _commentService.GetPostComments(id, userId);
            return Ok(comments);
        }

        [HttpGet("{id}/feedback-post")]
        public async Task<IActionResult> GetFeedbackPost(int id)
        {
            var feedbackPostDetail = await _postService.GetFeedbackPost(id);
            return Ok(feedbackPostDetail);
        }

        [HttpPatch("{id}/question-post/up-vote")]
        public async Task<IActionResult> UpdateUpVoteQuestionPost(int id)
        {
            var userId = GetUserId();
            var post = await _postService.SwitchPostVote(id, userId, PostReactionType.UpVote);
            await _unitOfWork.Commit();
            var questionPostDetail = await _postService.MapPostToQuestionPostDetail(post);
            return Ok(questionPostDetail);
        }

        [HttpPatch("{id}/feedback-post/up-vote")]
        public async Task<IActionResult> UpdateUpVoteFeedbackPost(int id)
        {
            var userId = GetUserId();
            var post = await _postService.SwitchPostVote(id, userId, PostReactionType.UpVote);
            await _unitOfWork.Commit();
            var feedbackPostDetail = await _postService.MapPostToFeedbackPostDetail(post);
            return Ok(feedbackPostDetail);
        }

        [HttpGet("{id}/check-vote")]
        public async Task<IActionResult> CheckIfUserLikedPost(int id)
        {
            var userId = GetUserId();
            var isVoted = await _postService.CheckIfUserVoted(id, userId);
            return Ok(isVoted);
        }
    }
}