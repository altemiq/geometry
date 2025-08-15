namespace Altemiq.Text.GeoJson.Stac;

using System.Text.Json;

internal static class Serializer
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions().AddGeoJson();
    
    public static TValue? Deserialize<TValue>(string json)
    {
        return JsonSerializer.Deserialize<TValue>(json, Options);
    }
    
    public static string Serialize<TValue>(TValue value)
    {
        return JsonSerializer.Serialize(value, Options);
    }
}