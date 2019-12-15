using AutoMapper;
using Data.Models;
using Data.Repositories;
using Service.Constants;
using Service.Extensions;
using Service.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public interface INotificationService
    {
        Task CreateOne(string ownerId, string creatorId, int postId, NotificationType type);
        Task RemoveOne(int id);
        Task<NotificationDto> MarkView(int id);
        Task CreatePostLikeNoti(string ownerId, string creatorId, int postId);
        Task<IEnumerable<NotificationDto>> GetAll(string userId);
        Task<bool> CheckExistenceOfPostLikeNoti(string ownerId, string creatorId, int postId);
        Task<NotificationDto> MapNotificationToNotificationDto(Notification notification);
        Task<int> Count(string userId);
    }
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepo;
        private readonly IUserInformationRepository _userInformationRepo;
        private readonly IPostRepository _postRepo;
        private readonly IMapper _mapper;
        public NotificationService(INotificationRepository notificationRepo, IUserInformationRepository userInformationRepo, IMapper mapper, IPostRepository postRepo)
        {
            _notificationRepo = notificationRepo;
            _userInformationRepo = userInformationRepo;
            _postRepo = postRepo;
            _mapper = mapper;
        }

        public async Task CreateOne(string ownerId, string creatorId, int postId, NotificationType type)
        {
            var notification = new Notification()
            {
                Content = CommonConstant.NotificationContentDic[type],
                CreatedDate = System.DateTime.UtcNow,
                Type = type,
                PostId = postId,
                OwnerId = ownerId,
                CreatorId = creatorId,
            };
            await _notificationRepo.Add(notification);
        }

        public async Task<NotificationDto> MapNotificationToNotificationDto(Notification notification)
        {
            var user = await _userInformationRepo.GetOne(notification.CreatorId);
            var post = await _postRepo.GetOne(notification.PostId);
            var notificationDto = _mapper.Map<NotificationDto>(user)
                                        .Map(notification, _mapper);
            notificationDto.PostType = post.Type;
            return notificationDto;
        }

        private async Task<IEnumerable<NotificationDto>> MapNotificationsToNotificationDtos(IEnumerable<Notification> notifications)
        {
            var notificationDtos = new List<NotificationDto>();
            foreach(var noti in notifications)
            {
                var notificationDto = await MapNotificationToNotificationDto(noti);
                notificationDtos.Add(notificationDto);
            }
            return notificationDtos;
        }

        public async Task<IEnumerable<NotificationDto>> GetAll(string userId)
        {
            var notifications = await _notificationRepo.GetManyByCondition(p => p.IsDeleted != true && p.OwnerId == userId);
            var notificationList = notifications.ToList().OrderByDescending(n => n.CreatedDate);
            return await MapNotificationsToNotificationDtos(notificationList);
        }

        public async Task<int> Count(string userId)
        {
            return await _notificationRepo.Count(p => p.IsDeleted != true && p.OwnerId == userId);
        }

        public async Task<bool> CheckExistenceOfPostLikeNoti(string ownerId, string creatorId, int postId)
        {
            var noti = await _notificationRepo.GetOneByCondition(n => n.OwnerId == ownerId 
            && n.CreatorId == creatorId 
            && n.PostId == postId 
            && n.Type == NotificationType.PostLike);
            return noti != null;
        }

        public async Task CreatePostLikeNoti(string ownerId, string creatorId, int postId)
        {
            var checkExistence = await CheckExistenceOfPostLikeNoti(ownerId, creatorId, postId);
            if(!checkExistence)
            {
                await CreateOne(ownerId, creatorId, postId, NotificationType.PostLike);
            }
        }

        public async Task RemoveOne(int id)
        {
            var noti = await _notificationRepo.GetOne(id);
            _notificationRepo.Remove(noti);
        }

        public async Task<NotificationDto> MarkView(int id)
        {
            var noti = await _notificationRepo.GetOne(id);
            noti.IsViewed = true;
            return await MapNotificationToNotificationDto(noti);
        }
    }
}
