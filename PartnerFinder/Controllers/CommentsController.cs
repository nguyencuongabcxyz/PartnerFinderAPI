using System.Threading.Tasks;
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
            var responseComment = await _commentService.MapModelToResponseComment(comment);
            return Ok(responseComment);
        }
    }
}