// -----------------------------------------------------------------------
// <copyright file="ItemCollectionConverter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson.Stac;

/// <summary>
/// The <see cref="ItemCollection"/> <see cref="JsonConverter"/>.
/// </summary>
internal sealed class ItemCollectionConverter : JsonConverter<ItemCollection?>
{
    private readonly System.Collections.Concurrent.ConcurrentDictionary<JsonSerializerOptions, JsonSerializerOptions> optionsCache = [];

    /// <inheritdoc/>
    public override ItemCollection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.Null)
        {
            return null;
        }

        if (reader.GetType<GeoJsonType>() is not GeoJsonType.FeatureCollection)
        {
            throw new InvalidOperationException();
        }

        _ = reader.ReadTo(JsonTokenType.PropertyName);

        List<Item>? features = default;
        int numberMatched = default;
        int numberReturned = default;
        List<Link>? links = default;
        while (reader.TokenType is not JsonTokenType.EndObject)
        {
            var propertyName = reader.GetString();
            _ = reader.Read();
            if (string.Equals(propertyName, nameof(features), StringComparison.Ordinal))
            {
                features = JsonSerializer.Deserialize<List<Item>>(ref reader, options);
            }
            else if (string.Equals(propertyName, nameof(numberMatched), StringComparison.Ordinal))
            {
                numberMatched = reader.GetInt32();
            }
            else if (string.Equals(propertyName, nameof(numberReturned), StringComparison.Ordinal))
            {
                numberReturned = reader.GetInt32();
            }
            else if (string.Equals(propertyName, nameof(links), StringComparison.Ordinal))
            {
                links = JsonSerializer.Deserialize<List<Link>>(ref reader, options);
            }
            else
            {
                reader.Skip();
            }

            _ = reader.Read();
        }

        return new()
        {
            NumberMatched = numberMatched,
            NumberReturned = numberReturned,
            Features = features as IReadOnlyList<Item> ?? [],
            Links = links as IReadOnlyList<Link> ?? [],
        };
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, ItemCollection? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteString("type", nameof(GeoJsonType.FeatureCollection));
        writer.WriteNumber("numberMatched", value.NumberMatched);
        writer.WriteNumber("numberReturned", value.NumberReturned);

        writer.WriteStartArray("features");
        foreach (var feature in value.Features)
        {
            if (feature is Item item)
            {
                JsonSerializer.Serialize(writer, item, options);
            }
            else
            {
                JsonSerializer.Serialize(writer, feature, options);
            }
        }

        writer.WriteEndArray();

        writer.WritePropertyName("links");
        JsonSerializer.Serialize(writer, value.Links, GetOptions(options));

        writer.WriteEndObject();

        JsonSerializerOptions GetOptions(JsonSerializerOptions opts)
        {
            return opts.DefaultIgnoreCondition is JsonIgnoreCondition.WhenWritingNull
                ? opts
                : this.optionsCache.GetOrAdd(opts, o => new(o) { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
        }
    }
}