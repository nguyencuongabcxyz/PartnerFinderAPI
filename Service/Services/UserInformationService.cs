using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Data.Repositories;

namespace Service.Services
{
    public interface IUserInformationService
    {
        Task<bool> CheckExistence(string id);
        Task<bool> CheckIfUserHaveSpecification(Expression<Func<UserInformation, bool>> specification);
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

        public async Task<bool> CheckExistence(string id)
        {
            var retrievedUser = await _userRepo.GetOne(id);
            return retrievedUser != null;
        }

        public async Task<bool> CheckIfUserHaveSpecification(Expression<Func<UserInformation, bool>> specification)
        {
            var userInfo = await _userInformationRepo.GetOneByCondition(specification);
            return userInfo != null;
        }
    }
}
