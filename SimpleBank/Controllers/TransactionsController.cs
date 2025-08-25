using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SBapi.Common.Dto;
using SBapi.Entity.Models;
using SBapi.Service.Repository.Implementation;
using SBapi.Service.Repository.Interface;

namespace SBapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]

    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionsRepository _transactionsRepository;

        public TransactionsController(ITransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
        }

        [HttpPost("Transfer")]
        public async Task<IActionResult> Transfer(MoneyTransferDto transferDto)
        {
            return Ok(await _transactionsRepository.TransferAsync(transferDto));
        }

        [HttpPost("Deposit")]
        public async Task<IActionResult> Deposit(MoneyTransferDto depositDto)
        {
            return Ok(await _transactionsRepository.DepositAsync(depositDto));
        }

        [HttpPost("Withdraw")]
        public async Task<IActionResult> Withdraw(MoneyTransferDto withdrawDto)
        {
            return Ok(await _transactionsRepository.WithdrawAsync(withdrawDto));
        }

        [HttpGet("GetAllTransactionsDetails")]
        public async Task<IActionResult> GetAllTransactionsDetails()
        {
            return Ok(await _transactionsRepository.GetAllTransactionsDetailsAsync());
        }
    }
}
