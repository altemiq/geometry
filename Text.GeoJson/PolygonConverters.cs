// -----------------------------------------------------------------------
// <copyright file="PolygonConverters.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The <see cref="Polygon{T}"/> converters.
/// </summary>
internal static class PolygonConverters
{
    /// <summary>
    /// The <see cref="Polygon"/> constructor.
    /// </summary>
    /// <param name="rings">The rings.</param>
    /// <returns>The polygon.</returns>
    public static Polygon PolygonConstructor(IList<Altemiq.Geometry.LinearRing<Point>> rings) => new([.. rings]);

    /// <summary>
    /// The <see cref="PolygonZ"/> constructor.
    /// </summary>
    /// <param name="rings">The rings.</param>
    /// <returns>The polygon.</returns>
    public static PolygonZ PolygonZConstructor(IList<Altemiq.Geometry.LinearRing<PointZ>> rings) => new([.. rings]);

    /// <summary>
    /// The <see cref="PolygonM"/> constructor.
    /// </summary>
    /// <param name="rings">The rings.</param>
    /// <returns>The polygon.</returns>
    public static PolygonM PolygonMConstructor(IList<Altemiq.Geometry.LinearRing<PointM>> rings) => new([.. rings]);

    /// <summary>
    /// The <see cref="PolygonZM"/> constructor.
    /// </summary>
    /// <param name="rings">The rings.</param>
    /// <returns>The polygon.</returns>
    public static PolygonZM PolygonZMConstructor(IList<Altemiq.Geometry.LinearRing<PointZM>> rings) => new([.. rings]);

    /// <summary>
    /// The <see cref="Polygon"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class PolygonConverter() : PolygonConverter<Point, Polygon>(PointConverters.PointConstructor, PolygonConstructor, PointConverters.PointDeconstructor);

    /// <summary>
    /// The <see cref="PolygonZ"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class PolygonZConverter() : PolygonConverter<PointZ, PolygonZ>(PointConverters.PointZConstructor, PolygonZConstructor, PointConverters.PointZDeconstructor);

    /// <summary>
    /// The <see cref="PolygonM"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class PolygonMConverter() : PolygonConverter<PointM, PolygonM>(PointConverters.PointMConstructor, PolygonMConstructor, PointConverters.PointMDeconstructor);

    /// <summary>
    /// The <see cref="PolygonZM"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class PolygonZMConverter() : PolygonConverter<PointZM, PolygonZM>(PointConverters.PointZMConstructor, PolygonZMConstructor, PointConverters.PointZMDeconstructor);

    /// <summary>
    /// The <see cref="Polygon"/> <see cref="JsonConverter"/>.
    /// </summary>
    /// <typeparam name="TPoint">The type of point.</typeparam>
    /// <typeparam name="TPolygon">The type of polygon.</typeparam>
    public abstract class PolygonConverter<TPoint, TPolygon>(Func<IList<double>, TPoint> createPoint, Func<IList<Altemiq.Geometry.LinearRing<TPoint>>, TPolygon> createLine, Func<TPoint, IList<double>> getCoordinates)
        : JsonConverter<TPolygon?>
        where TPolygon : Altemiq.Geometry.Polygon<TPoint>
    {
        /// <inheritdoc/>
        public override TPolygon? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.Null)
            {
                return null;
            }

            _ = reader.ReadTo(JsonTokenType.PropertyName);

            IList<Altemiq.Geometry.LinearRing<TPoint>> coordinates = [];
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
                    if (type is not GeometryType.Polygon)
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
                        // read each ring
                        _ = reader.Read();
                        Altemiq.Geometry.LinearRing<TPoint> ring = [];
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

                        coordinates.Add(ring);
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

            return createLine(coordinates);
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, TPolygon? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartObject();
            writer.WriteString("type", nameof(GeometryType.Polygon));
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