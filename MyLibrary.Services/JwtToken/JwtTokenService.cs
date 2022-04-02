using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyLibrary.Core;
using MyLibrary.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Services.JwtToken
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IMyRestaurantContext _context;

        public JwtTokenService(UserManager<User> userManager, IMyRestaurantContext context, IOptions<JwtSettings> jwtSetting)
        {
            _userManager = userManager;
            _context = context;
            _jwtSettings = jwtSetting.Value;
        }

        public bool ValidateRefreshToken(string token)
        {
            var tokenValidationParamters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,

                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.RefreshTokenSecret)),

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, tokenValidationParamters, out SecurityToken validateToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static string GenerateToken(string secretKey, string issuer, string audience, double expirationMinutes, IEnumerable<Claim>? claims = null)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        public async Task<RefreshToken> GenerateRefreshToken(Guid userId, string ipAddress)
        {
            var token = GenerateToken(_jwtSettings.RefreshTokenSecret,
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                _jwtSettings.RefreshTokenExpirationInMinutes,
                claims: null);

            var refreshToken = new RefreshToken
            {
                Token = token,
                UserId = userId,
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshTokenExpirationInMinutes),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };

            _context.Create(refreshToken);
            await _context.CommitAsync();
            return refreshToken;
        }

        public async Task<string> GenerateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("id", user.Id.ToString()),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName),
            };

            var userClaims = await _userManager.GetClaimsAsync(user);
            foreach (var claim in userClaims)
                claims.Add(claim);

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            return GenerateToken(_jwtSettings.AccessTokenSecret,
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                _jwtSettings.AccessTokenExpirationInMinutes,
                claims);
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(Expression<Func<RefreshToken, bool>> expression) => await _context.GetFirstOrDefaultAsync(expression);

        public async Task UpdateRefreshTokenAsync(RefreshToken token)
        {
            _context.Modify(token);
            await _context.CommitAsync();
        }
    }
}
