using Microsoft.AspNetCore.Identity;
using SBapi.Common.Dto;
using SBapi.Common.ErrorDto;
using SBapi.Entity.Security;

namespace SBapi.Service.Repository.Interface
{
    public interface IAuthorizeRepository
    {
        Task<Result<UserResponse>> AuthorizeUser(UserRequest userRequest);
        Task<Result<AppUser>> RegisterUser(RegisterRequestDto request);
        Task<Result<AppUser>> RegisterCustomer(CustomerRegisterDto customerRegisterDto);
        Task<Result<IdentityResult>> ChangePassword(AppUser user, string oldPassword, string newPassword);
        Task<Result<AppUser>> GetUserByName(string name);
    }
}
