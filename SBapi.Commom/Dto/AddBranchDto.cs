namespace SBapi.Common.Dto
{
    public class AddBranchDto
    {
        public required string BranchName { get; set; }
        public required string State { get; set; }
        public required string Country { get; set; }
    }
}
