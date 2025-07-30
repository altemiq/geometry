// -----------------------------------------------------------------------
// <copyright file="MultiLineStringConverters.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The <see cref="MultiLineString{TPoint,TLine}"/> converters.
/// </summary>
internal static class MultiLineStringConverters
{
    /// <summary>
    /// The <see cref="MultiLineString"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class MultiLineStringConverter()
        : MultiLineStringConverter<Point, LineString, MultiLineString>(PointConverters.PointConstructor, LineStringConverters.LineStringConstructor, MultiLineStringConstructor, PointConverters.PointDeconstructor)
    {
        private static MultiLineString MultiLineStringConstructor(IList<LineString> points) => new(points);
    }

    /// <summary>
    /// The <see cref="MultiLineStringZ"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class MultiLineStringZConverter()
        : MultiLineStringConverter<PointZ, LineStringZ, MultiLineStringZ>(PointConverters.PointZConstructor, LineStringConverters.LineStringZConstructor, MultiLineStringZConstructor, PointConverters.PointZDeconstructor)
    {
        private static MultiLineStringZ MultiLineStringZConstructor(IList<LineStringZ> points) => new(points);
    }

    /// <summary>
    /// The <see cref="MultiLineStringM"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class MultiLineStringMConverter()
        : MultiLineStringConverter<PointM, LineStringM, MultiLineStringM>(PointConverters.PointMConstructor, LineStringConverters.LineStringMConstructor, MultiLineStringMConstructor, PointConverters.PointMDeconstructor)
    {
        private static MultiLineStringM MultiLineStringMConstructor(IList<LineStringM> points) => new(points);
    }

    /// <summary>
    /// The <see cref="MultiLineStringZM"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class MultiLineStringZMConverter()
        : MultiLineStringConverter<PointZM, LineStringZM, MultiLineStringZM>(PointConverters.PointZMConstructor, LineStringConverters.LineStringZMConstructor, MultiLineStringZMConstructor, PointConverters.PointZMDeconstructor)
    {
        private static MultiLineStringZM MultiLineStringZMConstructor(IList<LineStringZM> points) => new(points);
    }

    /// <summary>
    /// The <see cref="MultiLineString{TPoint,TLine}"/> <see cref="JsonConverter"/>.
    /// </summary>
    /// <typeparam name="TPoint">The type of point.</typeparam>
    /// <typeparam name="TLine">The type of line.</typeparam>
    /// <typeparam name="TMultiLine">The type of multi-line.</typeparam>
    internal abstract class MultiLineStringConverter<TPoint, TLine, TMultiLine>(Func<IList<double>, TPoint> createPoint, Func<IList<TPoint>, TLine> createLine, Func<IList<TLine>, TMultiLine> createMultiLine, Func<TPoint, IList<double>> getCoordinates) : JsonConverter<TMultiLine?>
        where TLine : Altemiq.Geometry.Polyline<TPoint>
        where TMultiLine : IEnumerable<Altemiq.Geometry.Polyline<TPoint>>
    {
        /// <inheritdoc/>
        public override TMultiLine? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.Null)
            {
                return default;
            }

            _ = reader.ReadTo(JsonTokenType.PropertyName);

            // read the coordinates
            List<TLine> coordinates = [];
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
                    if (type is not GeometryType.MultiLineString)
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
                        // read each line string
                        _ = reader.Read();
                        IList<TPoint> polyline = [];
                        while (reader.TokenType is not JsonTokenType.EndArray)
                        {
                            _ = reader.Read();
                            List<double> items = [];
                            while (reader.TokenType is not JsonTokenType.EndArray)
                            {
                                items.Add(reader.GetDouble());
                                _ = reader.Read();
                            }

                            var point = createPoint(items);

                            polyline.Add(point);
                            _ = reader.Read();
                        }

                        coordinates.Add(createLine(polyline));
                        _ = reader.Read();
                    }
                }
                else
                {
                    reader.Skip();
                }

                _ = reader.Read();
            }

            return createMultiLine(coordinates);
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, TMultiLine? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartObject();
            writer.WriteString("type", nameof(GeometryType.MultiLineString));
            writer.WriteStartArray("coordinates");

            foreach (var ring in value)
            {
                writer.WriteStartArray();
                foreach (var point in ring)
                {
                    writer.WriteStartArray();
                    foreach (var coordinate in getCoordinates(point))
                    {
                        writer.WriteNumberValue(coordinate);
                    }

                    writer.WriteEndArray();
                }

                writer.WriteEndArray();
            }

            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}