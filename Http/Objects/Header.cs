using System.Reflection;
using System.Text;
using WebServer.Exceptions;

namespace WebServer.Http.Objects;

public class Header
{
    public string? Connection { get; set; }
    public string? UserAgent { get; set; }
    public string? Host { get; set; }
    public ulong? ContentLength { get; set; }
    public string? ContentType { get; set; }

    public string? this[string fieldName]
    {
        get => fieldName switch
        {
            nameof(Connection) => Connection,
            nameof(UserAgent) => UserAgent,
            nameof(Host) => Host,
            nameof(ContentLength) => ContentLength?.ToString(),
            nameof(ContentType) => ContentType,
            _ => throw new ArgumentException($"Field '{fieldName}' not found.", nameof(fieldName))
        };
        set
        {
            switch (fieldName)
            {
                case "Connection": Connection = value; break;
                case "User-Agent": UserAgent = value; break;
                case "Host": Host = value; break;
                case "content-length":
                    ContentLength = ulong.TryParse(value, out var parsedValue) ?
                    parsedValue : throw new InvalidFormatException($"Invalid value for {nameof(ContentLength)}."); break;
                case "Content-Type": ContentType = value; break;
                default: throw new ArgumentException($"Field '{fieldName}' not found.", nameof(fieldName));
            }
        }
    }
    public override string ToString()
    {
        var properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                .Where(prop => prop.GetIndexParameters().Length == 0);
        var sb = new StringBuilder("{");

        foreach (var prop in properties)
        {
            var value = prop.GetValue(this, null);
            if (value != null)
            {
                sb.Append($"{prop.Name}: {value},\n");
            }
        }

        if (sb.Length > 1)
            sb.Length -= 2;

        sb.Append('}');
        return sb.ToString();
    }
}
