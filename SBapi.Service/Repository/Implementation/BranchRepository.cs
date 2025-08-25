using Microsoft.EntityFrameworkCore;
using SBapi.Common.Dto;
using SBapi.Common.ErrorDto;
using SBapi.Entity.Data;
using SBapi.Entity.Models;
using SBapi.Service.Repository.Interface;

namespace SBapi.Service.Repository.Implementation
{
    public class BranchRepository : IBranchRepository
    {
        private readonly AppDbContext _context;

        public BranchRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Branch>> AddBranchAsync(AddBranchDto branch)
        {
            try
            {
                var value = await _context.Set<SequenceValue>()
                    .FromSqlRaw("EXEC dbo.GetNextIFSCSequenceValue")
                    .AsNoTracking().ToListAsync();

                int val = value.First().Value;

                
                Branch newBranch = new Branch
                {
                    IFSC = "SBIFSC" + val.ToString(),
                    BranchName = branch.BranchName, 
                    State = branch.State,
                    Country = branch.Country,
                };

                await _context.BranchSet.AddAsync(newBranch);
                await _context.SaveChangesAsync();

                return new Result<Branch>
                {
                    Response = newBranch
                };
            }
            catch (Exception ex)
            {
                return new Result<Branch>
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

        public async Task<Result<DeleteDto>> DeleteBranchAsync(string ifsc)
        {
            Result<DeleteDto> result = new Result<DeleteDto>();
            try
            {
                var branch = await _context.BranchSet.FindAsync(ifsc);
                if (branch != null)
                {
                    _context.BranchSet.Remove(branch);
                    await _context.SaveChangesAsync();
                    result.Response = new DeleteDto
                    {
                        Id = branch.IFSC,
                        Message = "Branch deleted successfully."
                    };
                }
                else
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "404",
                        ErrorMessage = "Branch not found."
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

        public async Task<Result<List<Branch>>> GetAllBranchesAsync()
        {
            Result<List<Branch>> result = new Result<List<Branch>>();
            try
            {
                List<Branch> branches = await _context.BranchSet.ToListAsync();
                result.Response = branches;
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

        public async Task<Result<Branch>> GetBranchByIFSCAsync(string ifsc)
        {
            Result<Branch> result = new Result<Branch>();
            try
            {
                var branch = await _context.BranchSet.FindAsync(ifsc);
                if (branch != null)
                {
                    result.Response = branch;
                }
                else
                {
                    result.Errors.Add(new Errors
                    {
                        ErrorCode = "404",
                        ErrorMessage = "Branch not found."
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
                
        public async Task<Result<Branch>> UpdateBranchAsync(Branch branch)
        {
            Result<Branch> result = new Result<Branch>();
            try
            {
                _context.BranchSet.Update(branch);
                await _context.SaveChangesAsync();
                result.Response = branch;
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

        public async Task<Result<List<IFSCDetailsDto>>> IFSCDetailsAsync(string ifsc)
        {
            Result<List<IFSCDetailsDto>> result = new Result<List<IFSCDetailsDto>>();
            try
            {
                var query = await (
                    from form in _context.ApplicationFormSet
                    join branch in _context.BranchSet on form.IFSC equals branch.IFSC
                    join user in _context.Users on form.FormId equals user.FormId
                    join accType in _context.AccountTypeSet on form.AccountTypeId equals accType.TypeId
                    join account in _context.AccountSet on user.AccountNumber equals account.AccountNumber
                    where form.IFSC == ifsc
                    select new IFSCDetailsDto
                    {
                        IFSCCode = form.IFSC,
                        AccountNumber = account.AccountNumber,
                        FullName = form.FullName,
                        Username = user.UserName,
                        AccountTypeName = accType.TypeName,
                        BranchName = branch.BranchName,
                        Balance = account.Balance,
                        Email = form.Email,
                        PhoneNumber = form.PhoneNumber
                    }
                ).ToListAsync();

                result.Response = query;
                return result;
            }
            catch (Exception ex)
            {
                return new Result<List<IFSCDetailsDto>>
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
