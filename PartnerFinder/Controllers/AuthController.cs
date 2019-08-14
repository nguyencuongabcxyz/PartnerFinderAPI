using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationSetting _appSetting;
        private readonly IMapper _mapper;

        public AuthController(
            IAuthService authService, 
            ITokenService tokenService, 
            UserManager<ApplicationUser> userManager, 
            IOptions<ApplicationSetting> appSetting,
            IMapper mapper)
        {
            _authService = authService;
            _tokenService = tokenService;
            _userManager = userManager;
            _appSetting = appSetting.Value;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("sádafwrew");
        }

        [HttpPost("CreateRole/{role}")]
        public async Task<object> CreateRole(string role)
        {
            var result = await _authService.AddRole(role);
            return Ok(result);
        }

        [HttpPost("Register")]
        public async Task<object> RegisterUser(UserDTO userDto)
        {
            var user = _mapper.Map<ApplicationUser>(userDto);
            var result = await _authService.RegisterUserAsUserRole(user, userDto.Password);
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginInfoDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            var result = await _authService.AuthenticateUser(user, model.Password);
            switch (result)
            {
                case AuthenticateUserResult.Invalid:
                    return BadRequest(new { message = "Username or password is incorrect!" });
                case AuthenticateUserResult.Blocked:
                    return Forbid();
                default:
                {
                    var token = await _tokenService.GenerateToken(user, _appSetting.JwtSecret);
                    return Ok(new { token, user.Id });
                }
            }
        }

    }
}