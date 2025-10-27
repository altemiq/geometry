// -----------------------------------------------------------------------
// <copyright file="Range.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson.Stac;

/// <summary>
/// The range.
/// </summary>
public class Range
{
    /// <summary>
    /// Gets the minimum.
    /// </summary>
    [JsonPropertyName("minimum")]
    [JsonConverter(typeof(NumberOrStringConverter))]
    public required NumberOrString Minimum { get; init; }

    /// <summary>
    /// Gets the maximum.
    /// </summary>
    [JsonPropertyName("maximum")]
    [JsonConverter(typeof(NumberOrStringConverter))]
    public required NumberOrString Maximum { get; init; }

    private sealed class NumberOrStringConverter : JsonConverter<NumberOrString>
    {
        public override NumberOrString Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => reader.TokenType switch
        {
            JsonTokenType.Number => new(reader.GetDouble()),
            JsonTokenType.String => new(reader.GetString() ?? throw new ArgumentNullException(nameof(reader))),
            _ => throw new InvalidOperationException(),
        };

        public override void Write(Utf8JsonWriter writer, NumberOrString value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case { Value: string stringValue }:
                    writer.WriteStringValue(stringValue);
                    break;
                case { Value: double doubleValue }:
                    writer.WriteNumberValue(doubleValue);
                    break;
            }
        }
    }
}