using System.Linq;
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
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ObjectExistenceFilter))]
    [Authorize]
    public class CommentsController : CommonBaseController
    {
        private readonly ICommentService _commentService;
        private readonly INotificationService _notificationService;
        private readonly IPostService _postService;
        private readonly IUnitOfWork _unitOfWork;

        public CommentsController(ICommentService commentService, IUnitOfWork unitOfWork, INotificationService notificationService, IPostService postService)
        {
            _commentService = commentService;
            _notificationService = notificationService;
            _postService = postService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> PostComment(RequestCommentDto requestComment)
        {
            var userId = GetUserId();
            requestComment.UserId = userId;
            var post = await _postService.GetOne((int)requestComment.PostId);
            var comment = await _commentService.AddOne(requestComment);
            if (requestComment.ParentId != null)
            {
                var parentComment = await _commentService.GetOne((int)requestComment.ParentId);
                await _notificationService.CreateOne(parentComment.UserId, userId, (int)requestComment.PostId, NotificationType.CommentReply);
            }
            else
            {
                await _notificationService.CreateOne(post.UserId, userId, (int)requestComment.PostId, NotificationType.PostComment);
            }
            await _unitOfWork.Commit();
            var responseComment = await _commentService.MapModelToResponseComment(comment, userId);
            return Ok(responseComment);
        }

        [HttpPatch("{id}/like")]
        public async Task<IActionResult> SwitchLikeReaction(int id)
        {
            var userId = GetUserId();
            var comment = await _commentService.SwitchReaction(id, userId, CommentReactionType.Like);
            await _unitOfWork.Commit();
            var responseComment = await _commentService.MapModelToResponseComment(comment, userId);
            responseComment.IsLiked = await _commentService.CheckIfUserLiked(id, userId);
            responseComment.SubComments =
                await _commentService.MapModelCollectionToResponseCommentCollection(comment.SubComments.ToList(), userId);
            return Ok(responseComment);
        }

        [HttpGet("{id}/check-like")]
        public async Task<IActionResult> CheckIfUserLiked(int id)
        {
            var userId = GetUserId();
            var isLiked = await _commentService.CheckIfUserLiked(id, userId);
            return Ok(isLiked);
        }

    }
}