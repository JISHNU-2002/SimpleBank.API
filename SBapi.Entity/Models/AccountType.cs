using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SBapi.Entity.Models
{
    [Table("tblAccountType")]
    public class    AccountType
    {
        [Key]
        public int TypeId { get; set; }
        [MaxLength(50)]
        public required string TypeName { get; set; } = "Temp Account Type";

        [Column(TypeName = "decimal(18,2)")]
        public decimal MinBalance { get; set; }
    }
}
