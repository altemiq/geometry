// -----------------------------------------------------------------------
// <copyright file="LineStringConverters.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The <see cref="LineString{TPoint}"/> converters.
/// </summary>
internal static class LineStringConverters
{
    /// <summary>
    /// The <see cref="LineString"/> constructor.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <returns>The line string.</returns>
    public static LineString LineStringConstructor(IList<Point> points) => new(points);

    /// <summary>
    /// The <see cref="LineStringZ"/> constructor.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <returns>The line string.</returns>
    public static LineStringZ LineStringZConstructor(IList<PointZ> points) => new(points);

    /// <summary>
    /// The <see cref="LineStringM"/> constructor.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <returns>The line string.</returns>
    public static LineStringM LineStringMConstructor(IList<PointM> points) => new(points);

    /// <summary>
    /// The <see cref="LineStringZM"/> constructor.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <returns>The line string.</returns>
    public static LineStringZM LineStringZMConstructor(IList<PointZM> points) => new(points);

    /// <summary>
    /// The <see cref="LineString"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class LineStringConverter() : LineStringConverter<Point, LineString>(PointConverters.PointConstructor, LineStringConstructor, PointConverters.PointDeconstructor);

    /// <summary>
    /// The <see cref="LineStringZ"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class LineStringZConverter() : LineStringConverter<PointZ, LineStringZ>(PointConverters.PointZConstructor, LineStringZConstructor, PointConverters.PointZDeconstructor);

    /// <summary>
    /// The <see cref="LineStringM"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class LineStringMConverter() : LineStringConverter<PointM, LineStringM>(PointConverters.PointMConstructor, LineStringMConstructor, PointConverters.PointMDeconstructor);

    /// <summary>
    /// The <see cref="LineStringZM"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class LineStringZMConverter() : LineStringConverter<PointZM, LineStringZM>(PointConverters.PointZMConstructor, LineStringZMConstructor, PointConverters.PointZMDeconstructor);

    /// <summary>
    /// The <see cref="LineString"/> <see cref="JsonConverter"/>.
    /// </summary>
    /// <typeparam name="TPoint">The type of point.</typeparam>
    /// <typeparam name="TLine">The type of line.</typeparam>
    public abstract class LineStringConverter<TPoint, TLine>(Func<IList<double>, TPoint> createPoint, Func<IList<TPoint>, TLine> createLine, Func<TPoint, IList<double>> getCoordinates) : JsonConverter<TLine?>
        where TLine : Altemiq.Geometry.Polyline<TPoint>
    {
        /// <inheritdoc/>
        public override TLine? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.Null)
            {
                return default;
            }

            // read the coordinates
            _ = reader.ReadTo(JsonTokenType.PropertyName);

            List<TPoint> coordinates = [];
            while (reader.TokenType is not JsonTokenType.EndObject)
            {
                var propertyName = reader.GetString();
                _ = reader.Read();

                if (string.Equals(propertyName, "type", StringComparison.Ordinal))
                {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
                    var type = Enum.Parse<GeometryType>(reader.GetString());
#else
                    var type = (GeometryType)Enum.Parse(typeof(GeometryType), reader.GetString());
#endif
                    if (type is not GeometryType.LineString)
                    {
                        throw new InvalidOperationException();
                    }
                }
                else if (string.Equals(propertyName, nameof(coordinates), StringComparison.Ordinal))
                {
                    // read the array
                    _ = reader.ReadTo(JsonTokenType.StartArray);
                    _ = reader.Read();
                    while (reader.TokenType is not JsonTokenType.EndArray)
                    {
                        // read each coordinate
                        _ = reader.Read();
                        List<double> items = [];
                        while (reader.TokenType is not JsonTokenType.EndArray)
                        {
                            items.Add(reader.GetDouble());
                            _ = reader.Read();
                        }

                        var coordinate = createPoint(items);

                        coordinates.Add(coordinate);
                        _ = reader.Read();
                    }
                }
                else
                {
                    reader.Skip();
                }

                _ = reader.Read();
            }

            return createLine(coordinates);
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, TLine? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartObject();
            writer.WriteString("type", nameof(GeometryType.LineString));
            writer.WriteStartArray("coordinates");

            foreach (var point in value)
            {
                writer.WriteStartArray();
                foreach (var coordinate in getCoordinates(point))
                {
                    writer.WriteNumberValue(coordinate);
                }

                writer.WriteEndArray();
            }

            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}