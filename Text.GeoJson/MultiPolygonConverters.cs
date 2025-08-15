// -----------------------------------------------------------------------
// <copyright file="MultiPolygonConverters.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The <see cref="Geometry.MultiGeometry{TPolygon}"/> converters.
/// </summary>
internal static class MultiPolygonConverters
{
    /// <summary>
    /// The <see cref="MultiPolygon"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class MultiPolygonConverter()
        : MultiPolygonConverter<Geometry.Point, Geometry.Polygon, MultiPolygon>(PointConverters.PointConstructor, PolygonConverters.PolygonConstructor, MultiPolygonConstructor, PointConverters.PointDeconstructor)
    {
        private static MultiPolygon MultiPolygonConstructor(IList<Geometry.Polygon> points) => new(points);
    }

    /// <summary>
    /// The <see cref="MultiPolygonZ"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class MultiPolygonZConverter()
        : MultiPolygonConverter<Geometry.PointZ, Geometry.PolygonZ, MultiPolygonZ>(PointConverters.PointZConstructor, PolygonConverters.PolygonZConstructor, MultiPolygonZConstructor, PointConverters.PointZDeconstructor)
    {
        private static MultiPolygonZ MultiPolygonZConstructor(IList<Geometry.PolygonZ> points) => new(points);
    }

    /// <summary>
    /// The <see cref="MultiPolygonM"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class MultiPolygonMConverter()
        : MultiPolygonConverter<Geometry.PointM, Geometry.PolygonM, MultiPolygonM>(PointConverters.PointMConstructor, PolygonConverters.PolygonMConstructor, MultiPolygonMConstructor, PointConverters.PointMDeconstructor)
    {
        private static MultiPolygonM MultiPolygonMConstructor(IList<Geometry.PolygonM> points) => new(points);
    }

    /// <summary>
    /// The <see cref="MultiPolygonZM"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class MultiPolygonZMConverter()
        : MultiPolygonConverter<Geometry.PointZM, Geometry.PolygonZM, MultiPolygonZM>(PointConverters.PointZMConstructor, PolygonConverters.PolygonZMConstructor, MultiPolygonZMConstructor, PointConverters.PointZMDeconstructor)
    {
        private static MultiPolygonZM MultiPolygonZMConstructor(IList<Geometry.PolygonZM> points) => new(points);
    }

    /// <summary>
    /// The <see cref="Geometry.Polygon"/> <see cref="JsonConverter"/>.
    /// </summary>
    /// <typeparam name="TPoint">The type of point.</typeparam>
    /// <typeparam name="TPolygon">The type of polygon.</typeparam>
    /// <typeparam name="TMultiPolygon">The type of multi-polygon.</typeparam>
    public abstract class MultiPolygonConverter<TPoint, TPolygon, TMultiPolygon>(
        Func<IList<double>, TPoint> createPoint,
        Func<IList<Geometry.LinearRing<TPoint>>, TPolygon> createPolygon,
        Func<IList<TPolygon>, TMultiPolygon> createMultiPolygon,
        Func<TPoint, IList<double>> getCoordinates) : JsonConverter<TMultiPolygon?>
        where TPolygon : Altemiq.Geometry.Polygon<TPoint>
        where TMultiPolygon : IEnumerable<Altemiq.Geometry.Polygon<TPoint>>
    {
        /// <inheritdoc/>
        public override TMultiPolygon? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.Null)
            {
                return default;
            }

            _ = reader.ReadTo(JsonTokenType.PropertyName);

            // read the coordinates
            List<TPolygon> coordinates = [];
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
                    if (type is not GeometryType.MultiPolygon)
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
                        _ = reader.Read();
                        IList<Geometry.LinearRing<TPoint>> polygon = [];
                        while (reader.TokenType is not JsonTokenType.EndArray)
                        {
                            // read each ring
                            _ = reader.Read();
                            Geometry.LinearRing<TPoint> ring = [];
                            while (reader.TokenType is not JsonTokenType.EndArray)
                            {
                                _ = reader.Read();
                                List<double> items = [];
                                while (reader.TokenType is not JsonTokenType.EndArray)
                                {
                                    items.Add(reader.GetDouble());
                                    _ = reader.Read();
                                }

                                var coordinate = createPoint(items);

                                ring.Add(coordinate);
                                _ = reader.Read();
                            }

                            polygon.Add(ring);
                            _ = reader.Read();
                        }

                        coordinates.Add(createPolygon(polygon));
                        _ = reader.Read();
                    }

                    _ = reader.Read();
                }
                else
                {
                    reader.Skip();
                }

                _ = reader.Read();
            }

            return createMultiPolygon(coordinates);
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, TMultiPolygon? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartObject();
            writer.WriteString("type", nameof(GeometryType.MultiPolygon));
            writer.WriteStartArray("coordinates");

            foreach (var polygon in value)
            {
                writer.WriteStartArray();
                foreach (var ring in polygon)
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
            }

            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}