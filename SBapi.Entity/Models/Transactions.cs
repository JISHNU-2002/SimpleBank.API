using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SBapi.Entity.Models
{
    [Table("tblTransaction")]
    public class Transactions
    {
        [Key]
        public int TransactionId { get; set; }
        [MaxLength(20)] 
        public required string FromAccountNumber { get; set; }
        [MaxLength(20)]
        public string? ToAccountNumber { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [DataType(DataType.Date)]
        public DateTime TransactionDate { get; set; }
        [MaxLength(20)]
        public string? TransactionType { get; set; }
    }
}
