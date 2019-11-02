using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Models;
using Data.Repositories;

namespace Service.Services
{
    public interface IPostReactionService
    {
        Task SwitchReaction(int postId, string userId, PostReactionType type);
        Task<int> Count(Expression<Func<PostReaction, bool>> condition);
    }
    public class PostReactionService : IPostReactionService
    {
        private readonly IPostReactionRepository _postReactionRepo;

        public PostReactionService(IPostReactionRepository postReactionRepo)
        {
            _postReactionRepo = postReactionRepo;
        }

        public async Task SwitchReaction(int postId, string userId, PostReactionType type)
        {
            var existingReaction = await _postReactionRepo.GetOneByCondition(r => r.PostId == postId && r.UserId == userId);
            if (existingReaction != null)
            {
                _postReactionRepo.Remove(existingReaction);
                return;
            }
            var postReaction = new PostReaction
            {
                PostId = postId,
                UserId = userId,
                Type = type
            };
            await _postReactionRepo.Add(postReaction);
        }

        public Task<int> Count(Expression<Func<PostReaction, bool>> condition)
        {
            return _postReactionRepo.Count(condition);
        }
    }
}
