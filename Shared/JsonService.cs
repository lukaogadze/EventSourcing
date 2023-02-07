using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Shared.Optionals;

namespace Shared;

public static class JsonService
{
    private static readonly JsonSerializerOptions Options = new()
    {
        IncludeFields = true,
        TypeInfoResolver = new CustomTypeInfoResolver()
    };

    private static readonly Dictionary<string, Type> Types = GetTypes();

    private static Dictionary<string, Type> GetTypes()
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(x => x.IsClass && x.GetCustomAttribute(typeof(CompilerGeneratedAttribute)) == null && x.FullName.StartsWith("Domain"))
            .ToDictionary(x => x.Name, x => x);


        return types;
    }

    public static Optional<T> Deserialize<T>(string json, string concreteClassTypeName = null)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return Optional.Nothing<T>();
        }

        if (!string.IsNullOrWhiteSpace(concreteClassTypeName))
        {
            var type = Types[concreteClassTypeName];
            var result = JsonSerializer.Deserialize(json, type, Options);

            return Optional.Something((T)result);
        }
        else
        {
            var result = JsonSerializer.Deserialize<T>(json, Options);
            return Optional.Something(result);
        }
    }


    public static string Serialize(object value)
    {
        return JsonSerializer.Serialize(value, value.GetType(), Options);
    }

    public static async Task<string> SerializeAsync(object value)
    {
        using var stream = new MemoryStream() { Position = 0 };
        await JsonSerializer.SerializeAsync(stream, value, value.GetType(), Options);
        using var streamReader = new StreamReader(stream);
        return await streamReader.ReadToEndAsync();
    }


    public static async ValueTask<Optional<T>> DeserializeAsync<T>(string json, string concreteClassTypeName = null)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return await ValueTask.FromResult(Optional.Nothing<T>());
        }

        using var stream = new MemoryStream() { Position = 0 };
        await using var streamWriter = new StreamWriter(stream);
        await streamWriter.WriteAsync(json);
        await streamWriter.FlushAsync();

        if (!string.IsNullOrWhiteSpace(concreteClassTypeName))
        {
            var type = Types[concreteClassTypeName];
            var result = await JsonSerializer.DeserializeAsync(stream, type, Options);
            return Optional.Something((T)result);
        }
        else
        {
            var result = await JsonSerializer.DeserializeAsync<T>(stream, Options);

            return Optional.Something(result);
        }
    }
}

public class CustomTypeInfoResolver : DefaultJsonTypeInfoResolver
{
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

        if (jsonTypeInfo.Kind == JsonTypeInfoKind.Object)
        {
            foreach (var jsonPropertyInfo in jsonTypeInfo.Properties)
            {
                var propertyName = jsonPropertyInfo.Name;

                jsonPropertyInfo.Set ??= (@object, value) =>
                {
                    @object.GetType().GetProperty(propertyName)?.SetValue(@object, value);
                };
            }

            if (jsonTypeInfo.CreateObject is null)
            {
                if (jsonTypeInfo.Type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length == 0)
                {
                    // The type doesn't have public constructors
                    jsonTypeInfo.CreateObject = () => Activator.CreateInstance(jsonTypeInfo.Type, true);
                }
            }
        }

        return jsonTypeInfo;
    }
}