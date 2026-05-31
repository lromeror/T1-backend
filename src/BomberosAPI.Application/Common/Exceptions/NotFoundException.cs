namespace BomberosAPI.Application.Common.Exceptions;

public class NotFoundException : AppException
{
    public NotFoundException(string resourceName, object key)
        : base($"{resourceName} with id '{key}' was not found.", 404) { }

    public NotFoundException(string message) : base(message, 404) { }
}
