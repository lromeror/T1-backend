namespace BomberosAPI.Application.Common.Exceptions;

/// <summary>
/// Se lanza cuando el usuario no esta autenticado.
/// HTTP 401 Unauthorized.
/// </summary>
public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message = "Authentication is required.")
        : base(message, 401)
    {
    }
}
