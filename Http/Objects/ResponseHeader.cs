using System.Reflection;
using System.Text;

namespace WebServer.Http.Objects;

public class ResponseHeader
{
    [FieldName("content-type")]
    public string? ContentType { get; set; }

    [FieldName("connection")]
    public string? Connection { get; set; }

    public override string ToString()
    {
        var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var sb = new StringBuilder();

        foreach (var prop in properties)
        {
            var value = prop.GetValue(this, null);
            if (value != null)
            {
                var fieldNameAttr = prop.GetCustomAttribute<FieldNameAttribute>();
                var fieldName = fieldNameAttr?.FieldName ?? prop.Name;

                sb.Append($"{fieldName}: {value}{HttpConstants.NewLine}");
            }
        }

        if (sb.Length > 1)
            sb.Length -= 2;

        return sb.ToString();
    }
}