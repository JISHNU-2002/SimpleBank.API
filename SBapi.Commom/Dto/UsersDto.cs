using System.ComponentModel.DataAnnotations;

namespace SBapi.Common.Dto
{ 
    public class UsersDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public int FormId { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string IFSC { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }

}
