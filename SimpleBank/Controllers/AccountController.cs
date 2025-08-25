using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using SBapi.Common.Dto;
using SBapi.Common.ErrorDto;
using SBapi.Entity.Migrations;
using SBapi.Entity.Security;
using SBapi.Service.Repository.Interface;

namespace SimpleBank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]

    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost("GetDashboard/{accountNumber}")]
        public async Task<IActionResult> GetDashboard(string accountNumber)
        {
            return Ok(await _accountRepository.GetDashboardDataAsync(accountNumber));
        }

        [HttpGet("GetProfile/{formId}")]
        public async Task<IActionResult> GetProfile(int formId)
        {
            return Ok(await _accountRepository.GetProfileByFormId(formId));
        }

        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(ProfileDto profileDto)
        {
            return Ok(await _accountRepository.UpdateProfileByFormId(profileDto));
        }

        [HttpGet("GetAllUsersWithDetails")]
        public async Task<IActionResult> GetAllUsersWithDetails()
        {
            return Ok(await _accountRepository.GetAllUsersWithDetailsAsync());
        }

        [HttpGet("GetUserById/{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            return Ok(await _accountRepository.GetUserByIdAsync(userId));
        }

        [HttpPost("DeleteUserById/{userId}")]
        public async Task<IActionResult> DeleteUserById(string userId)
        {
            return Ok(await _accountRepository.DeleteUserByIdAsync(userId));
        }
    }
}
