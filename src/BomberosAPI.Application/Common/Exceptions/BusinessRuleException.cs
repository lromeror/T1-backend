namespace BomberosAPI.Application.Common.Exceptions;

/// <summary>
/// Se lanza cuando una operacion viola una regla de negocio.
/// HTTP 422 Unprocessable Entity.
/// </summary>
public class BusinessRuleException : AppException
{
    public BusinessRuleException(string message)
        : base(message, 422)
    {
    }
}
