// -----------------------------------------------------------------------
// <copyright file="PointConverters.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The <see cref="Geometry.Point"/> <see cref="JsonConverter"/>.
/// </summary>
internal sealed class PointConverters
{
    /// <summary>
    /// The <see cref="Geometry.Point"/> constructor.
    /// </summary>
    /// <param name="coordinates">The coordinates.</param>
    /// <returns>The point.</returns>
    public static Geometry.Point PointConstructor(IList<double> coordinates) => new(coordinates[0], coordinates[1]);

    /// <summary>
    /// The <see cref="Geometry.PointZ"/> constructor.
    /// </summary>
    /// <param name="coordinates">The coordinates.</param>
    /// <returns>The point.</returns>
    public static Geometry.PointZ PointZConstructor(IList<double> coordinates) => new(coordinates[0], coordinates[1], coordinates[2]);

    /// <summary>
    /// The <see cref="Geometry.PointM"/> constructor.
    /// </summary>
    /// <param name="coordinates">The coordinates.</param>
    /// <returns>The point.</returns>
    public static Geometry.PointM PointMConstructor(IList<double> coordinates) => new(coordinates[0], coordinates[1], coordinates[2]);

    /// <summary>
    /// The <see cref="Geometry.PointZM"/> constructor.
    /// </summary>
    /// <param name="coordinates">The coordinates.</param>
    /// <returns>The point.</returns>
    public static Geometry.PointZM PointZMConstructor(IList<double> coordinates) => new(coordinates[0], coordinates[1], coordinates[2], coordinates[3]);

    /// <summary>
    /// The <see cref="Geometry.Point"/> deconstructor.
    /// </summary>
    /// <param name="point">The point.</param>
    /// <returns>The coordinates.</returns>
    public static IList<double> PointDeconstructor(Geometry.Point point) => [point.X, point.Y];

    /// <summary>
    /// The <see cref="Geometry.PointZ"/> deconstructor.
    /// </summary>
    /// <param name="point">The point.</param>
    /// <returns>The coordinates.</returns>
    public static IList<double> PointZDeconstructor(Geometry.PointZ point) => [point.X, point.Y, point.Z];

    /// <summary>
    /// The <see cref="Geometry.PointM"/> deconstructor.
    /// </summary>
    /// <param name="point">The point.</param>
    /// <returns>The coordinates.</returns>
    public static IList<double> PointMDeconstructor(Geometry.PointM point) => [point.X, point.Y, point.Measurement];

    /// <summary>
    /// The <see cref="Geometry.PointZM"/> deconstructor.
    /// </summary>
    /// <param name="point">The point.</param>
    /// <returns>The coordinates.</returns>
    public static IList<double> PointZMDeconstructor(Geometry.PointZM point) => [point.X, point.Y, point.Z, point.Measurement];

    /// <summary>
    /// The <see cref="Geometry.Point"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class PointConverter() : PointConverter<Geometry.Point>(PointConstructor, PointDeconstructor);

    /// <summary>
    /// The <see cref="Geometry.PointZ"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class PointZConverter() : PointConverter<Geometry.PointZ>(PointZConstructor, PointZDeconstructor);

    /// <summary>
    /// The <see cref="Geometry.PointM"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class PointMConverter() : PointConverter<Geometry.PointM>(PointMConstructor, PointMDeconstructor);

    /// <summary>
    /// The <see cref="Geometry.PointZM"/> <see cref="JsonConverter"/>.
    /// </summary>
    public sealed class PointZMConverter() : PointConverter<Geometry.PointZM>(PointZMConstructor, PointZMDeconstructor);

    /// <summary>
    /// The <see cref="Geometry.Point"/> converter.
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
                    if (reader.GetString() is { } value
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
                        && Enum.Parse<GeometryType>(value)
#else
                        && (GeometryType)Enum.Parse(typeof(GeometryType), value)
#endif
                            is not GeometryType.Point)
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