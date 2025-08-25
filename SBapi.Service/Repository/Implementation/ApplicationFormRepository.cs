using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SBapi.Common.Dto;
using SBapi.Common.ErrorDto;
using SBapi.Entity.Data;
using SBapi.Entity.Models;
using SBapi.Service.Repository.Interface;
using SBapi.Entity.Utility;
using Microsoft.AspNetCore.Identity;
using SBapi.Entity.Security;


namespace SBapi.Service.Repository.Implementation
{
    public class ApplicationFormRepository : IApplicationFormRepository
    {
        private readonly AppDbContext _context;
        private readonly IAccountRepository _accountRepository;
        private readonly IAuthorizeRepository _authorizeRepository;
        private readonly UserManager<AppUser> _userManager;

        public ApplicationFormRepository(AppDbContext context,
            IAccountRepository accountRepository,
            IAuthorizeRepository authorizeRepository,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _accountRepository = accountRepository;
            _authorizeRepository = authorizeRepository;
            _userManager = userManager;
        }

        public async Task<Result<DeleteDto>> DeleteFormAsync(int formId)
        {
            Result<DeleteDto> result = new Result<DeleteDto>();
            try
            {
                var form = await _context.ApplicationFormSet.FindAsync(formId);
                if (form != null)
                {
                    _context.ApplicationFormSet.Remove(form);
                    await _context.SaveChangesAsync();
                    result.Response = new DeleteDto
                    {
                        Id = form.FormId.ToString(),
                        Message = "Form deleted successfully."
                    };
                }
                else
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "404",
                        ErrorMessage = "Form not found."
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

        public async Task<Result<ApplicationForm>> UpdateFormAsync(ApplicationForm applicationForm)
        {
            Result<ApplicationForm> result = new Result<ApplicationForm>();
            try
            {
                _context.ApplicationFormSet.Update(applicationForm);
                await _context.SaveChangesAsync();
                result.Response = applicationForm;
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


        public async Task<Result<ApplicationForm>> AddFormAsync(ApplicationForm applicationForm)
        {
            try
            {
                _context.ApplicationFormSet.Add(applicationForm);
                await _context.SaveChangesAsync();
                return new Result<ApplicationForm>
                {
                    Response = applicationForm
                };
            }
            catch (Exception ex)
            {
                return new Result<ApplicationForm>
                {
                    Errors = new List<Errors>
                    {
                        new Errors
                        {
                            ErrorCode = "DB500",
                            ErrorMessage = ex.Message
                        }
                    }
                };
            }
        }

        public async Task<Result<List<ApplicationFormDto>>> GetAllFormsAsync()
        {
            Result<List<ApplicationFormDto>> result = new Result<List<ApplicationFormDto>>();
            try
            {
                var query = await (
                    from forms in _context.ApplicationFormSet
                    join accType in _context.AccountTypeSet on forms.AccountTypeId equals accType.TypeId
                    join branch in _context.BranchSet on forms.IFSC equals branch.IFSC
                    select new ApplicationFormDto
                    {
                        FormId = forms.FormId,
                        FullName = forms.FullName,
                        Email = forms.Email,
                        PhoneNumber = forms.PhoneNumber,
                        AadharNumber = forms.AadharNumber,
                        PAN = forms.PAN,
                        Address = forms.Address,
                        DOB = forms.DOB,
                        AccountTypeId = forms.AccountTypeId,
                        AccountTypeName = accType.TypeName,
                        IFSC = forms.IFSC,
                        BranchName = branch.BranchName,
                        DateOfRegistration = forms.DateOfRegistration,
                        Status = forms.Status
                    }
                ).ToListAsync();

                if(query == null)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "404",
                        ErrorMessage = "No record found"
                    });
                }
                else
                {
                    result.Response = query;
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

        public async Task<Result<ApplicationFormDto>> GetFormByIdAsync(int formId)
        {
            Result<ApplicationFormDto> result = new Result<ApplicationFormDto>();
            try
            {
                var query = await (
                    from form in _context.ApplicationFormSet
                    join accType in _context.AccountTypeSet on form.AccountTypeId equals accType.TypeId
                    join branch in _context.BranchSet on form.IFSC equals branch.IFSC
                    where form.FormId == formId
                    select new ApplicationFormDto
                    {
                        FormId = form.FormId,
                        FullName = form.FullName,
                        Email = form.Email,
                        PhoneNumber = form.PhoneNumber,
                        AadharNumber = form.AadharNumber,
                        PAN = form.PAN,
                        Address = form.Address,
                        DOB = form.DOB,
                        AccountTypeId = form.AccountTypeId,
                        AccountTypeName = accType.TypeName,
                        IFSC = form.IFSC,
                        BranchName = branch.BranchName,
                        DateOfRegistration = form.DateOfRegistration,
                        Status = form.Status
                    }
                ).FirstOrDefaultAsync();

                if (query == null)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "404",
                        ErrorMessage = "No record found"
                    });
                }
                else
                {
                    result.Response = query;
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

        public async Task<Result<ApplicationForm>> ApproveFormAsync(int formId)
        {
            Result<ApplicationForm> result = new Result<ApplicationForm>();

            try
            {
                var applicationForm = await _context.ApplicationFormSet.FindAsync(formId);
                if (applicationForm == null)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "404",
                        ErrorMessage = "Application form not found."
                    });
                    return result;
                }

                applicationForm.Status = ApplicationStatus.Approved.ToString();

                var accountTypeId = applicationForm.AccountTypeId;

                var minBalance = await (
                    from accType in _context.AccountTypeSet
                    where accType.TypeId == accountTypeId
                    select accType.MinBalance
                ).FirstOrDefaultAsync();

                var newAccount = await _accountRepository.CreateAccountAsync(minBalance);

                if (newAccount.Response == null)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "AC500",
                        ErrorMessage = "Failed to create account."
                    });
                    return result;
                }

