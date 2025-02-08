namespace WebServer.Exceptions;

public class TerminalInvalidFormatException : Exception
{
    public TerminalInvalidFormatException() { }
    public TerminalInvalidFormatException(string message) : base(message) { }
}