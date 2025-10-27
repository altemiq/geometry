// -----------------------------------------------------------------------
// <copyright file="Link.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson.Stac;

/// <summary>
/// The link.
/// </summary>
public class Link
{
    /// <summary>
    /// Gets the relation type of the link.
    /// </summary>
    [JsonPropertyName("rel")]
    [JsonPropertyOrder(0)]
    public required string Relation { get; init; }

    /// <summary>
    /// Gets the media type of the resource.
    /// </summary>
    [JsonPropertyName("type")]
    [JsonPropertyOrder(1)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Type { get; init; }

    /// <summary>
    /// Gets the location of the resource.
    /// </summary>
    [JsonPropertyName("href")]
    [JsonPropertyOrder(2)]
    public required Uri Location { get; init; }

    /// <summary>
    /// Gets the title of the resource.
    /// </summary>
    [JsonPropertyName("title")]
    [JsonPropertyOrder(3)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Title { get; init; }

    /// <summary>
    /// Gets the HTTP method that the resource expects.
    /// </summary>
    [JsonPropertyName("method")]
    [JsonPropertyOrder(4)]
    [JsonConverter(typeof(HttpMethodConverter))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public System.Net.Http.HttpMethod? Method { get; init; }

    private sealed class HttpMethodConverter : JsonConverter<System.Net.Http.HttpMethod?>
    {
        public override System.Net.Http.HttpMethod? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.String)
            {
                return reader.GetString() switch
                {
                    "GET" => System.Net.Http.HttpMethod.Get,
                    "POST" => System.Net.Http.HttpMethod.Post,
                    "PUT" => System.Net.Http.HttpMethod.Put,
                    "DELETE" => System.Net.Http.HttpMethod.Delete,
                    "HEAD" => System.Net.Http.HttpMethod.Head,
                    "OPTIONS" => System.Net.Http.HttpMethod.Options,
                    "TRACE" => System.Net.Http.HttpMethod.Trace,
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
                    "PATCH" => System.Net.Http.HttpMethod.Patch,
#endif
#if NET7_0_OR_GREATER
                    "CONNECT" => System.Net.Http.HttpMethod.Connect,
#endif
#if NET10_0_OR_GREATER
                    "QUERY" => System.Net.Http.HttpMethod.Query,
#endif
                    _ => default,
                };
            }

            return default;
        }

        public override void Write(Utf8JsonWriter writer, System.Net.Http.HttpMethod? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStringValue(value.Method);
        }
    }
}