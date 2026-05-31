namespace BomberosAPI.Application.Common.Exceptions;

public class ForbiddenException : AppException
{
    public ForbiddenException(string message = "You do not have permission to perform this action.")
        : base(message, 403) { }
}
