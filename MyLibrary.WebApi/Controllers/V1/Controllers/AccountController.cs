using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyLibrary.Business.Dtos.V1.AccountDtos;
using MyLibrary.Business.Repositories.Contracts;
using System.Threading.Tasks;

namespace MyLibrary.WebApi.Controllers.V1.Controllers
{
    [ApiVersion("1.0")]
    public class AccountController : BaseApiController
    {
        private readonly IAccountRepository _repository;
        public AccountController(IAccountRepository repository)
        {
            _repository = repository;
        }
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesDefaultResponseType]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await _repository.LoginAsync(loginDto, IpAddress());
            return Ok(result);
        }
        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress!.MapToIPv4().ToString();
        }
    }
}
