using MyLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Services.JwtToken
{
    public interface IJwtTokenService
    {
        Task<string> GenerateAccessToken(User user);
        Task<RefreshToken> GenerateRefreshToken(Guid userId, string ipAddress);
        bool ValidateRefreshToken(string token);
        Task<RefreshToken?> GetRefreshTokenAsync(Expression<Func<RefreshToken, bool>> expression);
        Task UpdateRefreshTokenAsync(RefreshToken token);
    }
}
