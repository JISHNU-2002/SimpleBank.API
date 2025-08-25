using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SBapi.Common.Dto;
using SBapi.Common.ErrorDto;
using SBapi.Entity.Data;
using SBapi.Entity.Models;
using SBapi.Entity.Security;
using SBapi.Service.Repository.Interface;

namespace SBapi.Service.Repository.Implementation
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Account>> CreateAccountAsync(decimal initialBalance)
        {
            Result<Account> result = new Result<Account>();
            try
            {
                var value = await _context.Set<SequenceValue>()
                    .FromSqlRaw("EXEC dbo.GetNextAccountNumberSequenceValue")
                    .AsNoTracking().ToListAsync();

                var accountNumber = value.First().Value;

                Account newAccount = new Account
                {
                    AccountNumber = accountNumber.ToString(),
                    Balance = initialBalance,
                };

                await _context.AccountSet.AddAsync(newAccount);
                await _context.SaveChangesAsync();

                result.Response = newAccount;
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

        public async Task<Result<DashboardDto>> GetDashboardDataAsync(string accountNumber)
        {
            Result<DashboardDto> result = new Result<DashboardDto>();

            try
            {
                var account = await _context.AccountSet.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
                var appUser = await _context.Users.FirstOrDefaultAsync(u => u.AccountNumber == accountNumber);

                if (account == null || appUser == null)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "404",
                        ErrorMessage = "User data not found."
                    });
                    return result;
                }

                var form = await _context.ApplicationFormSet.FirstOrDefaultAsync(f => f.FormId == appUser.FormId);
                if (form == null)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "404",
                        ErrorMessage = "User data not found."
                    });
                    return result;
                }

                var transactions = await (
                    from t in _context.TransactionsSet
                    join fromAcc in _context.AccountSet on t.FromAccountNumber equals fromAcc.AccountNumber
                    join fromUser in _context.Users on fromAcc.AccountNumber equals fromUser.AccountNumber
                    join fromForm in _context.ApplicationFormSet on fromUser.FormId equals fromForm.FormId

                    join toAcc in _context.AccountSet on t.ToAccountNumber equals toAcc.AccountNumber
                    join toUser in _context.Users on toAcc.AccountNumber equals toUser.AccountNumber
                    join toForm in _context.ApplicationFormSet on toUser.FormId equals toForm.FormId

                    where t.FromAccountNumber == accountNumber || t.ToAccountNumber == accountNumber
                    orderby t.TransactionDate descending

                    select new TransactionDto
                    {
                        TransactionId = t.TransactionId,
                        FromAccount = fromAcc.AccountNumber,
                        FromIFSC = fromForm.IFSC,
                        ToAccount = toAcc.AccountNumber,
                        ToIFSC = toForm.IFSC,
                        Amount = t.Amount,
                        TransactionType = t.TransactionType!,
                        TransactionDate = t.TransactionDate
                    }
                ).ToListAsync();

                result.Response = new DashboardDto
                {
                    FullName = form.FullName,
                    Email = form.Email,
                    PhoneNumber = form.PhoneNumber,
                    AccountNumber = account.AccountNumber,
                    Balance = account.Balance,
                    Transactions = transactions
                };
            }
            catch (Exception ex)
            {
                result.Errors.Add(new Errors
                {
                    ErrorCode = "500",
                    ErrorMessage = ex.Message
                });
            }

            return result;
        }

        public async Task<Result<ProfileDto>> GetProfileByFormId(int formId)
        {
            var result = new Result<ProfileDto>();

            try
            {
                var appUser = await _context.Users.FirstOrDefaultAsync(u => u.FormId == formId);
                var form = await _context.ApplicationFormSet.FirstOrDefaultAsync(f => f.FormId == appUser.FormId);
                var branch = await _context.BranchSet.FirstOrDefaultAsync(b => b.IFSC == form.IFSC);
                var account = await _context.AccountSet.FirstOrDefaultAsync(a => a.AccountNumber == appUser.AccountNumber);
                var accType = await _context.AccountTypeSet.FirstOrDefaultAsync(aT => aT.TypeId == form.AccountTypeId);

                if (appUser == null || form == null || branch == null || account == null || accType == null)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "404",
                        ErrorMessage = "User data not found."
                    });
                    return result;
                }

                var profile = new ProfileDto
                {
                    FormId = form.FormId,
                    FullName = form.FullName,
                    Email = form.Email,
                    PhoneNumber = form.PhoneNumber,
                    AadharNumber = form.AadharNumber,
                    PAN = form.PAN,
                    Address = form.Address,
                    DOB = form.DOB,
                    AccountTypeId = form.AccountTypeId,
                    AccountTypeName = accType.TypeName,
                    IFSC = form.IFSC,
                    DateOfRegistration = form.DateOfRegistration,
                    AccountNumber = appUser.AccountNumber,
                    BranchName = branch.BranchName,
                    Balance = account.Balance
                };

                result.Response = profile;
            }
            catch (Exception ex)
            {
                result.Errors.Add(new Errors
                {
                    ErrorCode = "500",
                    ErrorMessage = ex.Message
                });
            }
            return result;
        }

        public async Task<Result<ProfileDto>> UpdateProfileByFormId(ProfileDto profileDto)
        {            
            var result = new Result<ProfileDto>();
            
            try
            {           
                var form = await _context.ApplicationFormSet.FirstOrDefaultAsync(f => f.FormId == profileDto.FormId);
                if (form == null)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "404",
                        ErrorMessage = "Form not found"
                    });
                }
                else
                {
                    form.FullName = profileDto.FullName;
                    form.Email = profileDto.Email;
                    form.PhoneNumber = profileDto.PhoneNumber;
                    form.Address = profileDto.Address;

                    result.Response = profileDto;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add(new Errors
                {
                    ErrorCode = "500",
                    ErrorMessage = ex.Message
                });
            }
            return result;
        }

        public async Task<Result<List<UsersDto>>> GetAllUsersWithDetailsAsync()
        {
            Result<List<UsersDto>> result = new();
            try
            {
                List<UsersDto> users = await (
                    from user in _context.Users
                    join form in _context.ApplicationFormSet on user.FormId equals form.FormId
                    join branch in _context.BranchSet on form.IFSC equals branch.IFSC
                    join account in _context.AccountSet on user.AccountNumber equals account.AccountNumber
                    select new UsersDto
                    {
                        UserId = user.Id,
                        Username = user.UserName,
                        AccountNumber = user.AccountNumber,
                        Balance = account.Balance,
                        FormId = user.FormId,
                        FullName = form.FullName,
                        Email = form.Email,
                        IFSC = branch.IFSC,
                        BranchName = branch.BranchName,
                        IsActive = user.IsActive
                    }).ToListAsync();

                result.Response = users;
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

        public async Task<Result<UsersDto>> GetUserByIdAsync(string userId)
        {
            Result<UsersDto> result = new();
            try
            {
                var profile = await (
                    from user in _context.Users where user.Id == userId
                    join form in _context.ApplicationFormSet on user.FormId equals form.FormId
                    join branch in _context.BranchSet on form.IFSC equals branch.IFSC
                    join account in _context.AccountSet on user.AccountNumber equals account.AccountNumber
                    select new UsersDto
                    {
                        UserId = user.Id,
                        Username = user.UserName,
                        AccountNumber = user.AccountNumber,
                        Balance = account.Balance,
                        FormId = form.FormId,
                        FullName = form.FullName,
                        Email = form.Email,
                        IFSC = form.IFSC,
                        BranchName = branch.BranchName                   
                    }).FirstOrDefaultAsync();
                    
                if(profile != null)
                {
                    result.Response = profile;
                }
                else
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "404",
                        ErrorMessage = "User not found"
                    });
                }
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

        public async Task<Result<DeleteDto>> DeleteUserByIdAsync(string userId)
        {
            Result<DeleteDto> result = new Result<DeleteDto>();

            try
            {
                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "404",
                        ErrorMessage = "User not found"
                    });
                    return result;
                }

                //_context.Users.Remove(user);

                user.IsActive = false; // Soft delete
                await _context.SaveChangesAsync();

                result.Response = new DeleteDto { 
                    Id = userId,
                    Message = "User deleted successfully."
                };
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
