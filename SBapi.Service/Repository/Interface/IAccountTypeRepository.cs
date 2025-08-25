using SBapi.Common.Dto;
using SBapi.Common.ErrorDto;
using SBapi.Entity.Models;

namespace SBapi.Service.Repository.Interface
{
    public interface IAccountTypeRepository
    {
        Task<Result<List<AccountType>>> GetAllAccountTypes();
        Task<Result<AccountType>> GetAccountTypeById(int typeId);
        Task<Result<AccountType>> AddAccountType(AccountType accountType);
        Task<Result<AccountType>> UpdateAccountType(AccountType accountType);
        Task<Result<DeleteDto>> DeleteAccountType(int typeId);
    }
}
