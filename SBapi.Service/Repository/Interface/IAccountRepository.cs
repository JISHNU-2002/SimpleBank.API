using Microsoft.AspNetCore.Identity;
using SBapi.Common.Dto;
using SBapi.Common.ErrorDto;
using SBapi.Entity.Models;
using SBapi.Entity.Security;

namespace SBapi.Service.Repository.Interface
{
    public interface IAccountRepository
    {
        Task<Result<Account>> CreateAccountAsync(decimal initialBalance);
        Task<Result<DashboardDto>> GetDashboardDataAsync(string accountNumber);
        Task<Result<ProfileDto>> GetProfileByFormId(int formId);
        Task<Result<ProfileDto>> UpdateProfileByFormId(ProfileDto profileDto);
        Task<Result<List<UsersDto>>> GetAllUsersWithDetailsAsync();
        Task<Result<UsersDto>> GetUserByIdAsync(string userId);
        Task<Result<DeleteDto>> DeleteUserByIdAsync(string userId);
    }
}
