using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service;
using Service.Constants;
using Service.Models;
using Service.Services;

namespace PartnerFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        private readonly IServiceFactory _serviceFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationSetting _appSetting;

        public AuthController(
            IAuthService authService, 
            ITokenService tokenService, 
            UserManager<ApplicationUser> userManager, 
            IOptions<ApplicationSetting> appSetting,
            IServiceFactory serviceFactory)
        {
            _authService = authService;
            _tokenService = tokenService;
            _userManager = userManager;
            _appSetting = appSetting.Value;
            _serviceFactory = serviceFactory;
        }

        [HttpPost("CreateRole/{role}")]
        public async Task<object> CreateRole(string role)
        {
            var result = await _authService.AddRole(role);
            return Ok(result);
        }

        [HttpPost("Register")]
        public async Task<object> RegisterUser(UserDto userDto)
        {
            var user = new ApplicationUser();
            var result = await _authService.RegisterUserAsUserRole(userDto);

            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginInfoDto loginInfoDto)
        {
            var user = await _userManager.FindByNameAsync(loginInfoDto.UserName);
            var result = await _authService.AuthenticateUser(user, loginInfoDto.Password);
            var temp = AuthenticateUserResult.Succeeded;
            switch (result)
            {
                case AuthenticateUserResult.Invalid:
                    return BadRequest(new { message = "Username or password is incorrect!" });
                case AuthenticateUserResult.Blocked:
                    return Forbid();
                case AuthenticateUserResult.Succeeded:
                    var token = await _tokenService.GenerateToken(user, _appSetting.JwtSecret);
                    return Ok(new { token, user.Id });
                default:
                    return NotFound();
            }
        }

    }
}