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
    public interface IUserInformationService
    {
        Task<bool> CheckInitializedInfo(string id);
        Task<bool> CheckIfUserHaveSpecification(Expression<Func<UserInformation, bool>> specification);
        Task<IEnumerable<UserInformation>> GetManyWithCondition(Expression<Func<UserInformation, bool>> condition);
        Task UpdateLevel(string id, UserLevel userLevel);
        Task AddWithEmptyInfo(string id, string name);
        Task<int> GetPercentageOfCompletedInfo(string id);
        Task<UserInfoDto> GetOne(string id);
        Task<UserInfoDto> Update(string id, UserInfoDto userInfoDto);
        Task<UserInfoDto> UpdateMediaProfile(string id, MediaProfileDto mediaProfile);
        Task<IEnumerable<PartnerFinderDto>> GetPartnerFinders(string userId, string location, UserLevel level, int index, int size);
        Task<int> CountPartnerFinders(string userId, string location, UserLevel level);
        Task<IEnumerable<AdminUserDto>> GetAdminUsers(string userId, int index, int size);
        Task<IEnumerable<AdminUserDto>> SearchAdminUsers(string userId, string pattern);
        Task<AdminUserDto> ActiveUser(string userId);
        Task<AdminUserDto> BlockUser(string userId);
        AdminUserDto MapUserToAdminUser(UserInformation user);
        Task<int> CountAdminUsers(string userId);
    }
    public class UserInformationService : IUserInformationService
    {
        private readonly IUserInformationRepository _userInformationRepo;
        private readonly IPartnerRequestRepository _partnerRequestRepo;
        private readonly IPartnershipRepository _partnershipRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserInformationService(IUserInformationRepository userInformationRepo, 
            IMapper mapper, 
            IPartnerRequestRepository partnerRequestRepo, 
            IPartnershipRepository partnershipRepo,
            IUnitOfWork unitOfWork)
        {
            _userInformationRepo = userInformationRepo;
            _partnerRequestRepo = partnerRequestRepo;
            _partnershipRepo = partnershipRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task AddWithEmptyInfo(string id, string name)
        {
            var userInfo = new UserInformation()
            {
                UserId = id,
                Name = name,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            await _userInformationRepo.Add(userInfo);
        }

        public async Task<bool> CheckIfUserHaveSpecification(Expression<Func<UserInformation, bool>> specification)
        {
            var userInfo = await _userInformationRepo.GetOneByCondition(specification);
            return userInfo != null;
        }

        public async Task<bool> CheckInitializedInfo(string id)
        {
            var retrievedUserInfo = await _userInformationRepo.GetOne(id);
            return retrievedUserInfo != null;
        }

        public async Task<IEnumerable<UserInformation>> GetManyWithCondition(Expression<Func<UserInformation, bool>> condition)
        {
            return condition != null ? await _userInformationRepo.GetManyByCondition(condition) : await _userInformationRepo.GetAll();
        }

        public async Task<int> GetPercentageOfCompletedInfo(string id)
        { 
            var retrievedUserInfo = await _userInformationRepo.GetOne(id);
            if (retrievedUserInfo == null) return 0;
            var properties = typeof(UserInformation).GetProperties();
            //Subtract 1 for ApplicationUser property using for mapping model to DB layer
            var totalProps = properties.Length - 2;
            var setValueProps = properties.Select(p => typeof(UserInformation).GetProperty(p.Name)?.GetValue(retrievedUserInfo))
                                          .Count(propValue => propValue != null && propValue != "");

            return (int)((setValueProps/(double)totalProps)*100);
        }

        public async Task UpdateLevel(string id, UserLevel userLevel)
        {
            var userInfo = await _userInformationRepo.GetOne(id);
            userInfo.Level = userLevel;
        }

        public async Task<UserInfoDto> GetOne(string id)
        {
            var userInfoModel = await _userInformationRepo.GetOne(id);
            return _mapper.Map<UserInfoDto>(userInfoModel);
        }

        public async Task<UserInfoDto> Update(string id, UserInfoDto userInfoDto)
        {
            var userInfoModel = await _userInformationRepo.GetOne(id);
            _mapper.Map(userInfoDto, userInfoModel);
            userInfoModel.UpdatedDate = DateTime.Now;
            return _mapper.Map<UserInfoDto>(userInfoModel);
        }


        public async Task<UserInfoDto> UpdateMediaProfile(string id, MediaProfileDto mediaProfile)
        {
            var userInfoModel = await _userInformationRepo.GetOne(id);
            var mediaProps = typeof(MediaProfileDto).GetProperties();
            foreach (var prop in mediaProps)
            {
                var mediaPropValue = typeof(MediaProfileDto).GetProperty(prop.Name)?.GetValue(mediaProfile);
                if (mediaPropValue != null)
                {
                    typeof(UserInformation).GetProperty(prop.Name)?.SetValue(userInfoModel, mediaPropValue);
                }
            }

            return _mapper.Map<UserInfoDto>(userInfoModel);
        }

        private async Task<IEnumerable<PartnerFinderDto>> MapUserInfosToPartnerFinders(IEnumerable<UserInformation> users, string userId)
        {
            var partnerFinders = new List<PartnerFinderDto>();
            foreach(var user in users)
            {
                var isPendingRequest = await _partnerRequestRepo.CheckExistence(p => p.SenderId == userId && p.ReceiverId == user.UserId);
                var isInPartnership = await _partnershipRepo.CheckExistence(p => p.OwnerId == userId && p.PartnerId == user.UserId);
                var partnerFinder = _mapper.Map<PartnerFinderDto>(user);
                if(!isPendingRequest && !isInPartnership)
                {
                    partnerFinder.Status = PartnerFinderStatus.NotInPartnership;
                }else if(isPendingRequest)
                {
                    partnerFinder.Status = PartnerFinderStatus.PendingRequest;
                }
                else
                {
                    partnerFinder.Status = PartnerFinderStatus.InPartnership;
                }
                partnerFinders.Add(partnerFinder);
            }
            return partnerFinders;
        }



        public async Task<IEnumerable<PartnerFinderDto>> GetPartnerFinders(string userId, string location, UserLevel level, int index, int size)
        {
            Expression<Func<UserInformation, bool>> condition;
            if (location == null && level == UserLevel.Undefined)
            {
                condition = u => u.UserId != userId;
            }else if (location == null && level != UserLevel.Undefined)
            {
                condition = u => u.UserId != userId && u.Level == level;
            }else if (location != null && level == UserLevel.Undefined)
            {
                condition = u => u.UserId != userId && u.Location == location;
            }
            else
            {
                condition = u => u.UserId != userId && u.Location == location && u.Level == level;
            }
            var users = await _userInformationRepo.GetManyByCondition(condition);
            var userList = new List<UserInformation>();
            foreach(var user in users)
            {
                var isPendingRequest = await _partnerRequestRepo.CheckExistence(p => p.SenderId == userId && p.ReceiverId == user.UserId);
                var isInPartnership = await _partnershipRepo.CheckExistence(p => p.OwnerId == userId && p.PartnerId == user.UserId);
                if(!isPendingRequest && !isInPartnership)
                {
                    userList.Add(user);
                }
            }
            var returnUsers =  userList.OrderByDescending(u => u.UpdatedDate).Skip(index * size).Take(size);
            return await MapUserInfosToPartnerFinders(returnUsers, userId);
        }



        public async Task<int> CountPartnerFinders(string userId, string location, UserLevel level)
        {
            Expression<Func<UserInformation, bool>> condition;
            if (location == null && level == UserLevel.Undefined)
            {
                condition = u => u.UserId != userId;
            }
            else if (location == null && level != UserLevel.Undefined)
            {
                condition = u => u.UserId != userId && u.Level == level;
            }
            else if (location != null && level == UserLevel.Undefined)
            {
                condition = u => u.UserId != userId && u.Location == location;
            }
            else
            {
                condition = u => u.UserId != userId && u.Location == location && u.Level == level;
            }
            var users = await _userInformationRepo.GetManyByCondition(condition);
            var userList = new List<UserInformation>();
            foreach (var user in users)
            {
                var isPendingRequest = await _partnerRequestRepo.CheckExistence(p => p.SenderId == userId && p.ReceiverId == user.UserId);
                var isInPartnership = await _partnershipRepo.CheckExistence(p => p.OwnerId == userId && p.PartnerId == user.UserId);
                if (!isPendingRequest && !isInPartnership)
                {
                    userList.Add(user);
                }
            }
            return userList.Count;
        }

        public AdminUserDto MapUserToAdminUser(UserInformation user)
        {
            return _mapper.Map<AdminUserDto>(user);
        }

        public async Task<IEnumerable<AdminUserDto>> GetAdminUsers(string userId, int index, int size)
        {
            var users = await _userInformationRepo.OrderAndGetRange(index, size, OrderType.OrderByDescending, u => u.UpdatedDate, u => u.UserId != userId);
            var adminUsers = new List<AdminUserDto>();
            foreach(var user in users)
            {
                var adminUser = MapUserToAdminUser(user);
                adminUsers.Add(adminUser);
            }
            return adminUsers;
        }

        public async Task<IEnumerable<AdminUserDto>> SearchAdminUsers(string userId, string pattern)
        {
            var users = await _userInformationRepo.GetManyByCondition(u => u.Name.CaseInsensitiveContains(pattern) && u.UserId != userId);
            var adminUsers = new List<AdminUserDto>();
            foreach (var user in users)
            {
                var adminUser = MapUserToAdminUser(user);
                adminUsers.Add(adminUser);
            }
            return adminUsers;
        }

        public async Task<AdminUserDto> ActiveUser(string userId)
        {
            var user = await _userInformationRepo.GetOne(userId);
            user.IsBlocked = false;
            await _unitOfWork.Commit();
            return MapUserToAdminUser(user);
        }

        public async Task<AdminUserDto> BlockUser(string userId)
        {
            var user = await _userInformationRepo.GetOne(userId);
            user.IsBlocked = true;
            await _unitOfWork.Commit();
            return MapUserToAdminUser(user);
        }

        public async Task<int> CountAdminUsers(string userId)
        {
            var users = await _userInformationRepo.GetManyByCondition(u => u.UserId != userId);
            return users.Count();
        }
    }
}
