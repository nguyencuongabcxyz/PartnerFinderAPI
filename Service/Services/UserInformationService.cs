using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Models;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Service.Services
{
    public interface IUserInformationService
    {
        Task<bool> CheckExistence(string id);
        Task<bool> CheckIfUserHaveSpecification(Expression<Func<UserInformation, bool>> specification);
    }
    public class UserInformationService : IUserInformationService
    {
        private readonly IBaseRepository<UserInformation> _userInformationRepo;
        private readonly IBaseRepository<ApplicationUser> _userRepo;

        public UserInformationService(DbContext dbContext)
        {
            _userInformationRepo = new UserInformationRepository(dbContext);
            _userRepo = new UserRepository(dbContext);
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
