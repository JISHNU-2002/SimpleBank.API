namespace SBapi.Common.Dto
{
    public class RegisterRequestDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public int FormId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string AadharNumber { get; set; } = string.Empty;
        public string PAN { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public int AccountTypeId { get; set; }
        public string IFSC { get; set; } = string.Empty;
        public DateTime DateOfRegistration { get; set; }
        public string Status { get; set; } = "Approved";
    }
}
