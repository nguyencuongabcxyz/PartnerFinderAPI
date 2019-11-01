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
        Task<IEnumerable<ResponseCommentDto>> GetPostComments(int postId);
        Task<Comment> AddOne(RequestCommentDto requestComment);
        Task<int> Count(Expression<Func<Comment, bool>> condition);
        Task<ResponseCommentDto> MapModelToResponseComment(Comment comment);
    }
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IUserInformationRepository _userInformationRepo;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository commentRepo, IMapper mapper, IUserInformationRepository userInformationRepo)
        {
            _commentRepo = commentRepo;
            _mapper = mapper;
            _userInformationRepo = userInformationRepo;
        }

        public async Task<IEnumerable<ResponseCommentDto>> GetPostComments(int postId)
        {
            var comments = await _commentRepo.GetManyWithSubComment(c => c.IsDeleted != true && c.PostId == postId);
            var commentList = comments.ToList();
            var responseComments = await MapModelCollectionToResponseCommentCollection(commentList);
            for (var i = 0; i < commentList.Count(); i++)
            {
                responseComments[i].SubComments =
                    await MapModelCollectionToResponseCommentCollection(commentList[i].SubComments.ToList());
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
            return await _commentRepo.Count(condition);
        }

        public async Task<ResponseCommentDto> MapModelToResponseComment(Comment comment)
        {
            var userInfo = await _userInformationRepo.GetOne(comment.UserId);
            var responseComment = _mapper.Map<ResponseCommentDto>(userInfo)
                .Map(comment, _mapper);
            return responseComment;
        }

        private async Task<List<ResponseCommentDto>> MapModelCollectionToResponseCommentCollection(
            List<Comment> comments)
        {
            var responseComments = new List<ResponseCommentDto>();
            foreach (var comment in comments)
            {
                var responseComment = await MapModelToResponseComment(comment);
                responseComments.Add(responseComment);
            }

            return responseComments;
        }
    }
}
