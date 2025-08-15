// -----------------------------------------------------------------------
// <copyright file="PolygonConverters.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The <see cref="Geometry.Polygon{T}"/> converters.
/// </summary>
internal static class PolygonConverters
{
    /// <summary>
    /// The <see cref="Geometry.Polygon"/> constructor.
    /// </summary>
    /// <param name="rings">The rings.</param>
    /// <returns>The polygon.</returns>
    public static Geometry.Polygon PolygonConstructor(IList<Geometry.LinearRing<Geometry.Point>> rings) => new([.. rings]);

    /// <summary>
    /// The <see cref="Geometry.PolygonZ"/> constructor.
    /// </summary>
    /// <param name="rings">The rings.</param>
    /// <returns>The polygon.</returns>
    public static Geometry.PolygonZ PolygonZConstructor(IList<Geometry.LinearRing<Geometry.PointZ>> rings) => new([.. rings]);

    /// <summary>
    /// The <see cref="Geometry.PolygonM"/> constructor.
    /// </summary>
    /// <param name="rings">The rings.</param>
    /// <returns>The polygon.</returns>
    public static Geometry.PolygonM PolygonMConstructor(IList<Geometry.LinearRing<Geometry.PointM>> rings) => new([.. rings]);

    /// <summary>
    /// The <see cref="Geometry.PolygonZM"/> constructor.
    /// </summary>
    /// <param name="rings">The rings.</param>
    /// <returns>The polygon.</returns>
    public static Geometry.PolygonZM PolygonZMConstructor(IList<Geometry.LinearRing<Geometry.PointZM>> rings) => new([.. rings]);

    /// <summary>
    /// The <see cref="Geometry.Polygon"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class PolygonConverter() : PolygonConverter<Geometry.Point, Geometry.Polygon>(PointConverters.PointConstructor, PolygonConstructor, PointConverters.PointDeconstructor);

    /// <summary>
    /// The <see cref="Geometry.PolygonZ"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class PolygonZConverter() : PolygonConverter<Geometry.PointZ, Geometry.PolygonZ>(PointConverters.PointZConstructor, PolygonZConstructor, PointConverters.PointZDeconstructor);

    /// <summary>
    /// The <see cref="Geometry.PolygonM"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class PolygonMConverter() : PolygonConverter<Geometry.PointM, Geometry.PolygonM>(PointConverters.PointMConstructor, PolygonMConstructor, PointConverters.PointMDeconstructor);

    /// <summary>
    /// The <see cref="Geometry.PolygonZM"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class PolygonZMConverter() : PolygonConverter<Geometry.PointZM, Geometry.PolygonZM>(PointConverters.PointZMConstructor, PolygonZMConstructor, PointConverters.PointZMDeconstructor);

    /// <summary>
    /// The <see cref="Geometry.Polygon{TPoint}"/> <see cref="JsonConverter"/>.
    /// </summary>
    /// <typeparam name="TPoint">The type of point.</typeparam>
    /// <typeparam name="TPolygon">The type of polygon.</typeparam>
    public abstract class PolygonConverter<TPoint, TPolygon>(Func<IList<double>, TPoint> createPoint, Func<IList<Geometry.LinearRing<TPoint>>, TPolygon> createLine, Func<TPoint, IList<double>> getCoordinates)
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

            IList<Geometry.LinearRing<TPoint>> coordinates = [];
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