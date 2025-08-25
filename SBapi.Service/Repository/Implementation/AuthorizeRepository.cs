using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SBapi.Common.Dto;
using SBapi.Common.ErrorDto;
using SBapi.Entity.Data;
using SBapi.Entity.Migrations;
using SBapi.Entity.Models;
using SBapi.Entity.Security;
using SBapi.Service.Repository.Interface;

namespace SBapi.Service.Repository.Implementation
{
    public class AuthorizeRepository : IAuthorizeRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IAccountRepository _accountRepository;

        public AuthorizeRepository(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            AppDbContext context,
            IAccountRepository accountRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _accountRepository = accountRepository;
        }

        public async Task<Result<UserResponse>> AuthorizeUser(UserRequest userRequest)
        {
            Result<UserResponse> result = new Result<UserResponse>();

            try
            {
                var user = await _userManager.FindByNameAsync(userRequest.UserName);

                if (user != null && user.UserName != null && user.Email != null)
                {
                    var userResult = await _userManager.CheckPasswordAsync(user, userRequest.Password);
                    if (userResult)
                    {
                        var userResponse = new UserResponse
                        {
                            UserId = user.Id,
                            UserName = user.UserName,
                            Email = user.Email,
                            IsActive = user.IsActive,

                            AccountNumber = user.AccountNumber ?? string.Empty,
                            FormId = user.FormId
                        };

                        var roles = await _userManager.GetRolesAsync(user);
                        foreach (var role in roles)
                        {
                            userResponse.roles.Add(new Roles
                            {
                                RoleName = role
                            });
                        }

                        result.Response = userResponse;
                    }
                    else
                    {
                        result.Errors.Add(new Errors
                        {
                            ErrorCode = "Unauthorized",
                            ErrorMessage = "Invalid username or password."
                        });
                    }
                }
                else
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "404",
                        ErrorMessage = "User not found"
                    });
                }                    
            }
            catch(Exception ex)
            {
                result.Errors.Add(new Errors
                {
                    ErrorCode = "400",
                    ErrorMessage = ex.Message
                });
            }

            return result;
        }

        public async Task<Result<AppUser>> RegisterUser(RegisterRequestDto request)
        {
            Result<AppUser> result = new Result<AppUser>();

            try
            {               
                ApplicationForm application = new()
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    AadharNumber = request.AadharNumber,
                    PAN = request.PAN,
                    Address = request.Address,
                    AccountTypeId = request.AccountTypeId,
                    IFSC = request.IFSC,
                    DateOfRegistration = DateTime.UtcNow,
                    Status = "Approved"
                };

                _context.ApplicationFormSet.Add(application);
                await _context.SaveChangesAsync();

                var accountNumber = await _accountRepository.CreateAccountAsync(0);

                AppUser user = new()
                {
                    UserName = request.UserName,
                    Email = request.UserName,
                    IsActive = true,
                    AccountNumber = accountNumber.Response?.AccountNumber,
                    FormId = application.FormId
                };

                var userResult = await _userManager.CreateAsync(user, request.Password);

                if (!userResult.Succeeded)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "500",
                        ErrorMessage = "Registration failed"
                    });
                    return result;
                }

                result.Response = user;
            }
            catch (Exception ex)
            {
                result.Errors.Add(new Errors
                {
                    ErrorCode = "500",
                    ErrorMessage = ex.Message
                });
            }

            return result;
        }

        public async Task<Result<AppUser>> RegisterCustomer(CustomerRegisterDto customerRegisterDto)
        {
            Result<AppUser> result = new Result<AppUser>();

            try
            {
                var user = new AppUser
                {
                    FormId = customerRegisterDto.FormId,
                    UserName = customerRegisterDto.UserName,
                    Email = customerRegisterDto.UserName,
                    AccountNumber = customerRegisterDto.AccountNumber,
                    IsActive = true
                };

                var customer = await _userManager.CreateAsync(user, customerRegisterDto.Password);

                if (customer.Succeeded)
                {
                    result.Response = user;
                }
                else
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "RE400",
                        ErrorMessage = "RegistrationError"
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

        public async Task<Result<IdentityResult>> ChangePassword(AppUser user, string oldPassword, string newPassword)
        {
            Result<IdentityResult> result = new Result<IdentityResult>();

            var res = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

            if (res.Succeeded)
            {
                result.Response = res;
            }
            else
            {
                result.Errors.Add(new Errors
                {
                    ErrorCode = "300",
                    ErrorMessage = res.Errors.FirstOrDefault()?.Description
                });
            }
            return result;
        }

        public async Task<Result<AppUser>> GetUserByName(string name)
        {
            Result<AppUser> result = new Result<AppUser>();

            try
            {
                var user = await _userManager.FindByNameAsync(name);

                if (user != null)
                {
                    result.Response = user;
                }
            }
            catch (Exception ex) {
                result.Errors.Add(new Errors
                {
                    ErrorCode = "404",
                    ErrorMessage = ex.Message
                });
            }
            return result;
        }
    }
}
