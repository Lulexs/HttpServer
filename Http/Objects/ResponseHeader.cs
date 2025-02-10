using System.Reflection;
using System.Text;

namespace WebServer.Http.Objects;

public class ResponseHeader
{
    [FieldName("content-type")]
    public string? ContentType { get; set; }
    [FieldName("content-length")]
    public int? ContentLength { get; set; }

    [FieldName("connection")]
    public string? Connection { get; set; }
    [FieldName("server")]
    public string? Server { get; set; }

    public object? this[string propertyName]
    {
        get
        {
            var property = GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            return property?.GetValue(this);
        }
        set
        {
            var property = GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (property != null && value != null)
            {
                var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                object? convertedValue = Convert.ChangeType(value, targetType);
                property.SetValue(this, convertedValue);
            }
        }
    }

    public override string ToString()
    {
        var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                .Where(prop => prop.GetIndexParameters().Length == 0);
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