using SBapi.Common.Dto;
using SBapi.Common.ErrorDto;
using SBapi.Entity.Models;

namespace SBapi.Service.Repository.Interface
{
    public interface IBranchRepository
    {
        Task<Result<List<Branch>>> GetAllBranchesAsync();
        Task<Result<Branch>> GetBranchByIFSCAsync(string IFSC);
        Task<Result<Branch>> AddBranchAsync(AddBranchDto branch);
        Task<Result<Branch>> UpdateBranchAsync(Branch branch);
        Task<Result<DeleteDto>> DeleteBranchAsync(string IFSC);
        Task<Result<List<IFSCDetailsDto>>> IFSCDetailsAsync(string IFSC);
    }
}
