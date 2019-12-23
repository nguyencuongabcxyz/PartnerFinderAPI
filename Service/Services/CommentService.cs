using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Data.Models;
using Data.Repositories;
using Service.Extensions;
using Service.Models;

namespace Service.Services
{
    public interface ICommentService
    {
        Task<Comment> GetOne(int id);
        Task<IEnumerable<ResponseCommentDto>> GetPostComments(int postId, string currentUserId);
        Task<Comment> AddOne(RequestCommentDto requestComment);
        Task<int> Count(Expression<Func<Comment, bool>> condition);
        Task<Comment> SwitchReaction(int id, string userId, CommentReactionType type);
        Task<bool> CheckIfUserLiked(int id, string userId);
        Task<ResponseCommentDto> MapModelToResponseComment(Comment comment, string currentUserId);

        Task<List<ResponseCommentDto>> MapModelCollectionToResponseCommentCollection(
            List<Comment> comments, string currentUserId);
    }
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IUserInformationRepository _userInformationRepo;

        private readonly ICommentReactionService _commentReactionService;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository commentRepo, IMapper mapper, 
            IUserInformationRepository userInformationRepo, 
            ICommentReactionService commentReactionService)
        {
            _commentRepo = commentRepo;
            _mapper = mapper;
            _userInformationRepo = userInformationRepo;
            _commentReactionService = commentReactionService;
        }

        public async Task<IEnumerable<ResponseCommentDto>> GetPostComments(int postId, string currentUserId)
        {
            var comments = await _commentRepo.GetManyWithSubComment(c => c.IsDeleted != true && c.PostId == postId && c.ParentId == null);
            var commentList = comments.ToList();
            var responseComments = await MapModelCollectionToResponseCommentCollection(commentList, currentUserId);
            for (var i = 0; i < commentList.Count(); i++)
            {
                responseComments[i].SubComments =
                    await MapModelCollectionToResponseCommentCollection(commentList[i].SubComments.ToList(), currentUserId);
            }
            return responseComments;
        }


        public async Task<Comment> AddOne(RequestCommentDto requestComment)
        {
            var comment = _mapper.Map<Comment>(requestComment);
            comment.CreatedDate = DateTime.UtcNow;
            await _commentRepo.Add(comment);
            return comment;
        }

        public async Task<int> Count(Expression<Func<Comment, bool>> condition)
        {
            var comments = await _commentRepo.GetManyByCondition(condition);
            return comments.Count();
        }

        public async Task<Comment> SwitchReaction(int id, string userId, CommentReactionType type)
        {
            var comment = await _commentRepo.GetOneWithSubComment(c => c.Id == id);
            var isLiked = await CheckIfUserLiked(id, userId);
            comment.Like = isLiked ? comment.Like - 1 : comment.Like + 1;
            await _commentReactionService.SwitchReaction(id, userId, type);
            return comment;
        }

        public async Task<bool> CheckIfUserLiked(int id, string userId)
        {
            var reactionCount = await _commentReactionService.Count(r => r.CommentId == id && r.UserId == userId);
            return reactionCount > 0;
        }


        public async Task<ResponseCommentDto> MapModelToResponseComment(Comment comment, string currentUserId)
        {
            var userInfo = await _userInformationRepo.GetOne(comment.UserId);
            var responseComment = _mapper.Map<ResponseCommentDto>(userInfo)
                .Map(comment, _mapper);
            responseComment.IsLiked = await CheckIfUserLiked(comment.Id, currentUserId);
            return responseComment;
        }

        public async Task<List<ResponseCommentDto>> MapModelCollectionToResponseCommentCollection(
            List<Comment> comments, string currentUserId)
        {
            var responseComments = new List<ResponseCommentDto>();
            foreach (var comment in comments)
            {
                var responseComment = await MapModelToResponseComment(comment, currentUserId);
                responseComments.Add(responseComment);
            }

            return responseComments;
        }

        public async Task<Comment> GetOne(int id)
        {
            return await _commentRepo.GetOne(id);
        }
    }
}
