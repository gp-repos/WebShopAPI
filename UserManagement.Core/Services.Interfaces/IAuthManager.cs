using System.Threading.Tasks;
using UserManagement.Core.Domain.Entities;
using UserManagement.Core.Models;

namespace UserManagement.Core.Services.Interfaces
{
    public interface IAuthManager
    {
        Task<AppUser> GetUser(string email);

        Task<bool> CheckUserAccess(AppUser appUser, string password);

        Task<string> CreateToken(AppUser user);

        Task<string> CreateRefreshToken(AppUser user);

        Task<TokenRequest> VerifyRefreshToken(TokenRequest request);

    }
}
