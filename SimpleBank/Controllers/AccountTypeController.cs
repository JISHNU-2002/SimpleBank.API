using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SBapi.Entity.Models;
using SBapi.Service.Repository.Interface;

namespace SBapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]

    public class AccountTypeController : ControllerBase
    {
        private readonly IAccountTypeRepository _accountTypeRepository;

        public AccountTypeController(IAccountTypeRepository accountTypeRepository)
        {
            _accountTypeRepository = accountTypeRepository;
        }

        [HttpGet("GetAllAccountTypes")]
        public async Task<IActionResult> GetAllAccountTypes()
        {
            return Ok(await _accountTypeRepository.GetAllAccountTypes());
        }

        [HttpGet("GetAccountTypeById/{typeId}")]
        public async Task<IActionResult> GetAccountTypeById(int typeId)
        {
            return Ok(await _accountTypeRepository.GetAccountTypeById(typeId));
        }

        [HttpPost("AddAccountType")]
        public async Task<IActionResult> AddAccountType(AccountType accountType)
        {
            return Ok(await _accountTypeRepository.AddAccountType(accountType));
        }

        [HttpPut("UpdateAccountType")]
        public async Task<IActionResult> UpdateAccountType(AccountType accountType)
        {
            return Ok(await _accountTypeRepository.UpdateAccountType(accountType));
        }

        [HttpDelete("DeleteAccountTypeById/{typeId}")]
        public async Task<IActionResult> DeleteAccountTypeById(int typeId)
        {
            return Ok(await _accountTypeRepository.DeleteAccountType(typeId));
        }
    }
}
