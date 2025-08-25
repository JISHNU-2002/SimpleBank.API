using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SBapi.Entity.Models
{
    [Table("tblBranch")]
    public class Branch
    {
        [Key]
        [MaxLength(20)]
        public required string IFSC { get; set; }
        [MaxLength(50)]
        public string BranchName { get; set; } = string.Empty;
        [MaxLength(50)]
        public string State { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Country { get; set; } = string.Empty;
    }
}
