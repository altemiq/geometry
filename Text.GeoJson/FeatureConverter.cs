// -----------------------------------------------------------------------
// <copyright file="FeatureConverter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The <see cref="Feature"/> <see cref="JsonConverter"/>.
/// </summary>
internal sealed class FeatureConverter : JsonConverter<Feature?>
{
    /// <inheritdoc/>
    public override Feature? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.Null)
        {
            return null;
        }

        _ = reader.ReadTo(JsonTokenType.PropertyName);

        FeatureId? id = default;
        Geometry.Envelope? bbox = default;
        IGeometry? geometry = default;
        IReadOnlyDictionary<string, object?>? properties = default;

        while (reader.TokenType is not JsonTokenType.EndObject)
        {
            var propertyName = reader.GetString();

            // move to the value
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
                geometry = ReadGeometry(ref reader, options);
            }
            else if (string.Equals(propertyName, nameof(properties), StringComparison.Ordinal))
            {
                properties = ReadProperties(ref reader, options);
            }
            else if (string.Equals(propertyName, nameof(bbox), StringComparison.Ordinal))
            {
                bbox = ReadBoundingBox(ref reader);
            }
            else if (string.Equals(propertyName, nameof(id), StringComparison.Ordinal))
            {
                id = ReadId(ref reader);
            }
            else
            {
                reader.Skip();
            }

            _ = reader.Read();
        }

        return new Feature
        {
            Id = id,
            BoundingBox = bbox,
            Geometry = geometry,
            Properties = properties ?? new Dictionary<string, object?>(StringComparer.Ordinal),
        };
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, Feature? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteString("type", nameof(GeoJsonType.Feature));
        WriteId(writer, value.Id);
        WriteBoundingBox(writer, value.BoundingBox);
        WriteGeometry(writer, value.Geometry, options);
        WriteProperties(writer, value.Properties, options);

        writer.WriteEndObject();
    }

    /// <summary>
    /// Reads the geometry.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="options">The options.</param>
    /// <returns>The geometry.</returns>
    /// <exception cref="InvalidOperationException">Failed to read geometry.</exception>
    internal static IGeometry? ReadGeometry(ref Utf8JsonReader reader, JsonSerializerOptions options) => reader.ReadTo(JsonTokenType.StartObject, JsonTokenType.Null)
        ? GeometryConverter.Instance.Read(ref reader, typeof(IGeometry), options)
        : throw new InvalidOperationException();

    /// <summary>
    /// Reads the properties.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="options">The options.</param>
    /// <returns>The properties.</returns>
    internal static Dictionary<string, object?> ReadProperties(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        // move to the start of the object
        _ = reader.ReadTo(JsonTokenType.StartObject, JsonTokenType.Null);
        var dictionary = JsonSerializer.Deserialize<Dictionary<string, object?>>(ref reader, options) ?? throw new InvalidOperationException();

        return dictionary
            .ToDictionary(
                static x => x.Key,
                static x =>
                {
                    return GetValue(x.Value);

                    static object? GetValue(object? value)
                    {
                        return value switch
                        {
                            JsonElement { ValueKind: JsonValueKind.Null } => null,
                            JsonElement { ValueKind: JsonValueKind.String } e => e.GetString(),
                            JsonElement { ValueKind: JsonValueKind.Number } e when e.TryGetInt64(out var l) => l,
                            JsonElement { ValueKind: JsonValueKind.Number } e => e.GetDecimal(),
                            JsonElement { ValueKind: JsonValueKind.True } => true,
                            JsonElement { ValueKind: JsonValueKind.False } => false,
                            JsonElement { ValueKind: JsonValueKind.Object } e => ReadObject(e),
                            JsonElement { ValueKind: JsonValueKind.Array } e => ReadArray(e),
                            _ => throw new InvalidOperationException(),
                        };

                        static Dictionary<string, object?> ReadObject(JsonElement element)
                        {
                            return element
                                .EnumerateObject()
                                .ToDictionary(
                                    static property => property.Name,
                                    static property => GetValue(property.Value),
                                    StringComparer.Ordinal);
                        }

                        static object?[] ReadArray(JsonElement element)
                        {
                            return [.. element
                                .EnumerateArray()
                                .Select(static e => GetValue(e)),];
                        }
                    }
                },
                StringComparer.Ordinal);
    }

    /// <summary>
    /// Reads the ID.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <returns>The geometry.</returns>
    /// <exception cref="InvalidOperationException">Failed to read ID.</exception>
    internal static FeatureId ReadId(ref Utf8JsonReader reader) => reader.TokenType switch
    {
        JsonTokenType.Number => new FeatureId(reader.GetInt64()),
        JsonTokenType.String => new FeatureId(reader.GetString() ?? throw new ArgumentNullException(nameof(reader))),
        _ => throw new InvalidOperationException(),
    };

    /// <summary>
    /// Reads the bounding-box.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <returns>The geometry.</returns>
    internal static Geometry.Envelope? ReadBoundingBox(ref Utf8JsonReader reader)
    {
        _ = reader.Read();
        var values = new List<double>();
        while (reader.TokenType is not JsonTokenType.EndArray)
        {
            values.Add(reader.GetDouble());
            _ = reader.Read();
        }

        return new(values[0], values[1], values[2], values[3]);
    }

    /// <summary>
    /// Writes the properties.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="value">The value.</param>
    /// <param name="options">The options.</param>
    internal static void WriteProperties(Utf8JsonWriter writer, IReadOnlyDictionary<string, object?> value, JsonSerializerOptions options)
    {
        writer.WritePropertyName("properties");
        JsonSerializer.Serialize(writer, value, options);
    }

    /// <summary>
    /// Writes the geometry.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="value">The value.</param>
    /// <param name="options">The options.</param>
    internal static void WriteGeometry(Utf8JsonWriter writer, IGeometry? value, JsonSerializerOptions options)
    {
        writer.WritePropertyName("geometry");
        GeometryConverter.Instance.Write(writer, value, options);
    }

    /// <summary>
    /// Writes the bounding-box.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="value">The value.</param>
    internal static void WriteBoundingBox(Utf8JsonWriter writer, Geometry.Envelope? value)
    {
        if (value is { } bbox)
        {
            writer.WriteStartArray(nameof(bbox));
            writer.WriteNumberValue(bbox.Left);
            writer.WriteNumberValue(bbox.Bottom);
            writer.WriteNumberValue(bbox.Right);
            writer.WriteNumberValue(bbox.Top);
            writer.WriteEndArray();
        }
    }

    /// <summary>
    /// Writes the ID.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="value">The value.</param>
    internal static void WriteId(Utf8JsonWriter writer, FeatureId? value)
    {
        switch (value)
        {
            case { Value: string stringValue }:
                writer.WriteString("id", stringValue);
                break;
            case { Value: long longValue }:
                writer.WriteNumber("id", longValue);
                break;
        }
    }
}