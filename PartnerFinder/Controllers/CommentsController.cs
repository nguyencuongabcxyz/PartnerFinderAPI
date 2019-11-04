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
        private readonly IUnitOfWork _unitOfWork;

        public CommentsController(ICommentService commentService, IUnitOfWork unitOfWork)
        {
            _commentService = commentService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> PostComment(RequestCommentDto requestComment)
        {
            var userId = GetUserId();
            requestComment.UserId = userId;
            var comment = await _commentService.AddOne(requestComment);
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