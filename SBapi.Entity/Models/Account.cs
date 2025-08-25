using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SBapi.Entity.Models
{
    [Table("tblAccount")]
    public class Account
    {
        [Key]
        [MaxLength(20)]
        public required string AccountNumber { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
    }
}
