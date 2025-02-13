namespace WebServer.ControllersAttributes;

[AttributeUsage(AttributeTargets.Method)]
public class Verb(string verb) : Attribute
{
    public string HttpMethod { get; } = verb;
}