namespace Task07.Core.Exceptions;

public class ApiException : Exception
{
    public int StatusCode { get; }
    public string? Details { get; }

    public ApiException(string message, int statusCode = 500, string? details = null) 
        : base(message)
    {
        StatusCode = statusCode;
        Details = details;
    }
}

public class NotFoundException : ApiException
{
    public NotFoundException(string message, string? details = null) 
        : base(message, 404, details)
    {
    }
}

public class ValidationException : ApiException
{
    public ValidationException(string message, string? details = null) 
        : base(message, 400, details)
    {
    }
}