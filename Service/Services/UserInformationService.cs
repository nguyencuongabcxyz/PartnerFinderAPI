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
        Task<UserInfoDto> UpdateMediaProfile(string id, MediaProfileDto mediaProfile);
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
            //Subtract 1 for ApplicationUser property using for mapping model to DB layer
            var totalProps = properties.Length - 1;
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
    }
}
