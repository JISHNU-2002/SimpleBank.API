using System.ComponentModel.DataAnnotations;

namespace SBapi.Common.Dto
{
    public class UserResponse
    {
        public required string UserId { get; set; }
        public required string UserName { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        public bool IsActive { get; set; }

        // AccountNumber & FormId
        public required string AccountNumber { get; set; }
        public int FormId { get; set; }
        // roles is a collection of Roles, which contains RoleName
        public ICollection<Roles> roles  { get; set; } = new List<Roles>();
    }
    public class Roles
    {
      public required string RoleName { get; set; }
    }
}
