using SBapi.Common.Dto;
using SBapi.Common.ErrorDto;
using SBapi.Entity.Models;

namespace SBapi.Service.Repository.Interface
{
    public interface IApplicationFormRepository
    {
        Task<Result<ApplicationForm>> UpdateFormAsync(ApplicationForm applicationForm);
        Task<Result<DeleteDto>> DeleteFormAsync(int formId);


        Task<Result<List<ApplicationFormDto>>> GetAllFormsAsync();
        Task<Result<ApplicationFormDto>> GetFormByIdAsync(int formId);
        Task<Result<ApplicationForm>> AddFormAsync(ApplicationForm applicationForm);
        Task<Result<ApplicationForm>> ApproveFormAsync(int formId);
        Task<Result<ApplicationForm>> RejectFormAsync(int formId);
    }
}
