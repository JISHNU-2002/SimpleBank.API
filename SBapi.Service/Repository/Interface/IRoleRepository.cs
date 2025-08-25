using Microsoft.AspNetCore.Identity;
using SBapi.Common.Dto;
using SBapi.Common.ErrorDto;

namespace SBapi.Service.Repository.Interface
{
    public interface IRoleRepository
    {
        Task<Result<List<IdentityRole>>> GetAllRolesAsync();
        Task<Result<IdentityRole>> GetRoleByIdAsync(string id);
        Task<Result<IdentityResult>> CreateRoleAsync(IdentityRole role);
        Task<Result<IdentityResult>> DeleteRoleAsync(string id);
        Task<Result<AddRemoveRoleDto>> AddRemoveRolesAsync(string userId);
        Task<Result<IdentityResult>> UpdateUserRolesAsync(AddRemoveRoleDto addRemoveRoleDto);
        Task<Result<List<UserRolesDto>>> RoleDetailsAsync(string roleId);
    }
}
