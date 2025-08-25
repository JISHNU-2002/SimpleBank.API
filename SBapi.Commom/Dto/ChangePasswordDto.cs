using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SBapi.Common.Dto
{
    public class ChangePasswordDto
    {
        public required string Username { get; set; }
        [DataType(DataType.Password)]
        [DisplayName("Current Password")]
        public required string CurrentPassword { get; set; }
        [DataType(DataType.Password)]
        [DisplayName("New Password")]
        public required string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [DisplayName("Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "Passwords don't match.")]
        public required string ConfirmPassword { get; set; }
    }
}
