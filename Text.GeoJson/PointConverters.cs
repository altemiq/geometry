// -----------------------------------------------------------------------
// <copyright file="PointConverters.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The <see cref="Point"/> <see cref="JsonConverter"/>.
/// </summary>
internal sealed class PointConverters
{
    /// <summary>
    /// The <see cref="Point"/> constructor.
    /// </summary>
    /// <param name="coordinates">The coordinates.</param>
    /// <returns>The point.</returns>
    public static Point PointConstructor(IList<double> coordinates) => new(coordinates[0], coordinates[1]);

    /// <summary>
    /// The <see cref="PointZ"/> constructor.
    /// </summary>
    /// <param name="coordinates">The coordinates.</param>
    /// <returns>The point.</returns>
    public static PointZ PointZConstructor(IList<double> coordinates) => new(coordinates[0], coordinates[1], coordinates[2]);

    /// <summary>
    /// The <see cref="PointM"/> constructor.
    /// </summary>
    /// <param name="coordinates">The coordinates.</param>
    /// <returns>The point.</returns>
    public static PointM PointMConstructor(IList<double> coordinates) => new(coordinates[0], coordinates[1], coordinates[2]);

    /// <summary>
    /// The <see cref="PointZM"/> constructor.
    /// </summary>
    /// <param name="coordinates">The coordinates.</param>
    /// <returns>The point.</returns>
    public static PointZM PointZMConstructor(IList<double> coordinates) => new(coordinates[0], coordinates[1], coordinates[2], coordinates[3]);

    /// <summary>
    /// The <see cref="Point"/> deconstructor.
    /// </summary>
    /// <param name="point">The point.</param>
    /// <returns>The coordinates.</returns>
    public static IList<double> PointDeconstructor(Point point) => [point.X, point.Y];

    /// <summary>
    /// The <see cref="PointZ"/> deconstructor.
    /// </summary>
    /// <param name="point">The point.</param>
    /// <returns>The coordinates.</returns>
    public static IList<double> PointZDeconstructor(PointZ point) => [point.X, point.Y, point.Z];

    /// <summary>
    /// The <see cref="PointM"/> deconstructor.
    /// </summary>
    /// <param name="point">The point.</param>
    /// <returns>The coordinates.</returns>
    public static IList<double> PointMDeconstructor(PointM point) => [point.X, point.Y, point.Measurement];

    /// <summary>
    /// The <see cref="PointZM"/> deconstructor.
    /// </summary>
    /// <param name="point">The point.</param>
    /// <returns>The coordinates.</returns>
    public static IList<double> PointZMDeconstructor(PointZM point) => [point.X, point.Y, point.Z, point.Measurement];

    /// <summary>
    /// The <see cref="Point"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class PointConverter() : PointConverter<Point>(PointConstructor, PointDeconstructor);

    /// <summary>
    /// The <see cref="PointZ"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class PointZConverter() : PointConverter<PointZ>(PointZConstructor, PointZDeconstructor);

    /// <summary>
    /// The <see cref="PointM"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class PointMConverter() : PointConverter<PointM>(PointMConstructor, PointMDeconstructor);

    /// <summary>
    /// The <see cref="PointZM"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class PointZMConverter() : PointConverter<PointZM>(PointZMConstructor, PointZMDeconstructor);

    /// <summary>
    /// The <see cref="Point"/> converter.
    /// </summary>
    /// <typeparam name="TPoint">The type of point.</typeparam>
    /// <param name="createPoint">The create point function.</param>
    /// <param name="getCoordinates">The function to get coordinates from a point.</param>
    public abstract class PointConverter<TPoint>(Func<IList<double>, TPoint> createPoint, Func<TPoint, IList<double>> getCoordinates) : JsonConverter<TPoint?>
    {
        /// <inheritdoc/>
        public override TPoint? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.Null)
            {
                return default;
            }

            _ = reader.ReadTo(JsonTokenType.PropertyName);

            TPoint? coordinates = default;
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
                    if (type is not GeometryType.Point)
                    {
                        throw new InvalidOperationException();
                    }
                }
                else if (string.Equals(propertyName, nameof(coordinates), StringComparison.Ordinal))
                {
                    List<double> items = [];
                    _ = reader.ReadTo(JsonTokenType.StartArray);
                    _ = reader.Read();
                    while (reader.TokenType is not JsonTokenType.EndArray)
                    {
                        items.Add(reader.GetDouble());
                        _ = reader.Read();
                    }

                    coordinates = createPoint(items);
                }
                else
                {
                    reader.Skip();
                }

                _ = reader.Read();
            }

            return coordinates is not null
                ? coordinates
                : throw new InvalidOperationException();
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, TPoint? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartObject();
            writer.WriteString("type", nameof(GeometryType.Point));
            writer.WriteStartArray("coordinates");

            foreach (var coordinate in getCoordinates(value))
            {
                writer.WriteNumberValue(coordinate);
            }

            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}