namespace SBapi.Common.Dto
{
    public class DashboardDto
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public List<TransactionDto> Transactions { get; set; } = new();
    }

    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public string FromAccount { get; set; } = string.Empty;
        public string FromIFSC { get; set; } = string.Empty;
        public string ToAccount { get; set; } = string.Empty;
        public string ToIFSC { get; set; } = string.Empty;
        public string TransactionType { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
