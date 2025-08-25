using Microsoft.AspNetCore.Mvc;
using SBapi.Common.Dto;
using SBapi.Service.Repository.Interface;

namespace SimpleBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenRepository _tokenRepository;

        public TokenController(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        [HttpPost("GetToken")]
        public async Task<IActionResult> GetToken(UserRequest userRequest)
        {
            var token = await _tokenRepository.GetTokenAsync(userRequest);
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Invalid credentials");
            }
            return Ok(token);
        }
    }
}
