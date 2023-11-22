namespace Relaxinema.WebAPI.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class JsonQueryAttribute : Attribute
{
    public string ParameterName { get; }
    public Type ParamType { get; }
    
    public JsonQueryAttribute(string parameterName, Type paramType)
    {
        ParameterName = parameterName;
        ParamType = paramType;
    }
}