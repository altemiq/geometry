// -----------------------------------------------------------------------
// <copyright file="FeatureCollectionConverter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The <see cref="FeatureCollection"/> <see cref="JsonConverter"/>.
/// </summary>
internal sealed class FeatureCollectionConverter : JsonConverter<FeatureCollection?>
{
    /// <inheritdoc/>
    public override FeatureCollection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.Null)
        {
            return null;
        }

        _ = reader.ReadTo(JsonTokenType.PropertyName);

        List<Feature>? features = default;
        while (reader.TokenType is not JsonTokenType.EndObject)
        {
            var propertyName = reader.GetString();
            _ = reader.Read();
            if (string.Equals(propertyName, nameof(features), StringComparison.Ordinal))
            {
                features = JsonSerializer.Deserialize<List<Feature>>(ref reader, options);
            }
            else
            {
                reader.Skip();
            }

            _ = reader.Read();
        }

        return new()
        {
            Features = (IReadOnlyList<Feature>?)features ?? [],
        };
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, FeatureCollection? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteString("type", nameof(GeoJsonType.FeatureCollection));

        writer.WritePropertyName("features");
        JsonSerializer.Serialize(writer, value.Features, options);

        writer.WriteEndObject();
    }
}