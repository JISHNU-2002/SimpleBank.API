using SBapi.Common.Dto;

namespace SBapi.Service.Repository.Interface
{
    public interface ITokenRepository
    {
        public Task<string> GetTokenAsync(UserRequest userRequest);
    }
}
