using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SBapi.Common.Dto;
using SBapi.Common.ErrorDto;
using SBapi.Entity.Security;
using SBapi.Service.Repository.Interface;
using System.Data;

namespace SBapi.Service.Repository.Implementation
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleRepository(RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<Result<List<IdentityRole>>> GetAllRolesAsync()
        {
            var result = new Result<List<IdentityRole>>();
            try
            {
                var roles = await _roleManager.Roles.ToListAsync();

                if(roles != null)
                {
                    result.Response = roles;
                }
                else
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "404",
                        ErrorMessage = "No roles found"
                    });
                }                
            }
            catch (Exception ex)
            {
                result.Errors.Add(new Errors
                {
                    ErrorCode = "ROLE_FETCH_ERROR",
                    ErrorMessage = ex.Message
                });
            }
            return result;
        }

        public async Task<Result<IdentityRole>> GetRoleByIdAsync(string id)
        {
            var result = new Result<IdentityRole>();
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "ROLE_NOT_FOUND",
                        ErrorMessage = "Role not found"
                    });
                }
                else
                {
                    result.Response = role;
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add(new Errors
                {
                    ErrorCode = "ROLE_FETCH_ERROR",
                    ErrorMessage = ex.Message
                });
            }
            return result;
        }

        public async Task<Result<IdentityResult>> CreateRoleAsync(IdentityRole role)
        {
            var result = new Result<IdentityResult>();
            try
            {
                var existingRole = await _roleManager.FindByNameAsync(role.Name!);
                if (existingRole != null)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "ROLE_EXISTS",
                        ErrorMessage = "Role already exists"
                    });
                    return result;
                }

                var identityResult = await _roleManager.CreateAsync(role);
                if (!identityResult.Succeeded)
                {
                    result.Errors = identityResult.Errors
                        .Select(e => new Errors
                        {
                            ErrorCode = e.Code,
                            ErrorMessage = e.Description
                        }).ToList();
                }
                else
                {
                    result.Response = identityResult;
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add(new Errors
                {
                    ErrorCode = "ROLE_CREATION_FAILED",
                    ErrorMessage = ex.Message
                });
            }
            return result;
        }

        public async Task<Result<IdentityResult>> DeleteRoleAsync(string id)
        {
            var result = new Result<IdentityResult>();

            try
            {
                var existingRole = await _roleManager.FindByIdAsync(id);                

                if (existingRole == null)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "ROLE_NOT_FOUND",
                        ErrorMessage = "The specified role does not exist."
                    });
                    return result;
                }

                if (existingRole.Name == "SuperAdmin" || existingRole.Name == "Manager" || existingRole.Name == "Customer")
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "600",
                        ErrorMessage = "Unauthorized Action"
                    });
                    return result;
                }

                var deleteResult = await _roleManager.DeleteAsync(existingRole);
                if (!deleteResult.Succeeded)
                {
                    result.Errors = deleteResult.Errors
                        .Select(e => new Errors
                        {
                            ErrorCode = e.Code,
                            ErrorMessage = e.Description
                        }).ToList();
                }
                else
                {
                    result.Response = deleteResult;
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add(new Errors
                {
                    ErrorCode = "ROLE_DELETE_ERROR",
                    ErrorMessage = ex.Message
                });
            }

            return result;
        }

        public async Task<Result<AddRemoveRoleDto>> AddRemoveRolesAsync(string userId)
        {
            var result = new Result<AddRemoveRoleDto>();

            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if(user != null)
                {
                    var addRemoveRoleDto = new AddRemoveRoleDto
                    {
                        UserId = user.Id,
                        UserName = user.Email!
                    };

                    var roles = await _roleManager.Roles.ToListAsync();

                    if(roles != null)
                    {
                        foreach (var role in roles)
                        {
                            addRemoveRoleDto.AddRemoveRoles.Add(new AddRemoveRole
                            {
                                RoleId = role.Id,
                                RoleName = role.Name!,
                                IsSelected = await _userManager.IsInRoleAsync(user, role.Name!)
                            });
                        }
                    }

                    result.Response = addRemoveRoleDto;
                }
                else
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "300",
                        ErrorMessage = "User not found"
                    });
                } 
            }
            catch (Exception ex)
            {
                result.Errors.Add(new Errors
                {
                    ErrorCode = "ROLE_DELETE_ERROR",
                    ErrorMessage = ex.Message
                });
            }

            return result;
        }

        public async Task<Result<IdentityResult>> UpdateUserRolesAsync(AddRemoveRoleDto addRemoveRoleDto)
        {
            Result<IdentityResult> result = new Result<IdentityResult>();

            try
            {
                var user = await _userManager.FindByIdAsync(addRemoveRoleDto.UserId);

                if(user != null)
                {
                    for (int i = 0; i < addRemoveRoleDto.AddRemoveRoles.Count; i++)
                    {
                        if (addRemoveRoleDto.AddRemoveRoles[i].IsSelected && !await _userManager.IsInRoleAsync(user, addRemoveRoleDto.AddRemoveRoles[i].RoleName))
                        {
                            result.Response = await _userManager.AddToRoleAsync(user, addRemoveRoleDto.AddRemoveRoles[i].RoleName);
                        }
                        else if (!addRemoveRoleDto.AddRemoveRoles[i].IsSelected && await _userManager.IsInRoleAsync(user, addRemoveRoleDto.AddRemoveRoles[i].RoleName))
                        {
                            result.Response = await _userManager.RemoveFromRoleAsync(user, addRemoveRoleDto.AddRemoveRoles[i].RoleName);
                        }
                    }
                }
                else
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "400",
                        ErrorMessage = "User not found"
                    });
                }
                
            }
            catch(Exception ex)
            {
                result.Errors.Add(new Errors
                {
                    ErrorCode = "500",
                    ErrorMessage = ex.Message
                });
            }
            
            return result;
        }

        public async Task<Result<List<UserRolesDto>>> RoleDetailsAsync(string roleId)
        {
            Result<List<UserRolesDto>> result = new Result<List<UserRolesDto>>();

            try
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                if (role == null)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "ROLE_NOT_FOUND",
                        ErrorMessage = "Role not found"
                    });
                }
                else
                {
                    var users = await _userManager.GetUsersInRoleAsync(role.Name);

                    if(users != null) 
                    {
                        var usersList = users.Select(u => new UserRolesDto
                        {
                            FormId = u.FormId,
                            UserName = u.UserName,
                            AccountNumber = u.AccountNumber
                        }).ToList();

                        result.Response = usersList;
                    }                   
                }                    
            }
            catch(Exception ex)
            {
                result.Errors.Add(new Errors
                {
                    ErrorCode = "ROLE_DETAILS_ERROR",
                    ErrorMessage = ex.Message
                });
            }
          
            return result;
        }
    }
}
