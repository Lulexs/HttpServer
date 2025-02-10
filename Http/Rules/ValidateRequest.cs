namespace WebServer.Http.Rules;

public static class ValidateRequest
{
    public static ValidationResult Validate(HttpRequest request)
    {
        if (!HttpValidation.IsValidHttpMethod(request.RequestLine.HttpMethod))
        {
            return new ValidationResult() { Request = request, ErrorCode = StatusCodes.StatusCodes405MethodNotAllowed };
        }
        else if (!HttpValidation.IsValidHttpVersion(request.RequestLine.HttpVersion))
        {
            return new ValidationResult() { Request = request, ErrorCode = StatusCodes.StatusCode505HttpVersionNotSupported };
        }
        else if (!HttpValidation.IsValidConnection(request.Header.Connection))
        {
            return new ValidationResult() { Request = request, ErrorCode = StatusCodes.StatusCodes400BadRequest };
        }

        return new ValidationResult() { Request = request, ErrorCode = null };
    }
}