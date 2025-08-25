using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SBapi.Common.Dto;
using SBapi.Service.Repository.Implementation;
using SBapi.Service.Repository.Interface;

namespace SBapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]

    //[Authorize(Roles = "SuperAdmin")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            return Ok(await _roleRepository.GetAllRolesAsync());
        }

        [HttpGet("GetRoleById/{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            return Ok(await _roleRepository.GetRoleByIdAsync(id));
        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(RoleDto roleDto)
        {
            IdentityRole role = new()
            {
                Name = roleDto.RoleName
            };

            return Ok(await _roleRepository.CreateRoleAsync(role));
        }

        [HttpDelete("DeleteRole/{id}")]     
        public async Task<IActionResult> DeleteRole(string id)
        {
            return Ok(await _roleRepository.DeleteRoleAsync(id));
        }

        [HttpGet("UserRoles/{userId}")]
        public async Task<ActionResult<AddRemoveRoleDto>> GetUserRoles(string userId)
        {
            return Ok(await _roleRepository.AddRemoveRolesAsync(userId));
        }

        [HttpPost("UpdateUserRoles")]
        public async Task<IActionResult> UpdateUserRoles(AddRemoveRoleDto addRemoveRoleDto)
        {
            return Ok(await _roleRepository.UpdateUserRolesAsync(addRemoveRoleDto));
        }

        [HttpGet("GetRoleDetailsById/{id}")]
        public async Task<IActionResult> GetRoleDetailsById(string id)
        {
            return Ok(await _roleRepository.RoleDetailsAsync(id));
        }
    }
}
