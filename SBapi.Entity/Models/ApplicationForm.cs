using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SBapi.Entity.Models
{
    [Table("tblApplicationForm")]
    public class ApplicationForm
    {
        [Key]
        public int FormId { get; set; }
        [MaxLength(50)]
        public required string FullName { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        [Phone]
        public required string PhoneNumber { get; set; }
        [MaxLength(12)]
        public required string AadharNumber { get; set; }
        [MaxLength(10)]
        public required string PAN { get; set; }
        [MaxLength(100)]
        public required string Address { get; set; }
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
        public required int AccountTypeId { get; set; }
        public required string IFSC { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfRegistration { get; set; }
        [MaxLength(20)]
        public string Status { get; set; } = Utility.ApplicationStatus.FormFilled.ToString();
    }
}
