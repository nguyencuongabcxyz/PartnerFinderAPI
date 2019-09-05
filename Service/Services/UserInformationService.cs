using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Data.Repositories;

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
    }
    public class UserInformationService : IUserInformationService
    {
        private readonly IUserInformationRepository _userInformationRepo;
        private readonly IUserRepository _userRepo;

        public UserInformationService(IRepositoryFactory repositoryFactory)
        {
            _userInformationRepo = repositoryFactory.CreateUserInformationRepo();
            _userRepo = repositoryFactory.CreateUserRepo();
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
            return await _userInformationRepo.GetManyByCondition(condition);
        }

        public async Task<int> GetPercentageOfCompletedInfo(string id)
        { 
            var retrievedUserInfo = await _userInformationRepo.GetOne(id);
            if (retrievedUserInfo == null) return 0;
            var properties = typeof(UserInformation).GetProperties();
            var totalProps = properties.Length;
            var setValueProps = properties.Select(p => typeof(UserInformation).GetProperty(p.Name).GetValue(retrievedUserInfo))
                                          .Count(propValue => propValue != null);

            return (int)(((double)setValueProps/(double)totalProps)*100);
        }

        public async Task UpdateLevel(string id, UserLevel userLevel)
        {
            var userInfo = await _userInformationRepo.GetOne(id);
            userInfo.Level = userLevel;
        }
    }
}
