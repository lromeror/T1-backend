namespace BomberosAPI.Application.Common.Exceptions;

/// <summary>
/// Se lanza cuando el usuario no tiene permisos para realizar la operacion.
/// HTTP 403 Forbidden.
/// </summary>
public class ForbiddenException : AppException
{
    public ForbiddenException(string message = "You do not have permission to perform this action.")
        : base(message, 403)
    {
    }
}
