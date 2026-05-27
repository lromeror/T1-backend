namespace BomberosAPI.Application.Common.Exceptions;

/// <summary>
/// Se lanza cuando existe un conflicto con el estado actual del recurso (ej: duplicado).
/// HTTP 409 Conflict.
/// </summary>
public class ConflictException : AppException
{
    public ConflictException(string message)
        : base(message, 409)
    {
    }
}
