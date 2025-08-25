using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SBapi.Common.Dto;
using SBapi.Entity.Models;
using SBapi.Service.Repository.Interface;

namespace SBapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]

    public class BranchController : ControllerBase
    {
        private readonly IBranchRepository _branchRepository;

        public BranchController(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        [HttpGet("GetAllBranches")]
        public async Task<IActionResult> GetAllBranches()
        {
            return Ok(await _branchRepository.GetAllBranchesAsync());
        }

        [HttpGet("GetBranchByIFSC/{ifsc}")]
        public async Task<IActionResult> GetBranchByIFSC(string ifsc)
        {
            return Ok(await _branchRepository.GetBranchByIFSCAsync(ifsc));
        }

        [HttpPost("AddBranch")]
        public async Task<IActionResult> AddBranch(AddBranchDto branchDto)
        {
            return Ok(await _branchRepository.AddBranchAsync(branchDto));
        }

        [HttpPut("UpdateBranch")]
        public async Task<IActionResult> UpdateBranch(Branch branch)
        {
            return Ok(await _branchRepository.UpdateBranchAsync(branch));
        }

        [HttpDelete("DeleteBranch/{ifsc}")]
        public async Task<IActionResult> DeleteBranch(string ifsc)
        {
            return Ok(await _branchRepository.DeleteBranchAsync(ifsc));
        }

        [HttpGet("IFSCDetails/{ifsc}")]
        public async Task<IActionResult> GetIFSCDetails(string ifsc)
        {
            return Ok(await _branchRepository.IFSCDetailsAsync(ifsc));
        }
    }
}
