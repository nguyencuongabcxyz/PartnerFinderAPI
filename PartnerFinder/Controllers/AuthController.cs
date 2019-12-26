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
        private readonly IUserInformationService _userInformationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationSetting _appSetting;

        public AuthController(
            IAuthService authService, 
            ITokenService tokenService, 
            UserManager<ApplicationUser> userManager, 
            IOptions<ApplicationSetting> appSetting,
            IUserInformationService userInformationService,
            IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _tokenService = tokenService;
            _userManager = userManager;
            _appSetting = appSetting.Value;
            _userInformationService = userInformationService;
            _unitOfWork = unitOfWork;
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
            var result = await _authService.RegisterUserAsUserRole(userDto);
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginInfoDto loginInfoDto)
        {
            var user = await _userManager.FindByNameAsync(loginInfoDto.UserName);
            var check = await _userInformationService.CheckInitializedInfo(user.Id);
            if (!check)
            {
                await _userInformationService.AddWithEmptyInfo(user.Id, "");
                await _unitOfWork.Commit();
            }
            var userInfo = await _userInformationService.GetOne(user.Id);
            var result = await _authService.AuthenticateUser(user, loginInfoDto.Password, userInfo.IsBlocked);
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