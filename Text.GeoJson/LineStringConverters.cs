// -----------------------------------------------------------------------
// <copyright file="LineStringConverters.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The <see cref="Geometry.Polyline{TPoint}"/> converters.
/// </summary>
internal static class LineStringConverters
{
    /// <summary>
    /// The <see cref="Geometry.Polyline"/> constructor.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <returns>The line string.</returns>
    public static Geometry.Polyline LineStringConstructor(IList<Geometry.Point> points) => new(points);

    /// <summary>
    /// The <see cref="Geometry.PolylineZ"/> constructor.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <returns>The line string.</returns>
    public static Geometry.PolylineZ LineStringZConstructor(IList<Geometry.PointZ> points) => new(points);

    /// <summary>
    /// The <see cref="Geometry.PolylineM"/> constructor.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <returns>The line string.</returns>
    public static Geometry.PolylineM LineStringMConstructor(IList<Geometry.PointM> points) => new(points);

    /// <summary>
    /// The <see cref="Geometry.PolylineZM"/> constructor.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <returns>The line string.</returns>
    public static Geometry.PolylineZM LineStringZMConstructor(IList<Geometry.PointZM> points) => new(points);

    /// <summary>
    /// The <see cref="Geometry.Polyline"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class LineStringConverter() : LineStringConverter<Geometry.Point, Geometry.Polyline>(PointConverters.PointConstructor, LineStringConstructor, PointConverters.PointDeconstructor);

    /// <summary>
    /// The <see cref="Geometry.PolylineZ"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class LineStringZConverter() : LineStringConverter<Geometry.PointZ, Geometry.PolylineZ>(PointConverters.PointZConstructor, LineStringZConstructor, PointConverters.PointZDeconstructor);

    /// <summary>
    /// The <see cref="Geometry.PolylineM"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class LineStringMConverter() : LineStringConverter<Geometry.PointM, Geometry.PolylineM>(PointConverters.PointMConstructor, LineStringMConstructor, PointConverters.PointMDeconstructor);

    /// <summary>
    /// The <see cref="Geometry.PolylineZM"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class LineStringZMConverter() : LineStringConverter<Geometry.PointZM, Geometry.PolylineZM>(PointConverters.PointZMConstructor, LineStringZMConstructor, PointConverters.PointZMDeconstructor);

    /// <summary>
    /// The <see cref="Geometry.Polyline{TPoint}"/> <see cref="JsonConverter"/>.
    /// </summary>
    /// <typeparam name="TPoint">The type of point.</typeparam>
    /// <typeparam name="TLine">The type of line.</typeparam>
    public abstract class LineStringConverter<TPoint, TLine>(Func<IList<double>, TPoint> createPoint, Func<IList<TPoint>, TLine> createLine, Func<TPoint, IList<double>> getCoordinates) : JsonConverter<TLine?>
        where TLine : Geometry.Polyline<TPoint>
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