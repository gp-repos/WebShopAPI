using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Core.Domain.Entities;
using UserManagement.Core.Models;
using UserManagement.Core.Services.Interfaces;

namespace WebShop.Infrastructure.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthManager(UserManager<AppUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async  Task<AppUser> GetUser(string email)
        {
            return await _userManager.FindByNameAsync(email);
        }

        public async Task<bool> CheckUserAccess(AppUser appUser, string password)
        {
            var validPassword = await _userManager.CheckPasswordAsync(appUser, password);
            return (appUser != null && validPassword);
        }

        public async Task<string> CreateToken(AppUser user)
        {
            var key = _configuration.GetSection("AuthSecureKey").Value;
            if (string.IsNullOrEmpty(key))
                throw new Exception("JWT AuthSecureKey not specified");

            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtSettings = _configuration.GetSection("JWT");

            var expiration = DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("lifetime").Value));

            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("Issuer").Value,
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
            );
   
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> CreateRefreshToken(AppUser user)
        {
            await _userManager.RemoveAuthenticationTokenAsync(user, "WebShopApi", "RefreshToken");
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(user, "WebShopApi", "RefreshToken");
            var result = await _userManager.SetAuthenticationTokenAsync(user, "WebShopApi", "RefreshToken", newRefreshToken);
            return newRefreshToken;
        }

        public async Task<TokenRequest> VerifyRefreshToken(TokenRequest request)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
            var username = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == ClaimTypes.Name)?.Value;
            AppUser user = await _userManager.FindByNameAsync(username);
            try
            {
                var isValid = await _userManager.VerifyUserTokenAsync(user, "WebShopApi", "RefreshToken", request.RefreshToken);
                if (isValid)
                {
                    return new TokenRequest { Token = await CreateToken(user), RefreshToken = await CreateRefreshToken(user) } ;
                }
                await _userManager.UpdateSecurityStampAsync(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }
    }
}
