namespace BomberosAPI.Application.Common.Exceptions;

public class ValidationException : AppException
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public ValidationException(IReadOnlyDictionary<string, string[]> errors)
        : base("One or more validation errors occurred.", 400)
        => Errors = errors;

    public ValidationException(string field, string message)
        : this(new Dictionary<string, string[]> { { field, [message] } }) { }
}
