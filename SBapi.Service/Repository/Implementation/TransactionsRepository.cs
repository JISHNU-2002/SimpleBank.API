using Microsoft.EntityFrameworkCore;
using SBapi.Common.Dto;
using SBapi.Common.ErrorDto;
using SBapi.Entity.Data;
using SBapi.Entity.Models;
using SBapi.Service.Repository.Interface;

namespace SBapi.Service.Repository.Implementation
{
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly AppDbContext _context;

        public TransactionsRepository(AppDbContext context,
            IAccountRepository accountRepository)
        {
            _context = context;
        }

        public async Task<Result<MoneyTransferDto>> TransferAsync(MoneyTransferDto transferDto)
        {
            Result<MoneyTransferDto> result = new Result<MoneyTransferDto>();
            try
            {
                if(transferDto == null)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "400",
                        ErrorMessage = "Transfer data cannot be null."
                    });
                    return result;
                }

                if(transferDto.Amount <= 0)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "400",
                        ErrorMessage = "Transfer amount must be greater than zero."
                    });
                    return result;
                }

                var fromAccount = await _context.AccountSet
                    .FirstOrDefaultAsync(a => a.AccountNumber == transferDto.FromAccountNumber);
                var toAccount = await _context.AccountSet
                    .FirstOrDefaultAsync(a => a.AccountNumber == transferDto.ToAccountNumber);

                if(fromAccount == null || toAccount == null)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "404",
                        ErrorMessage = "One or both accounts not found."
                    });
                    return result;
                }

                if(fromAccount.Balance < transferDto.Amount)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "InsufficientFunds",
                        ErrorMessage = "Insufficient funds in the source account."
                    });
                    return result;
                }

                var minBalance = await (
                    from user in _context.Users
                    where user.AccountNumber == transferDto.FromAccountNumber
                    join form in _context.ApplicationFormSet
                        on user.FormId equals form.FormId
                    join accType in _context.AccountTypeSet
                        on form.AccountTypeId equals accType.TypeId
                    select accType.MinBalance
                ).FirstOrDefaultAsync();

                if((fromAccount.Balance - transferDto.Amount) < minBalance)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "Minimum balance",
                        ErrorMessage = "Minimum balance required"
                    });
                    return result;
                }

                fromAccount.Balance -= transferDto.Amount;
                toAccount.Balance += transferDto.Amount;

                var transactionRecord = new Transactions
                {
                    FromAccountNumber = transferDto.FromAccountNumber,
                    ToAccountNumber = transferDto.ToAccountNumber,
                    Amount = transferDto.Amount,
                    TransactionDate = DateTime.UtcNow,
                    TransactionType = transferDto.TransactionType
                };
                await _context.TransactionsSet.AddAsync(transactionRecord);
                await _context.SaveChangesAsync();

                result.Response = transferDto;
            }
            catch (Exception ex)
            {
                result.Errors.Add(new Errors
                {
                    ErrorCode = "DB500",
                    ErrorMessage = ex.Message
                });
            }
            return result;
        }

        public async Task<Result<MoneyTransferDto>> DepositAsync(MoneyTransferDto depositDto)
        {
            Result<MoneyTransferDto> result = new Result<MoneyTransferDto>();
            try
            {
                if (depositDto == null)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "400",
                        ErrorMessage = "Deposit data cannot be null."
                    });
                    return result;
                }

                if (depositDto.Amount <= 0)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "400",
                        ErrorMessage = "Deposit amount must be greater than zero."
                    });
                    return result;
                }

                var toAccount = await _context.AccountSet
                    .FirstOrDefaultAsync(a => a.AccountNumber == depositDto.ToAccountNumber);

                if (toAccount == null)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "404",
                        ErrorMessage = "Account not found."
                    });
                    return result;
                }

                toAccount.Balance += depositDto.Amount;

                var transactionRecord = new Transactions
                {
                    FromAccountNumber = depositDto.FromAccountNumber,
                    ToAccountNumber = depositDto.ToAccountNumber,
                    Amount = depositDto.Amount,
                    TransactionDate = DateTime.UtcNow,
                    TransactionType = depositDto.TransactionType
                };
                await _context.TransactionsSet.AddAsync(transactionRecord);
                await _context.SaveChangesAsync();

                result.Response = depositDto;
            }
            catch (Exception ex)
            {
                result.Errors.Add(new Errors
                {
                    ErrorCode = "DB500",
                    ErrorMessage = ex.Message
                });
            }
            return result;
        }

        public async Task<Result<MoneyTransferDto>> WithdrawAsync(MoneyTransferDto withdrawDto)
        {
            Result<MoneyTransferDto> result = new Result<MoneyTransferDto>();
            try
            {
                if (withdrawDto == null)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "400",
                        ErrorMessage = "Withdraw data cannot be null."
                    });
                    return result;
                }

                if (withdrawDto.Amount <= 0)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "400",
                        ErrorMessage = "Withdraw amount must be greater than zero."
                    });
                    return result;
                }

                var fromAccount = await _context.AccountSet
                    .FirstOrDefaultAsync(a => a.AccountNumber == withdrawDto.FromAccountNumber);

                if (fromAccount == null)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "404",
                        ErrorMessage = "Account not found."
                    });
                    return result;
                }

                if (fromAccount.Balance < withdrawDto.Amount)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "InsufficientFunds",
                        ErrorMessage = "Insufficient funds in the source account."
                    });
                    return result;
                }

                var minBalance = await (
                    from user in _context.Users
                    where user.AccountNumber == withdrawDto.FromAccountNumber
                    join form in _context.ApplicationFormSet
                        on user.FormId equals form.FormId
                    join accType in _context.AccountTypeSet
                        on form.AccountTypeId equals accType.TypeId
                    select accType.MinBalance
                ).FirstOrDefaultAsync();

                if ((fromAccount.Balance - withdrawDto.Amount) < minBalance)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "Minimum balance",
                        ErrorMessage = "Minimum balance required"
                    });
                    return result;
                }

                fromAccount.Balance -= withdrawDto.Amount;

                var transactionRecord = new Transactions
                {
                    FromAccountNumber = withdrawDto.FromAccountNumber,
                    ToAccountNumber = withdrawDto.ToAccountNumber,
                    Amount = withdrawDto.Amount,
                    TransactionDate = DateTime.UtcNow,
                    TransactionType = withdrawDto.TransactionType
                };
                await _context.TransactionsSet.AddAsync(transactionRecord);
                await _context.SaveChangesAsync();

                result.Response = withdrawDto;
            }
            catch (Exception ex)
            {
                result.Errors.Add(new Errors
                {
                    ErrorCode = "DB500",
                    ErrorMessage = ex.Message
                });
            }
            return result;
        }

        public async Task<Result<List<TransactionDetailsDto>>> GetAllTransactionsDetailsAsync()
        {
            Result<List<TransactionDetailsDto>> result = new();
            try
            {
                List<TransactionDetailsDto> transactions = await (
                    from t in _context.TransactionsSet

                    join fromAcc in _context.AccountSet on t.FromAccountNumber equals fromAcc.AccountNumber
                    join fromUser in _context.Users on fromAcc.AccountNumber equals fromUser.AccountNumber
                    join fromForm in _context.ApplicationFormSet on fromUser.FormId equals fromForm.FormId

                    join toAcc in _context.AccountSet on t.ToAccountNumber equals toAcc.AccountNumber
                    join toUser in _context.Users on toAcc.AccountNumber equals toUser.AccountNumber
                    join toForm in _context.ApplicationFormSet on toUser.FormId equals toForm.FormId

                    select new TransactionDetailsDto
                    {
                        FullName = fromForm.FullName,
                        FromAccount = fromAcc.AccountNumber,
                        FromIFSC = fromForm.IFSC,
                        ToAccount = toAcc.AccountNumber,
                        ToIFSC = toForm.IFSC,
                        TransactionType = t.TransactionType!,
                        Amount = t.Amount,
                        TransactionDate = t.TransactionDate
                    }).ToListAsync();

                result.Response = transactions;
            }
            catch (Exception ex)
            {
                result.Errors.Add(new Errors
                {
                    ErrorCode = "SERVER_ERROR",
                    ErrorMessage = ex.Message
                });
            }
            return result;
        }
    }
}
