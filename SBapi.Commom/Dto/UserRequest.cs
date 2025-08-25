using System.ComponentModel.DataAnnotations;

namespace SBapi.Common.Dto
{
    public class UserRequest
    {
        [EmailAddress]
        public required string UserName { get; set; }
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
