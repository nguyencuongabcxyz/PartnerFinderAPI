using Data.Models;
using Microsoft.AspNetCore.Identity;
using Service.Constants;
using Service.Models;
using System.Threading.Tasks;

namespace Service.Services
{
    public interface IAuthService
    {
        Task<object> RegisterUserAsUserRole(ApplicationUser user, string password);
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

        public async Task<object> RegisterUserAsUserRole(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return result;
            }
            await _userManager.AddToRoleAsync(user, AuthenticationConstant.UserRole);
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
