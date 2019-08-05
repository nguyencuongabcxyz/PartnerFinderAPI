using Data.Models;
using Microsoft.AspNetCore.Identity;
using Service.Constants;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public interface IAuthService
    {
        Task<object> RegisterUserAsUserRole(UserDTO user);
        Task<AuthenticateUserResult> AuthenticateUser(ApplicationUser user, string password);
    }
    public class AuthService : IAuthService
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;

        public AuthService(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<object> RegisterUserAsUserRole(UserDTO user)
        {
            var applicationUser = new ApplicationUser()
            {
                UserName = user.UserName,
                Email = user.Email,
            };
            var result = await _userManager.CreateAsync(applicationUser, user.Password);
            if (!result.Succeeded)
            {
                return result;
            }
            await _userManager.AddToRoleAsync(applicationUser, AuthenticationConstant.UserRole);
            return result;
        }

        public async Task<AuthenticateUserResult> AuthenticateUser(ApplicationUser user, string password)
        {
            if (user == null || await _userManager.CheckPasswordAsync(user, password))
            {
                return AuthenticateUserResult.Invalid;
            }
            if (user.IsActive == false)
            {
                return AuthenticateUserResult.Blocked;
            }
            return AuthenticateUserResult.Succeeded;
        }


    }
}
