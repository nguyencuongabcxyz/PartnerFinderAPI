using Data.Models;
using Microsoft.AspNetCore.Identity;
using Service.Constants;
using Service.Models;
using System.Threading.Tasks;

namespace Service.Services
{
    public interface IAuthService
    {
        Task<object> RegisterUserAsUserRole(UserDTO user);
        Task<AuthenticateUserResult> AuthenticateUser(ApplicationUser user, string password);
        Task<object> AddRole(string roleName);
    }
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
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

        public async Task<object> AddRole(string roleName)
        {
            var role = new IdentityRole(roleName);
            var result = await _roleManager.CreateAsync(role);
            return result;
        }

        public async Task<AuthenticateUserResult> AuthenticateUser(ApplicationUser user, string password)
        {
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                return AuthenticateUserResult.Invalid;
            }

            if (user.IsBlocked)
            {
                return AuthenticateUserResult.Blocked;
            }

            return AuthenticateUserResult.Succeeded;
        }


    }
}
