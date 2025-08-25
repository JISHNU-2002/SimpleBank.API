using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SBapi.Entity.Models;
using SBapi.Service.Repository.Interface;

namespace SBapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]

    public class ApplicationFormController : ControllerBase
    {
        private readonly IApplicationFormRepository _applicationFormRepository;

        public ApplicationFormController(IApplicationFormRepository applicationFormRepository)
        {
            _applicationFormRepository = applicationFormRepository;
        }

        [HttpPut("UpdateForm")]
        public async Task<IActionResult> UpdateForm(ApplicationForm applicationForm)
        {
            return Ok(await _applicationFormRepository.UpdateFormAsync(applicationForm));
        }
        [HttpDelete("DeleteForm/{formId}")]
        public async Task<IActionResult> DeleteFormById(int formId)
        {
            return Ok(await _applicationFormRepository.DeleteFormAsync(formId));
        }


        [HttpGet("GetAllForms")]
        public async Task<IActionResult> GetAllForms()
        {
            return Ok(await _applicationFormRepository.GetAllFormsAsync());
        }

        [HttpGet("GetFormById/{formId}")]
        public async Task<IActionResult> GetFormById(int formId)
        {
            return Ok(await _applicationFormRepository.GetFormByIdAsync(formId));
        }

        [HttpPost("AddForm")]
        public async Task<IActionResult> AddForm(ApplicationForm applicationForm)
        {
            return Ok(await _applicationFormRepository.AddFormAsync(applicationForm));
        }

        [HttpPost("ApproveForm/{formId}")]
        public async Task<IActionResult> ApproveForm(int formId)
        {
            return Ok(await _applicationFormRepository.ApproveFormAsync(formId));
        }

        [HttpPost("RejectForm/{formId}")]
        public async Task<IActionResult> RejectForm(int formId)
        {
            return Ok(await _applicationFormRepository.RejectFormAsync(formId));
        }
    }
}
