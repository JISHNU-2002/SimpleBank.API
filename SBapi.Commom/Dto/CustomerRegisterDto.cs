using System.ComponentModel.DataAnnotations;

namespace SBapi.Common.Dto
{
    public class CustomerRegisterDto
    {
        public int FormId { get; set; }
        [EmailAddress]
        public required string UserName { get; set; }
        [DataType(DataType.Password)]
        public required string Password { get; set; }
        public required string AccountNumber { get; set; }
    }
}
