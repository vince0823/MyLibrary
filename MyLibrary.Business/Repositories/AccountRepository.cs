using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MyLibrary.Business.Dtos.V1.AccountDtos;
using MyLibrary.Business.Errors;
using MyLibrary.Business.Repositories.Contracts;
using MyLibrary.Models;
using MyLibrary.Services.JwtToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Business.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _token;
        //private readonly IUserAccessorService _userAccessor;
        private readonly UserManager<User> _userManager;
        public AccountRepository(IMapper mapper, UserManager<User> userManager, IJwtTokenService token /*IUserAccessorService userAccessor*/)
        {
            _mapper = mapper;
            _userManager = userManager;
            _token = token;
            //_userAccessor = userAccessor;
        }

        public async Task<TokenResultDto> LoginAsync(LoginDto loginDto, string ipAddress)
        {
            var dbUser = await _userManager.FindByNameAsync(loginDto.Email);
            if (dbUser == null)
                throw new RestException(HttpStatusCode.Unauthorized, "Username or password is incorrect.");

            ////if(!dbUser.EmailConfirmed)
            ////    throw new RestException(HttpStatusCode.Forbidden, $"Email is not confirmed for {loginDto.Email }.");

            var result = await _userManager.CheckPasswordAsync(dbUser, loginDto.Password);
            if (!result)
                throw new RestException(HttpStatusCode.Unauthorized, "Username or password is incorrect.");

            var accessToken = await _token.GenerateAccessToken(dbUser);
            var refreshToken = await _token.GenerateRefreshToken(dbUser.Id, ipAddress);

            return new TokenResultDto { AccessToken = accessToken, RefreshToken = refreshToken.Token };
        }
    }
}
