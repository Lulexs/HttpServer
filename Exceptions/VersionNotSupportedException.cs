namespace WebServer.Exceptions;

public class VersionNotSupportedException : Exception
{
    public VersionNotSupportedException(string message) : base(message) { }
}