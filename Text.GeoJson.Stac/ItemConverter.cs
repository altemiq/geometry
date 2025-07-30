// -----------------------------------------------------------------------
// <copyright file="ItemConverter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson.Stac;

/// <summary>
/// The <see cref="Item"/> <see cref="JsonConverter"/>.
/// </summary>
internal sealed class ItemConverter : JsonConverter<Item?>
{
    private readonly System.Collections.Concurrent.ConcurrentDictionary<JsonSerializerOptions, JsonSerializerOptions> optionsCache = [];

    /// <inheritdoc/>
    public override Item? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.Null)
        {
            return default;
        }

        _ = reader.ReadTo(JsonTokenType.PropertyName);

        string? stacVersion = default;
        List<string?>? stacExtensions = default;
        FeatureId? id = default;
        Altemiq.Geometry.Envelope? bbox = default;
        IGeometry? geometry = default;
        Dictionary<string, object?>? properties = default;
        List<Link>? links = default;
        Dictionary<string, Asset?>? assets = default;
        string? collection = default;

        while (reader.TokenType is not JsonTokenType.EndObject)
        {
            var propertyName = reader.GetString();
            _ = reader.Read();
            if (string.Equals(propertyName, "type", StringComparison.Ordinal))
            {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
                var type = Enum.Parse<GeoJsonType>(reader.GetString());
#else
                var type = (GeoJsonType)Enum.Parse(typeof(GeoJsonType), reader.GetString());
#endif
                if (type is not GeoJsonType.Feature)
                {
                    throw new InvalidOperationException();
                }
            }
            else if (string.Equals(propertyName, nameof(geometry), StringComparison.Ordinal))
            {
                geometry = FeatureConverter.ReadGeometry(ref reader, options);
            }
            else if (string.Equals(propertyName, nameof(properties), StringComparison.Ordinal))
            {
                properties = FeatureConverter.ReadProperties(ref reader, options);
            }
            else if (string.Equals(propertyName, nameof(bbox), StringComparison.Ordinal))
            {
                bbox = FeatureConverter.ReadBoundingBox(ref reader);
            }
            else if (string.Equals(propertyName, nameof(id), StringComparison.Ordinal))
            {
                id = FeatureConverter.ReadId(ref reader);
            }
            else if (string.Equals(propertyName, "stac_version", StringComparison.Ordinal))
            {
                stacVersion = reader.GetString();
            }
            else if (string.Equals(propertyName, "stac_extensions", StringComparison.Ordinal))
            {
                _ = reader.Read();
                var values = new List<string?>();
                while (reader.TokenType is not JsonTokenType.EndArray)
                {
                    values.Add(reader.GetString());
                    _ = reader.Read();
                }

                stacExtensions = values;
            }
            else if (string.Equals(propertyName, nameof(collection), StringComparison.Ordinal))
            {
                collection = reader.GetString();
            }
            else if (string.Equals(propertyName, nameof(assets), StringComparison.Ordinal))
            {
                assets = JsonSerializer.Deserialize<Dictionary<string, Asset?>>(ref reader, options);
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

        _ = reader.ReadTo(JsonTokenType.EndObject);

        return new()
        {
            Version = stacVersion,
            Extensions = stacExtensions,
            Id = id,
            BoundingBox = bbox,
            Geometry = geometry,
            Properties = properties ?? new Dictionary<string, object?>(StringComparer.Ordinal),
            Collection = collection,
            Links = links,
            Assets = assets,
        };
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, Item? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteString("stac_version", value.Version);
        writer.WritePropertyName("stac_extensions");
        JsonSerializer.Serialize(writer, value.Extensions, options);
        writer.WriteString("type", nameof(GeoJsonType.Feature));
        FeatureConverter.WriteId(writer, value.Id);
        FeatureConverter.WriteBoundingBox(writer, value.BoundingBox);
        FeatureConverter.WriteGeometry(writer, value.Geometry, options);
        FeatureConverter.WriteProperties(writer, value.Properties, options);
        writer.WriteString("collection", value.Collection);

        var subOptions = GetOptions(options);
        writer.WritePropertyName("links");
        JsonSerializer.Serialize(writer, value.Links, subOptions);
        writer.WritePropertyName("assets");
        JsonSerializer.Serialize(writer, value.Assets, subOptions);

        writer.WriteEndObject();

        JsonSerializerOptions GetOptions(JsonSerializerOptions opts)
        {
            return opts.DefaultIgnoreCondition is JsonIgnoreCondition.WhenWritingNull
                ? opts
                : this.optionsCache.GetOrAdd(opts, o => new(o) { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
        }
    }
}