namespace BomberosAPI.Application.Common.Exceptions;

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message = "Authentication is required.")
        : base(message, 401) { }
}
