using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;
using Service.Models;
using System.IdentityModel.Tokens.Jwt;

namespace Service.Services
{
    public interface ITokenService
    {
        Task<string> GenerateToken(ApplicationUser user, string secretKey);
    }
    public class TokenService : ITokenService
    {
        private UserManager<ApplicationUser> _userManager;

        public TokenService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<string> GenerateToken(ApplicationUser user, string secretKey)
        {
            var role = await _userManager.GetRolesAsync(user);
            IdentityOptions options = new IdentityOptions();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("name", user.UserName),
                    new Claim("userId", user.Id),
                    new Claim(options.ClaimsIdentity.RoleClaimType, role.FirstOrDefault()),
                }),
                Expires = new DateTime(0,0,0,0,100,0),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return token;
        }
    }
}
