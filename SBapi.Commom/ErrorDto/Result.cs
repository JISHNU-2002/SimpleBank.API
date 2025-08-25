namespace SBapi.Common.ErrorDto
{
    public abstract class Result
    {
        public List<Errors> Errors { get; set; } = new List<Errors>();
        public bool isError => Errors != null && Errors.Any();
    }

    public class Result<T> : Result
    {
        public T ?Response { get; set; }
        public string? WarningMessage { get; set; }
    }
}
