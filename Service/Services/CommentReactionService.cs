using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Models;
using Data.Repositories;

namespace Service.Services
{
    public interface ICommentReactionService
    {
        Task<int> Count(Expression<Func<CommentReaction, bool>> condition);
        Task SwitchReaction(int commentId, string userId, CommentReactionType type);
    }
    public class CommentReactionService : ICommentReactionService
    {
        private readonly ICommentReactionRepository _commentReactionRepo;

        public CommentReactionService(ICommentReactionRepository commentReactionRepo)
        {
            _commentReactionRepo = commentReactionRepo;
        }

        public async Task<int> Count(Expression<Func<CommentReaction, bool>> condition)
        {
            return await _commentReactionRepo.Count(condition);
        }

        public async Task SwitchReaction(int commentId, string userId, CommentReactionType type)
        {
            var existingReaction =
                await _commentReactionRepo.GetOneByCondition(r => r.CommentId == commentId && r.UserId == userId);
            if (existingReaction != null)
            {
                _commentReactionRepo.Remove(existingReaction);
                return;
            }
            var commentReaction = new CommentReaction()
            {
                UserId = userId,
                CommentId = commentId,
                Type = type,
            };
            await _commentReactionRepo.Add(commentReaction);
        }
    }
}
