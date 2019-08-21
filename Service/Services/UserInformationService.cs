using System;
using System.Linq.Expressions;
using Data.Models;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Service.Services
{
    public interface IUserInformationService
    {
        bool CheckExistence(string id);
        bool CheckIfUserHaveSpecification(Expression<Func<UserInformation, bool>> specification);
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

        public bool CheckExistence(string id)
        {
            var retrievedUser = _userRepo.GetOne(id);
            return retrievedUser != null;
        }

        public bool CheckIfUserHaveSpecification(Expression<Func<UserInformation, bool>> specification)
        {
            var userInfo = _userInformationRepo.GetOneByCondition(specification);
            return userInfo != null;
        }
    }
}
