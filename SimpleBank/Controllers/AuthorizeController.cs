using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SBapi.Common.Dto;
using SBapi.Common.ErrorDto;
using SBapi.Entity.Security;
using SBapi.Service.Repository.Implementation;
using SBapi.Service.Repository.Interface;
using System.Security.Claims;

namespace SimpleBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AuthorizeController : ControllerBase
    {
        private readonly IAuthorizeRepository _authorizeRepository;
        private readonly UserManager<AppUser> _userManager;

        public AuthorizeController(IAuthorizeRepository authorizeRepository,
            UserManager<AppUser> userManager)
        {
            _authorizeRepository = authorizeRepository;
            _userManager = userManager;
        }

        [HttpPost("AuthorizeUser")]
        public async Task<IActionResult> AuthorizeUser(UserRequest userRequest)
        {
            return Ok(await _authorizeRepository.AuthorizeUser(userRequest));
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(RegisterRequestDto request)
        {
            return Ok(await _authorizeRepository.RegisterUser(request));
        }

        [HttpPost("ChangePassword")]
        public async Task<ActionResult<Result<IdentityResult>>> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return new Result<IdentityResult>()
                {
                    Errors = new List<Errors>()
                    {
                        new Errors()
                        {
                             ErrorCode="NotValid",
                             ErrorMessage="Cannot Change password"
                         }
                    }
                };
            }

            var user = await _authorizeRepository.GetUserByName(changePasswordDto.Username);
            if (user == null)
            {
                return new Result<IdentityResult>()
                {
                    Errors = new List<Errors>()
                    {
                        new Errors()
                        {
                            ErrorCode="NotValid",
                            ErrorMessage="User Not Found"
                        }
                    }
                };
            }
            
            return await _authorizeRepository.ChangePassword(user.Response, changePasswordDto.CurrentPassword, changePasswordDto.ConfirmPassword);
        }
    }
}
