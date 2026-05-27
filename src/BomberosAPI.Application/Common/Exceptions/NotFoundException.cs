namespace BomberosAPI.Application.Common.Exceptions;

/// <summary>
/// Se lanza cuando un recurso solicitado no existe en la base de datos.
/// HTTP 404 Not Found.
/// </summary>
public class NotFoundException : AppException
{
    public NotFoundException(string resourceName, object key)
        : base($"{resourceName} with id '{key}' was not found.", 404)
    {
    }

    public NotFoundException(string message)
        : base(message, 404)
    {
    }
}
