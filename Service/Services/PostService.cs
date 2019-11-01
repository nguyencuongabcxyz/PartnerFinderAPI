
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Data.Models;
using Data.Repositories;
using Service.Extensions;
using Service.Models;

namespace Service.Services
{
    public interface IPostService
    {
        Task<IEnumerable<DashboardPostDto>> GetQuestionPostsForDashboard(int index, int size = 8);
        Task<IEnumerable<DashboardPostDto>> GetFeedbackPostsForDashboard(int index, int size = 8);
        Task<int> CountQuestionPosts();
        Task<int> CountFeedbackPosts();

        Task<Post> AddQuestionPost(QuestionPostDto questionPostDto, string userId);
        Task<Post> AddFeedbackPost(FeedbackPostDto feedbackPostDto, string userId);
        Task<QuestionPostDetailDto> MapPostToQuestionPostDetail(Post post);
        Task<FeedbackPostDetailDto> MapPostToFeedbackPostDetail(Post post);
        Task<QuestionPostDetailDto> GetQuestionPost(int id);
        Task<FeedbackPostDetailDto> GetFeedbackPost(int id);
        Task<QuestionPostDetailDto> UpdateQuestionPostVote(int postId, string userId, PostReactionType type);
        Task<bool> CheckIfUserVoted(int postId, string userId);
    }
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepo;
        private readonly IUserInformationRepository _userInformationRepo;

        private readonly ICommentService _commentService;
        private readonly IPostReactionService _postReactionService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PostService(IPostRepository postRepo, IUserInformationRepository userInformationRepo, IMapper mapper, ICommentService commentService, IPostReactionService postReactionService, IUnitOfWork unitOfWork)
        {
            _postRepo = postRepo;
            _userInformationRepo = userInformationRepo;
            _mapper = mapper;
            _commentService = commentService;
            _postReactionService = postReactionService;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CountQuestionPosts()
        {
            return await _postRepo.Count(p => p.IsDeleted != true && p.Type == PostType.Question);
        }

        public async Task<int> CountFeedbackPosts()
        {
            Expression<Func<Post, bool>> condition = p => p.IsDeleted != true && (p.Type == PostType.SpokenFeedback || p.Type == PostType.WrittenFeedback);
            return await _postRepo.Count(condition);
        }

        public async Task<IEnumerable<DashboardPostDto>> GetFeedbackPostsForDashboard(int index, int size = 8)
        {
            Expression<Func<Post, bool>> condition = p => p.IsDeleted != true && (p.Type == PostType.WrittenFeedback || p.Type == PostType.SpokenFeedback);
            var posts = await _postRepo.OrderAndGetRange(index, size, OrderType.OrderByDescending, p => p.UpdatedDate,
                condition);
            return await MapPostToDashboardPost(posts);
        }

        public async Task<IEnumerable<DashboardPostDto>> GetQuestionPostsForDashboard(int index, int size = 8)
        {
            Expression<Func<Post, bool>> condition = p => p.IsDeleted != true && p.Type == PostType.Question;
            var posts = await _postRepo.OrderAndGetRange(index, size, OrderType.OrderByDescending, p => p.UpdatedDate,
                condition);
            return await MapPostToDashboardPost(posts);
        }

        private async Task<IEnumerable<DashboardPostDto>> MapPostToDashboardPost(IEnumerable<Post> posts)
        {
            var dashBoardPosts = new List<DashboardPostDto>();
            foreach (var post in posts)
            {
                var userInfo = await _userInformationRepo.GetOne(post.UserId);
                var dashBoardPost = _mapper.Map<DashboardPostDto>(userInfo)
                                           .Map(post, _mapper);
                dashBoardPost.AnswerNumber = await _commentService.Count(c => c.PostId == post.Id);
                dashBoardPosts.Add(dashBoardPost);
            }

            return dashBoardPosts;
        }

        public async Task<Post> AddQuestionPost(QuestionPostDto questionPostDto, string userId)
        {
            var post = _mapper.Map<Post>(questionPostDto);
            post.Type = PostType.Question;
            post.UserId = userId;
            post.CreatedDate = DateTime.UtcNow;
            post.UpdatedDate = DateTime.UtcNow;
            await _postRepo.Add(post);
            return post;
        }

        public async Task<Post> AddFeedbackPost(FeedbackPostDto feedbackPostDto, string userId)
        {
            var post = _mapper.Map<Post>(feedbackPostDto);
            post.UserId = userId;
            post.CreatedDate = DateTime.UtcNow;
            post.UpdatedDate = DateTime.UtcNow;
            await _postRepo.Add(post);
            return post;
        }

        public async Task<QuestionPostDetailDto> MapPostToQuestionPostDetail(Post post)
        {
            var userInfo = await _userInformationRepo.GetOne(post.UserId);
            var questionPostDetail = _mapper.Map<QuestionPostDetailDto>(userInfo)
                .Map(post, _mapper);
            questionPostDetail.AnswerNumber = await _commentService.Count(c => c.PostId == post.Id);
            questionPostDetail.UpVote = await _postReactionService.Count(r => r.PostId == post.Id);
            return questionPostDetail;
        }

        public async Task<FeedbackPostDetailDto> MapPostToFeedbackPostDetail(Post post)
        {
            var userInfo = await _userInformationRepo.GetOne(post.UserId);
            var feedbackPostDetail = _mapper.Map<FeedbackPostDetailDto>(userInfo)
                .Map(post, _mapper);
            feedbackPostDetail.AnswerNumber = await _commentService.Count(c => c.PostId == post.Id);
            feedbackPostDetail.UpVote = await _postReactionService.Count(r => r.PostId == post.Id);
            return feedbackPostDetail;
        }

        public async Task<QuestionPostDetailDto> GetQuestionPost(int id)
        {
            var post = await _postRepo.GetOne(id);
            return await MapPostToQuestionPostDetail(post);
        }

        public async Task<FeedbackPostDetailDto> GetFeedbackPost(int id)
        {
            var post = await _postRepo.GetOne(id);
            return await MapPostToFeedbackPostDetail(post);
        }

        public async Task<QuestionPostDetailDto> UpdateQuestionPostVote(int postId, string userId, PostReactionType type)
        {
            var post = await _postRepo.GetOne(postId);
            var reactionCountOfUser = await _postReactionService.Count(r => r.UserId == userId && r.PostId == postId);
            if (reactionCountOfUser != 0)
            {
                return await MapPostToQuestionPostDetail(post);
            }
            await _postReactionService.AddOne(postId, userId, type);
            await _unitOfWork.Commit();
            post.Upvote = await _postReactionService.Count(r => r.PostId == postId);
            return await MapPostToQuestionPostDetail(post);
        }

        public async Task<bool> CheckIfUserVoted(int postId, string userId)
        {
            var reactionCountOfUser = await _postReactionService.Count(r => r.UserId == userId && r.PostId == postId);
            return reactionCountOfUser > 0;
        }
    }
}
