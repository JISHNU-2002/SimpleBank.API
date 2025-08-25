using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBapi.Common.Dto
{
    public class ProfileDto
    {
        // From ApplicationForm
        public int FormId { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string AadharNumber { get; set; }
        public required string PAN { get; set; }
        public required string Address { get; set; }
        public DateTime DOB { get; set; }
        public int AccountTypeId { get; set; }
        public required string AccountTypeName { get; set; }
        public required string IFSC { get; set; }
        public DateTime DateOfRegistration { get; set; }

        // From Account 
        public required string AccountNumber { get; set; }
        public required string BranchName { get; set; }
        public decimal Balance { get; set; }
    }
}
