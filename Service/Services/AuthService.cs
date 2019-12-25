using Data.Models;
using Microsoft.AspNetCore.Identity;
using Service.Constants;
using System.Threading.Tasks;
using AutoMapper;
using Service.Models;

namespace Service.Services
{
    public interface IAuthService
    {
        Task<object> RegisterUserAsUserRole(UserDto userDto);
        Task<AuthenticateUserResult> AuthenticateUser(ApplicationUser user, string password, bool isBlocked);
        Task<object> AddRole(string roleName);
    }
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<object> RegisterUserAsUserRole(UserDto userDto)
        {
            var user = _mapper.Map<ApplicationUser>(userDto);
            var result = await _userManager.CreateAsync(user, userDto.Password);
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

        public async Task<AuthenticateUserResult> AuthenticateUser(ApplicationUser user, string password, bool isBlocked)
        {
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                return AuthenticateUserResult.Invalid;
            }

            if (isBlocked)
            {
                return AuthenticateUserResult.Blocked;
            }

            return AuthenticateUserResult.Succeeded;
        }


    }
}
