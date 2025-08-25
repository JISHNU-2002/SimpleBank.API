using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SBapi.Entity.Security
{
    public class AppUser : IdentityUser
    {
        public bool IsActive { get; set; } = true;
        [MaxLength(20)]
        public string? AccountNumber { get; set; }
        public int FormId { get; set; }
    }
}
