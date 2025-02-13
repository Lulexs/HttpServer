namespace WebServer.ControllersAttributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class Route(string route) : Attribute
{
    public string TheRoute { get; } = route;
}