                var registerResult = await _authorizeRepository.RegisterCustomer(new CustomerRegisterDto
                {
                    FormId = applicationForm.FormId,
                    UserName = applicationForm.Email,
                    Password = "Password@123",
                    AccountNumber = newAccount.Response.AccountNumber
                });

                if (registerResult.Response == null)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "REG500",
                        ErrorMessage = "User registration failed."
                    });
                    return result;
                }

                var user = await _userManager.FindByEmailAsync(applicationForm.Email);
                if(user == null)
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "400",
                        ErrorMessage = "User not found."
                    });
                    return result;
                }
                await _userManager.AddToRoleAsync(user, "Customer");

                _context.ApplicationFormSet.Update(applicationForm);
                await _context.SaveChangesAsync();

                result.Response = applicationForm;
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add(new Errors
                {
                    ErrorCode = "DB500",
                    ErrorMessage = ex.Message
                });
                return result;
            }
        }

        public async Task<Result<ApplicationForm>> RejectFormAsync(int formId)
        {
            Result<ApplicationForm> result = new Result<ApplicationForm>();

            try
            {
                var applicationForm = await _context.ApplicationFormSet.FindAsync(formId);
                if (applicationForm == null)
                {
                    return new Result<ApplicationForm>
                    {
                        Errors = new List<Errors>
                        {
                            new Errors
                            {
                                ErrorCode = "404",
                                ErrorMessage = "Application form not found."
                            }
                        }
                    };
                }

                applicationForm.Status = ApplicationStatus.Rejected.ToString();
                _context.ApplicationFormSet.Update(applicationForm);
                await _context.SaveChangesAsync();

                return new Result<ApplicationForm>
                {
                    Response = applicationForm
                };
            }
            catch (Exception ex)
            {
                return new Result<ApplicationForm>
                {
                    Errors = new List<Errors>
                    {
                        new Errors
                        {
                            ErrorCode = "DB500",
                            ErrorMessage = ex.Message
                        }
                    }
                };
            }
        }
    }
}
