using Microsoft.EntityFrameworkCore;
using SBapi.Common.Dto;
using SBapi.Common.ErrorDto;
using SBapi.Entity.Data;
using SBapi.Entity.Models;
using SBapi.Service.Repository.Interface;
using System;

namespace SBapi.Service.Repository.Implementation
{
    public class AccountTypeRepository : IAccountTypeRepository
    {
        private readonly AppDbContext _context;

        public AccountTypeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<AccountType>> AddAccountType(AccountType accountType)
        {
            try
            {
                _context.AccountTypeSet.Add(accountType);
                await _context.SaveChangesAsync();
                return new Result<AccountType>
                {
                    Response = accountType
                };
            }
            catch (Exception ex)
            {
                return new Result<AccountType>
                {
                    Errors = new List<Errors>
                    {
                        new Errors
                        {
                            ErrorCode = "DB500",
                            ErrorMessage = ex.Message
                        }
                    }
                };
            }
        }

        public async Task<Result<DeleteDto>> DeleteAccountType(int typeId)
        {
            Result<DeleteDto> result = new Result<DeleteDto>();
            try
            {
                var type = await _context.AccountTypeSet.FindAsync(typeId);

                if (type != null)
                {
                    _context.AccountTypeSet.Remove(type);
                    await _context.SaveChangesAsync();

                    result.Response = new DeleteDto
                    {
                        Id = type.TypeId.ToString(),
                        Message = "Account type deleted successfully."
                    };
                }
                else
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "404",
                        ErrorMessage = "Form not found."
                    });
                }
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

        public async Task<Result<AccountType>> GetAccountTypeById(int typeId)
        {
            Result<AccountType> result = new Result<AccountType>();
            try
            {
                var type = await _context.AccountTypeSet.FindAsync(typeId);

                if (type != null)
                {
                    result.Response = type;
                }
                else
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "404",
                        ErrorMessage = "Form not found."
                    });
                }
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

        public async Task<Result<List<AccountType>>> GetAllAccountTypes()
        {
            Result<List<AccountType>> result = new Result<List<AccountType>>();
            try
            {
                List<AccountType> types = await _context.AccountTypeSet.ToListAsync();

                if (types != null)
                {
                    result.Response = types;
                }
                else
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "404",
                        ErrorMessage = "Form not found."
                    });
                }
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

        public async Task<Result<AccountType>> UpdateAccountType(AccountType accountType)
        {
            Result<AccountType> result = new Result<AccountType>();
            try
            {
                _context.AccountTypeSet.Update(accountType);
                await _context.SaveChangesAsync();
                result.Response = accountType;
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
    }
}
