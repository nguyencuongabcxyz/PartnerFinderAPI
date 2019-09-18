using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Data.Models;
using Data.Repositories;
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
        Expression<Func<UserInformation, bool>> HandleFilterCondition(FilteringUserConditionDto filteringCondition);
    }
    public class UserInformationService : IUserInformationService
    {
        private readonly IUserInformationRepository _userInformationRepo;
        private readonly IMapper _mapper;

        public UserInformationService(IUserInformationRepository userInformationRepo, IMapper mapper)
        {
            _userInformationRepo = userInformationRepo;
            _mapper = mapper;
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
            var totalProps = properties.Length;
            var setValueProps = properties.Select(p => typeof(UserInformation).GetProperty(p.Name)?.GetValue(retrievedUserInfo))
                                          .Count(propValue => propValue != null);

            return (int)((setValueProps/(double)totalProps)*100);
        }

        public async Task UpdateLevel(string id, UserLevel userLevel)
        {
            var userInfo = await _userInformationRepo.GetOne(id);
            userInfo.Level = userLevel;
        }

        public Expression<Func<UserInformation, bool>> HandleFilterCondition(FilteringUserConditionDto filteringCondition)
        {
            if (filteringCondition.Level == UserLevel.Undefined && string.IsNullOrEmpty(filteringCondition.Location))
            {
                return null;
            }

            if (filteringCondition.Level == UserLevel.Undefined && !string.IsNullOrEmpty(filteringCondition.Location))
            {
                return (u) => u.Location == filteringCondition.Location;
            }

            if (filteringCondition.Level != UserLevel.Undefined && string.IsNullOrEmpty(filteringCondition.Location))
            {
                return (u) => u.Level == filteringCondition.Level;
            }
            return (u) => u.Level == filteringCondition.Level && u.Location == filteringCondition.Location;
        }

        public async Task<UserInfoDto> GetOne(string id)
        {
            var userInfoModel = await _userInformationRepo.GetOne(id);
            return _mapper.Map<UserInfoDto>(userInfoModel);
        }

        public async Task<UserInfoDto> Update(string id, UserInfoDto userInfoDto)
        {
            var userInfoModel = await _userInformationRepo.GetOne(id);
            var tempVideo = userInfoModel.Video;
            var tempAudio = userInfoModel.VoiceAudio;
            _mapper.Map<UserInfoDto, UserInformation>(userInfoDto, userInfoModel);
            userInfoModel.UpdatedDate = DateTime.Now;
            userInfoModel.Video = tempVideo;
            userInfoModel.VoiceAudio = tempAudio;
            return _mapper.Map<UserInfoDto>(userInfoModel);
        }
    }
}
