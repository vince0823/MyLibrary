using MyLibrary.Business.Dtos.V1.AccountDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Business.Repositories.Contracts
{
    public interface IAccountRepository
    {
        //Task<RegisterResultDto> RegisterAdminAsync(RegisterAdminDto registerDto);
        //Task<RegisterResultDto> RegisterNormalAsync(RegisterNormalDto registerDto);
        Task<TokenResultDto> LoginAsync(LoginDto loginDto, string ipAddress);
        //Task RevokeToken(RevokeDto revokeDto, string ipAddress);
        //Task<TokenResultDto> RefreshToken(RefreshDto refreshDto, string ipAddress);
        //CurrentUserDto GetCurrentUser();
        //Task<IEnumerable<GetUserDto>> GetUsersAsync();
    }
}
