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
        string? id = default;
        Geometry.Envelope? bbox = default;
        Geometry.IGeometry? geometry = default;
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
                if (reader.GetString() is { } typeString)
                {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
                    var type = Enum.Parse<GeoJsonType>(typeString);
#else
                    var type = (GeoJsonType)Enum.Parse(typeof(GeoJsonType), typeString);
#endif
                    if (type is not GeoJsonType.Feature)
                    {
                        throw new InvalidOperationException();
                    }
                }
                else
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
                properties = FeatureConverter.ReadProperties(ref reader, options, TryConvertProperty);
            }
            else if (string.Equals(propertyName, nameof(bbox), StringComparison.Ordinal))
            {
                bbox = FeatureConverter.ReadBoundingBox(ref reader);
            }
            else if (string.Equals(propertyName, nameof(id), StringComparison.Ordinal))
            {
                id = reader.GetString();
            }
            else if (string.Equals(propertyName, Constants.StacVersion, StringComparison.Ordinal))
            {
                stacVersion = reader.GetString();
            }
            else if (string.Equals(propertyName, Constants.StacExtensions, StringComparison.Ordinal))
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
#pragma warning disable S3236
            Version = ExceptionHelper.ThrowIfNull(stacVersion, Constants.StacVersion),
#pragma warning restore S3236
            Extensions = stacExtensions,
            Id = ExceptionHelper.ThrowIfNull(id),
            BoundingBox = bbox,
            Geometry = geometry,
            Properties = properties ?? new Dictionary<string, object?>(StringComparer.Ordinal),
            Collection = collection,
            Links = ExceptionHelper.ThrowIfNull(links),
            Assets = ExceptionHelper.ThrowIfNull(assets),
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
        writer.WriteString(Constants.StacVersion, value.Version);
        if (value.Extensions is { } extensions)
        {
            writer.WritePropertyName(Constants.StacExtensions);
            JsonSerializer.Serialize(writer, extensions, options);
        }

        writer.WriteString("type", nameof(GeoJsonType.Feature));
        writer.WriteString("id", value.Id);
        FeatureConverter.WriteBoundingBox(writer, value.BoundingBox);
        FeatureConverter.WriteGeometry(writer, value.Geometry, options);

        FeatureConverter.WriteProperties(writer, value.Properties, options);
        writer.WriteString("collection", value.Collection);

        writer.WritePropertyName("links");
        JsonSerializer.Serialize(writer, value.Links, options);
        writer.WritePropertyName("assets");
        JsonSerializer.Serialize(writer, value.Assets, options);

        writer.WriteEndObject();
    }

    private static bool TryConvertProperty(string paramName, JsonElement element, out object? value)
    {
        if (paramName is "providers")
        {
            (var success, value) = element switch
            {
                { ValueKind: JsonValueKind.Object } => (true, (object?)JsonSerializer.Deserialize<Provider>(element.GetRawText())),
                { ValueKind: JsonValueKind.Array } => (true, element
                    .EnumerateArray()
                    .Select(arrayElement => JsonSerializer.Deserialize<Provider>(arrayElement.GetRawText()))
                    .ToList()
                    .AsReadOnly()),
                _ => (false, null),
            };

            return success;
        }

        value = default;
        return false;
    }
}