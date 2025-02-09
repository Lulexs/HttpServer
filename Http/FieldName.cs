namespace WebServer.Http;

[AttributeUsage(AttributeTargets.Property)]
public class FieldNameAttribute(string fieldName) : Attribute
{
    public string FieldName { get; } = fieldName;
}