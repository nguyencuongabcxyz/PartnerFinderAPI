
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Data;
using Data.Models;
using Data.Repositories;
using Service.Extensions;
using Service.Models;

namespace Service.Services
{
    public interface IPostService
    {
        Task<IEnumerable<DashboardPostDto>> GetQuestionPostsForPagination(int index, int size = 8);
        Task<IEnumerable<DashboardPostDto>> GetFeedbackPostsForPagination(int index, int size = 8);
        Task<int> CountQuestionPosts();
        Task<int> CountFeedbackPosts();
    }
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepo;
        private readonly ICommentRepository _commentRepo;
        private readonly IUserInformationRepository _userInformationRepo;
        private readonly IMapper _mapper;

        public PostService(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _postRepo = repositoryFactory.CreatePostRepo();
            _commentRepo = repositoryFactory.CreateCommentRepo();
            _userInformationRepo = repositoryFactory.CreateUserInformationRepo();
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

        public async Task<IEnumerable<DashboardPostDto>> GetFeedbackPostsForPagination(int index, int size = 8)
        {
            Expression<Func<Post, bool>> condition = p => p.IsDeleted != true && (p.Type == PostType.WrittenFeedback || p.Type == PostType.SpokenFeedback);
            var posts = await _postRepo.OrderAndGetRange(index, size, OrderType.OrderByDescending, p => p.UpdatedDate,
                condition);
            return await MapModelToDtoModel(posts);
        }

        public async Task<IEnumerable<DashboardPostDto>> GetQuestionPostsForPagination(int index, int size = 8)
        {
            Expression<Func<Post, bool>> condition = p => p.IsDeleted != true && p.Type == PostType.Question;
            var posts = await _postRepo.OrderAndGetRange(index, size, OrderType.OrderByDescending, p => p.UpdatedDate,
                condition);
            return await MapModelToDtoModel(posts);
        }

        private async Task<IEnumerable<DashboardPostDto>> MapModelToDtoModel(IEnumerable<Post> posts)
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
    }
}
