
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

        Task<QuestionPostDetailDto> AddQuestionPost(QuestionPostDto questionPostDto, string userId);
        Task<QuestionPostDetailDto> GetQuestionPost(int id);
    }
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepo;
        private readonly ICommentRepository _commentRepo;
        private readonly IUserInformationRepository _userInformationRepo;
        private readonly IMapper _mapper;

        public PostService(IPostRepository postRepo, ICommentRepository commentRepo, IUserInformationRepository userInformationRepo, IMapper mapper)
        {
            _postRepo = postRepo;
            _commentRepo = commentRepo;
            _userInformationRepo = userInformationRepo;
            _mapper = mapper;
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
                dashBoardPost.AnswerNumber = await _commentRepo.Count(c => c.PostId == post.Id);
                dashBoardPosts.Add(dashBoardPost);
            }

            return dashBoardPosts;
        }

        public async Task<QuestionPostDetailDto> AddQuestionPost(QuestionPostDto questionPostDto, string userId)
        {
            var post = _mapper.Map<Post>(questionPostDto);
            post.Type = PostType.Question;
            post.UserId = userId;
            post.CreatedDate = DateTime.UtcNow;
            post.UpdatedDate = DateTime.UtcNow;
            _postRepo.Add(post);
            var addedPost = post;
            // return addedPost;
            var userInfo = await _userInformationRepo.GetOne(userId);
            var questionPostDetail = _mapper.Map<QuestionPostDetailDto>(userInfo)
                .Map(addedPost, _mapper);
            return questionPostDetail;
            // return post;
        }

        public async Task<QuestionPostDetailDto> GetQuestionPost(int id)
        {
            var post = await _postRepo.GetOne(id);
            var userInfo = await _userInformationRepo.GetOne(post.UserId);
            var questionPostDetail = _mapper.Map<QuestionPostDetailDto>(userInfo)
                .Map(post, _mapper);
            questionPostDetail.AnswerNumber = await _commentRepo.Count(c => c.PostId == post.Id);
            return questionPostDetail;
        }


    }
}
