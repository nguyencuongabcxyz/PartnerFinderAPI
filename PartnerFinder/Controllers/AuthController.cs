﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PartnerFinder.CustomFilters;
using Service.Constants;
using Service.Models;
using Service.Services;

namespace PartnerFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ValidateModel]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationSetting _appSetting;

        public AuthController(IAuthService authService, ITokenService tokenService, UserManager<ApplicationUser> userManager, IOptions<ApplicationSetting> appSetting)
        {
            _authService = authService;
            _tokenService = tokenService;
            _userManager = userManager;
            _appSetting = appSetting.Value;
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
        public async Task<object> RegisterUser(UserDTO user)
        {
            var result = await _authService.RegisterUserAsUserRole(user);
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginInfoDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            var result = await _authService.AuthenticateUser(user, model.Password);
            if(result == AuthenticateUserResult.Invalid)
            {
                return BadRequest(new { message = "Username or password is incorrect!" });
            }
            if(result == AuthenticateUserResult.Blocked)
            {
                return Forbid();
            }
            var token = await _tokenService.GenerateToken(user, _appSetting.Jwt_Secret);
            return Ok(new { token });
        }

    }
}