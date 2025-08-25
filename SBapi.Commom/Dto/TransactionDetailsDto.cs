namespace SBapi.Common.Dto
{
    public class TransactionDetailsDto
    {
        public string FullName { get; set; } = string.Empty;
        public string FromAccount { get; set; } = string.Empty;
        public string FromIFSC { get; set; } = string.Empty;
        public string ToAccount { get; set; } = string.Empty;
        public string ToIFSC { get; set; } = string.Empty;
        public string TransactionType { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
