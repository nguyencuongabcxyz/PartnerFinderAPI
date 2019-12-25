
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Data.Models;
using Data.Repositories;
using Service.Constants;
using Service.Extensions;
using Service.Models;

namespace Service.Services
{
    public interface IPostService
    {
        Task<Post> GetOne(int id);
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
        Task<IEnumerable<FeedbackPostDetailDto>> SearchFeedbackPosts(string pattern);
        Task<IEnumerable<QuestionPostDetailDto>> SearchQuestionPosts(string pattern);
        Task<Post> SwitchPostVote(int postId, string userId, PostReactionType type);
        Task<bool> CheckIfUserVoted(int postId, string userId);
        Task ClosePost(int id);
    }
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepo;
        private readonly IUserInformationRepository _userInformationRepo;

        private readonly ICommentService _commentService;
        private readonly IPostReactionService _postReactionService;
        private readonly IMapper _mapper;

        public PostService(IPostRepository postRepo, IUserInformationRepository userInformationRepo, 
            IMapper mapper, ICommentService commentService, IPostReactionService postReactionService)
        {
            _postRepo = postRepo;
            _userInformationRepo = userInformationRepo;
            _mapper = mapper;
            _commentService = commentService;
            _postReactionService = postReactionService;
        }

        public async Task<int> CountQuestionPosts()
        {
            return await _postRepo.Count(p => p.IsClosed != true 
                                              && p.Type == PostType.Question);
        }

        public async Task<int> CountFeedbackPosts()
        {
            Expression<Func<Post, bool>> condition = p => p.IsClosed != true 
                                                          && (p.Type == PostType.SpokenFeedback || p.Type == PostType.WrittenFeedback);
            return await _postRepo.Count(condition);
        }

        public async Task<IEnumerable<DashboardPostDto>> GetFeedbackPostsForDashboard(int index, int size = 8)
        {
            Expression<Func<Post, bool>> condition = p => p.IsClosed != true 
                                                          && (p.Type == PostType.WrittenFeedback || p.Type == PostType.SpokenFeedback);
            var posts = await _postRepo.OrderAndGetRange(index, size, OrderType.OrderByDescending, p => p.UpdatedDate,
                condition);
            return await MapPostsToDashboardPosts(posts);
        }

        public async Task<IEnumerable<DashboardPostDto>> GetQuestionPostsForDashboard(int index, int size = 8)
        {
            Expression<Func<Post, bool>> condition = p => p.IsClosed != true && p.Type == PostType.Question;
            var posts = await _postRepo.OrderAndGetRange(index, size, OrderType.OrderByDescending, p => p.UpdatedDate,
                condition);
            return await MapPostsToDashboardPosts(posts);
        }

        private async Task<IEnumerable<DashboardPostDto>> MapPostsToDashboardPosts(IEnumerable<Post> posts)
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
            return questionPostDetail;
        }

        private async Task<IEnumerable<QuestionPostDetailDto>> MapPostsToQuestionPostsDetail(IEnumerable<Post> posts)
        {
            var questionPostsDetail = new List<QuestionPostDetailDto>();
            foreach (var post in posts)
            {
                questionPostsDetail.Add(await MapPostToQuestionPostDetail(post));
            }

            return questionPostsDetail;
        }

        public async Task<FeedbackPostDetailDto> MapPostToFeedbackPostDetail(Post post)
        {
            var userInfo = await _userInformationRepo.GetOne(post.UserId);
            var feedbackPostDetail = _mapper.Map<FeedbackPostDetailDto>(userInfo)
                .Map(post, _mapper);
            feedbackPostDetail.AnswerNumber = await _commentService.Count(c => c.PostId == post.Id);
            return feedbackPostDetail;
        }

        private async Task<IEnumerable<FeedbackPostDetailDto>> MapPostsToFeedbackPostsDetail(IEnumerable<Post> posts)
        {
            var feedbackPostsDetail = new List<FeedbackPostDetailDto>();
            foreach (var post in posts)
            {
                feedbackPostsDetail.Add(await MapPostToFeedbackPostDetail(post));
            }

            return feedbackPostsDetail;
        }

        public async Task<QuestionPostDetailDto> GetQuestionPost(int id)
        {
            var post = await _postRepo.GetOneByCondition(p => p.Id == id && p.IsClosed != true);
            if (post == null) return null;
            return await MapPostToQuestionPostDetail(post);
        }

        public async Task<FeedbackPostDetailDto> GetFeedbackPost(int id)
        {
            var post = await _postRepo.GetOneByCondition(p => p.Id == id && p.IsClosed != true);
            if (post == null) return null;
            return await MapPostToFeedbackPostDetail(post);
        }

        public async Task<IEnumerable<FeedbackPostDetailDto>> SearchFeedbackPosts(string pattern)
        {
            if (pattern == null) return null;
            var arrayPattern = pattern.Split(" ");
            var posts = await _postRepo.GetManyByCondition(p => p.IsClosed != true
                                                                && (p.Type == PostType.SpokenFeedback ||
                                                                    p.Type == PostType.WrittenFeedback)
                                                                && arrayPattern.All(s => p.Title.CaseInsensitiveContains(s)), CommonConstant.SearchLimit);
            return await MapPostsToFeedbackPostsDetail(posts);
        }

        public async Task<IEnumerable<QuestionPostDetailDto>> SearchQuestionPosts(string pattern)
        {
            var arrayPattern = pattern.Split(" ");
            var posts = await _postRepo.GetManyByCondition(p => p.IsClosed != true
                                                                && (p.Type == PostType.Question)
                                                                && arrayPattern.All(s => p.Title.CaseInsensitiveContains(s)), CommonConstant.SearchLimit);
            return await MapPostsToQuestionPostsDetail(posts);
        }

        public async Task<Post> SwitchPostVote(int postId, string userId, PostReactionType type)
        {
            var post = await _postRepo.GetOne(postId);
            var isVoted = await CheckIfUserVoted(postId, userId);
            post.UpVote = isVoted ? post.UpVote - 1 : post.UpVote + 1;
            await _postReactionService.SwitchReaction(postId, userId, type);
            return post;
        }

        public async Task<bool> CheckIfUserVoted(int postId, string userId)
        {
            var reactionCount = await _postReactionService.Count(r => r.UserId == userId && r.PostId == postId);
            return reactionCount > 0;
        }

        public async Task<Post> GetOne(int id)
        {
            return await _postRepo.GetOneByCondition( p => p.Id == id && p.IsClosed != true);
        }

        public async Task ClosePost(int id)
        {
            var post = await _postRepo.GetOne(id);
            post.IsClosed = true;
        }
    }
}
