using SBapi.Common.Dto;
using SBapi.Common.ErrorDto;
using SBapi.Entity.Models;

namespace SBapi.Service.Repository.Interface
{
    public interface ITransactionsRepository
    {
        Task<Result<MoneyTransferDto>> TransferAsync(MoneyTransferDto transferDto);
        Task<Result<MoneyTransferDto>> DepositAsync(MoneyTransferDto depositDto);
        Task<Result<MoneyTransferDto>> WithdrawAsync(MoneyTransferDto withdrawDto);
        Task<Result<List<TransactionDetailsDto>>> GetAllTransactionsDetailsAsync();

    }
}
