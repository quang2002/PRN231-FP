namespace SharedService.Helpers;

using System.ComponentModel;
using System.Reflection;

[AttributeUsage(AttributeTargets.Property)]
public class EnvironmentAttribute : Attribute
{
    public string Key { get; private set; }

    public EnvironmentAttribute(string key)
    {
        this.Key = key;
    }

    public static void Populate(object obj)
    {
        const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        foreach (var propertyInfo in obj.GetType().GetProperties(bindingFlags))
        {
            if (propertyInfo.GetCustomAttribute<EnvironmentAttribute>() is not { } envVarAttribute)
            {
                continue;
            }

            if (Environment.GetEnvironmentVariable(envVarAttribute.Key) is not { Length: > 0 } environmentVariable)
            {
                continue;
            }

            var converter = TypeDescriptor.GetConverter(propertyInfo.PropertyType);

            if (!converter.CanConvertFrom(typeof(string))) continue;
            
            var value = converter.ConvertFromString(environmentVariable);
            propertyInfo.SetValue(obj, value);
        }
    }
}