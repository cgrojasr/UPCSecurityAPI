namespace UPCSecurityAPI.CrossCutting.Exceptions;

public class AppValidationException : Exception
{
    public AppValidationException(string message)
        : base(message)
    {
    }

    public AppValidationException(Dictionary<string, string[]> errors)
        : base("Validation error.")
    {
        Errors = errors;
    }

    public Dictionary<string, string[]>? Errors { get; }
}
