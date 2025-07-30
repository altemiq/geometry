// -----------------------------------------------------------------------
// <copyright file="MultiPointConverters.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The <see cref="MultiPoint{TPoint}"/> converters.
/// </summary>
internal static class MultiPointConverters
{
    /// <summary>
    /// The <see cref="MultiPoint"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class MultiPointConverter() : MultiPointConverter<Point, MultiPoint>(PointConverters.PointConstructor, MultiPointConstructor, PointConverters.PointDeconstructor)
    {
        private static MultiPoint MultiPointConstructor(IList<Point> points) => new(points);
    }

    /// <summary>
    /// The <see cref="MultiPointZ"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class MultiPointZConverter() : MultiPointConverter<PointZ, MultiPointZ>(PointConverters.PointZConstructor, MultiPointZConstructor, PointConverters.PointZDeconstructor)
    {
        private static MultiPointZ MultiPointZConstructor(IList<PointZ> points) => new(points);
    }

    /// <summary>
    /// The <see cref="MultiPointM"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class MultiPointMConverter() : MultiPointConverter<PointM, MultiPointM>(PointConverters.PointMConstructor, MultiPointMConstructor, PointConverters.PointMDeconstructor)
    {
        private static MultiPointM MultiPointMConstructor(IList<PointM> points) => new(points);
    }

    /// <summary>
    /// The <see cref="MultiPointZ"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class MultiPointZMConverter() : MultiPointConverter<PointZM, MultiPointZM>(PointConverters.PointZMConstructor, MultiPointZMConstructor, PointConverters.PointZMDeconstructor)
    {
        private static MultiPointZM MultiPointZMConstructor(IList<PointZM> points) => new(points);
    }

    /// <summary>
    /// The <see cref="MultiPoint"/> <see cref="JsonConverter"/>.
    /// </summary>
    /// <typeparam name="TPoint">The type of point.</typeparam>
    /// <typeparam name="TMultiPoint">The type of multipoint.</typeparam>
    public abstract class MultiPointConverter<TPoint, TMultiPoint>(Func<IList<double>, TPoint> createPoint, Func<IList<TPoint>, TMultiPoint> createMultiPoint, Func<TPoint, IList<double>> getCoordinates) : JsonConverter<TMultiPoint?>
        where TMultiPoint : IEnumerable<TPoint>
    {
        /// <inheritdoc/>
        public override TMultiPoint? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.Null)
            {
                return default;
            }

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
                    if (type is not GeometryType.MultiPoint)
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

                    _ = reader.Read();
                }
                else
                {
                    reader.Skip();
                }

                _ = reader.Read();
            }

            return createMultiPoint(coordinates);
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, TMultiPoint? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartObject();
            writer.WriteString("type", nameof(GeometryType.MultiPoint));
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