namespace WebServer.Http.Rules;

public class ValidationResult
{
    public StatusCodes? ErrorCode { get; set; }
    public required HttpRequest Request { get; set; }
}