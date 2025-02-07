namespace WebServer.Exceptions;

public class UnrecognizedMethodException : Exception
{
    public UnrecognizedMethodException(string message) : base(message) { }
}