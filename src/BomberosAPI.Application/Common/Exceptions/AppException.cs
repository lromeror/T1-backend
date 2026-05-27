namespace BomberosAPI.Application.Common.Exceptions;

/// <summary>
/// Excepcion base para todas las excepciones de la aplicacion.
/// </summary>
public abstract class AppException : Exception
{
    public int StatusCode { get; }

    protected AppException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}
