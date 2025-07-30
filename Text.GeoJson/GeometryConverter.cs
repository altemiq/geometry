// -----------------------------------------------------------------------
// <copyright file="GeometryConverter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The <see cref="IGeometry"/> <see cref="JsonConverter"/>.
/// </summary>
internal sealed class GeometryConverter : JsonConverter<IGeometry?>
{
    /// <summary>
    /// The instance of the converter.
    /// </summary>
    public static readonly JsonConverter<IGeometry?> Instance = new GeometryConverter();

    /// <inheritdoc/>
    public override IGeometry? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.Null)
        {
            return null;
        }

        var baseReader = reader;
        return (IGeometry?)JsonSerializer.Deserialize(
            ref reader,
            GetGeometryType(baseReader.GetType<GeometryType>()),
            options);
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, IGeometry? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }

    private static Type GetGeometryType(GeometryType type) => type switch
    {
        GeometryType.Point => typeof(Point),
        GeometryType.LineString => typeof(LineString),
        GeometryType.Polygon => typeof(Polygon),
        GeometryType.MultiPoint => typeof(MultiPoint),
        GeometryType.MultiLineString => typeof(MultiLineString),
        GeometryType.MultiPolygon => typeof(MultiPolygon),
        _ => throw new InvalidOperationException(),
    };
}