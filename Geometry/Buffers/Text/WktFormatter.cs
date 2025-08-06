// -----------------------------------------------------------------------
// <copyright file="WktFormatter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Text;

/// <summary>
/// The WKT formatter.
/// </summary>
public static class WktFormatter
{
    private const byte OpenBracket = (byte)'(';

    private const byte CloseBracket = (byte)')';

    private const byte Comma = (byte)',';

    private const byte Space = (byte)' ';

    private delegate bool TryFormatValue<in T>(Span<byte> destination, T value, out int bytesWritten);

    /// <summary>
    /// Formats a <see cref="Geometry.Point"/> as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(Geometry.Point value, Span<byte> destination, out int bytesWritten) => TryFormat(destination, string.Empty, value, static p => p.IsEmpty, TryFormatWithoutType, out bytesWritten);

    /// <summary>
    /// Formats a <see cref="Geometry.PointZ"/> as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(Geometry.PointZ value, Span<byte> destination, out int bytesWritten) => TryFormat(destination, "Z", value, static p => p.IsEmpty, TryFormatWithoutType, out bytesWritten);

    /// <summary>
    /// Formats a <see cref="Geometry.PointM"/> as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(Geometry.PointM value, Span<byte> destination, out int bytesWritten) => TryFormat(destination, "M", value, static p => p.IsEmpty, TryFormatWithoutType, out bytesWritten);

    /// <summary>
    /// Formats a <see cref="Geometry.PointZM"/> as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(Geometry.PointZM value, Span<byte> destination, out int bytesWritten) => TryFormat(destination, "ZM", value, static p => p.IsEmpty, TryFormatWithoutType, out bytesWritten);

    /// <summary>
    /// Formats <see cref="Geometry.Point"/> instances as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(IEnumerable<Geometry.Point> value, Span<byte> destination, out int bytesWritten) => TryFormat(destination, string.Empty, value, TryFormatWithoutBrackets, out bytesWritten);

    /// <summary>
    /// Formats <see cref="Geometry.PointZ"/> instances as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(IEnumerable<Geometry.PointZ> value, Span<byte> destination, out int bytesWritten) => TryFormat(destination, "Z", value, TryFormatWithoutBrackets, out bytesWritten);

    /// <summary>
    /// Formats <see cref="Geometry.PointM"/> instances as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(IEnumerable<Geometry.PointM> value, Span<byte> destination, out int bytesWritten) => TryFormat(destination, "M", value, TryFormatWithoutBrackets, out bytesWritten);

    /// <summary>
    /// Formats <see cref="Geometry.PointZM"/> instances as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(IEnumerable<Geometry.PointZM> value, Span<byte> destination, out int bytesWritten) => TryFormat(destination, "ZM", value, TryFormatWithoutBrackets, out bytesWritten);

    /// <summary>
    /// Formats a <see cref="Geometry.Polyline"/> as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(Geometry.Polyline<Geometry.Point> value, Span<byte> destination, out int bytesWritten) => TryFormat(destination, string.Empty, value, TryFormatWithoutBrackets, out bytesWritten);

    /// <summary>
    /// Formats a <see cref="Geometry.PolylineZ"/> as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(Geometry.Polyline<Geometry.PointZ> value, Span<byte> destination, out int bytesWritten) => TryFormat(destination, string.Empty, value, TryFormatWithoutBrackets, out bytesWritten);

    /// <summary>
    /// Formats a <see cref="Geometry.PolylineM"/> as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(Geometry.Polyline<Geometry.PointM> value, Span<byte> destination, out int bytesWritten) => TryFormat(destination, string.Empty, value, TryFormatWithoutBrackets, out bytesWritten);

    /// <summary>
    /// Formats a <see cref="Geometry.PolylineZM"/> as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(Geometry.Polyline<Geometry.PointZM> value, Span<byte> destination, out int bytesWritten) => TryFormat(destination, string.Empty, value, TryFormatWithoutBrackets, out bytesWritten);

    /// <summary>
    /// Formats <see cref="Geometry.Polyline"/> instances as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(IEnumerable<Geometry.Polyline<Geometry.Point>> value, Span<byte> destination, out int bytesWritten) => TryFormat(destination, string.Empty, value, TryFormatWithoutType, out bytesWritten);

    /// <summary>
    /// Formats <see cref="Geometry.PolylineZ"/> instances as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(IEnumerable<Geometry.Polyline<Geometry.PointZ>> value, Span<byte> destination, out int bytesWritten) => TryFormat(destination, string.Empty, value, TryFormatWithoutType, out bytesWritten);

    /// <summary>
    /// Formats <see cref="Geometry.PolylineM"/> instances as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(IEnumerable<Geometry.Polyline<Geometry.PointM>> value, Span<byte> destination, out int bytesWritten) => TryFormat(destination, string.Empty, value, TryFormatWithoutType, out bytesWritten);

    /// <summary>
    /// Formats <see cref="Geometry.PolylineZM"/> instances as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(IEnumerable<Geometry.Polyline<Geometry.PointZM>> value, Span<byte> destination, out int bytesWritten) => TryFormat(destination, string.Empty, value, TryFormatWithoutType, out bytesWritten);

    /// <summary>
    /// Formats a <see cref="Geometry.Polygon"/> as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(Geometry.Polygon<Geometry.Point> value, Span<byte> destination, out int bytesWritten) => TryFormat(destination, string.Empty, value, TryFormatWithoutType, out bytesWritten);

    /// <summary>
    /// Formats a <see cref="Geometry.PolygonZ"/> as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(Geometry.Polygon<Geometry.PointZ> value, Span<byte> destination, out int bytesWritten) => TryFormat(destination, string.Empty, value, TryFormatWithoutType, out bytesWritten);

    /// <summary>
    /// Formats a <see cref="Geometry.PolygonM"/> as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(Geometry.Polygon<Geometry.PointM> value, Span<byte> destination, out int bytesWritten) => TryFormat(destination, string.Empty, value, TryFormatWithoutType, out bytesWritten);

    /// <summary>
    /// Formats a <see cref="Geometry.PolygonZM"/> as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(Geometry.Polygon<Geometry.PointZM> value, Span<byte> destination, out int bytesWritten) => TryFormat(destination, string.Empty, value, TryFormatWithoutType, out bytesWritten);

    /// <summary>
    /// Formats <see cref="Geometry.Polygon"/> instances as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(IEnumerable<Geometry.Polygon<Geometry.Point>> value, Span<byte> destination, out int bytesWritten) => TryFormat(destination, string.Empty, value, TryFormatWithoutType, out bytesWritten);

    /// <summary>
    /// Formats <see cref="Geometry.PolygonZ"/> instances as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(IEnumerable<Geometry.Polygon<Geometry.PointZ>> value, Span<byte> destination, out int bytesWritten) => TryFormat(destination, string.Empty, value, TryFormatWithoutType, out bytesWritten);

    /// <summary>
    /// Formats <see cref="Geometry.PolygonM"/> instances as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(IEnumerable<Geometry.Polygon<Geometry.PointM>> value, Span<byte> destination, out int bytesWritten) => TryFormat(destination, string.Empty, value, TryFormatWithoutType, out bytesWritten);

    /// <summary>
    /// Formats <see cref="Geometry.PolygonZM"/> instances as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(IEnumerable<Geometry.Polygon<Geometry.PointZM>> value, Span<byte> destination, out int bytesWritten) => TryFormat(destination, string.Empty, value, TryFormatWithoutType, out bytesWritten);

    private static bool TryFormatWithoutType<T>(Span<byte> destination, T item, TryFormatValue<T> write, out int total)
    {
        total = 0;
        destination[0] = OpenBracket;
        total++;
        if (!write(destination[1..], item, out var count))
        {
            total += count;
            return false;
        }

        total += count;
        destination[total] = CloseBracket;
        total++;
        return true;
    }

    private static bool TryFormatWithoutType<T>(Span<byte> destination, IEnumerable<T> items, TryFormatValue<T> write, out int total)
    {
        destination[0] = OpenBracket;
        destination = destination[1..];
        total = 1;

        bool first = true;
        foreach (var item in items)
        {
            if (!first)
            {
                destination[0] = Comma;
                destination[1] = Space;
                destination = destination[2..];
                total += 2;
            }

            first = false;
            if (!write(destination, item, out var count))
            {
                total += count;
                return false;
            }

            destination = destination[count..];
            total += count;
        }

        destination[0] = CloseBracket;
        total++;
        return true;
    }

    private static bool TryFormatWithoutType<T>(Span<byte> destination, IEnumerable<IEnumerable<T>> parts, TryFormatValue<T> write, out int total)
    {
        destination[0] = OpenBracket;
        destination = destination[1..];
        total = 1;

        bool first = true;
        foreach (var part in parts)
        {
            if (!first)
            {
                destination[0] = Comma;
                destination[1] = Space;
                destination = destination[2..];
                total += 2;
            }

            first = false;
            if (!TryFormatWithoutType(destination, part, write, out int count))
            {
                total += count;
                return false;
            }

            destination = destination[count..];
            total += count;
        }

        destination[0] = CloseBracket;
        total++;
        return true;
    }

    private static bool TryFormatWithoutType(Span<byte> destination, Geometry.Point point, out int bytesWritten) => TryFormatWithoutType(destination, point, TryFormatWithoutBrackets, out bytesWritten);

    private static bool TryFormatWithoutType(Span<byte> destination, Geometry.PointZ point, out int bytesWritten) => TryFormatWithoutType(destination, point, TryFormatWithoutBrackets, out bytesWritten);

    private static bool TryFormatWithoutType(Span<byte> destination, Geometry.PointM point, out int bytesWritten) => TryFormatWithoutType(destination, point, TryFormatWithoutBrackets, out bytesWritten);

    private static bool TryFormatWithoutType(Span<byte> destination, Geometry.PointZM point, out int bytesWritten) => TryFormatWithoutType(destination, point, TryFormatWithoutBrackets, out bytesWritten);

    private static bool TryFormatWithoutType(Span<byte> destination, IEnumerable<Geometry.Point> points, out int bytesWritten) => TryFormatWithoutType(destination, points, TryFormatWithoutBrackets, out bytesWritten);

    private static bool TryFormatWithoutType(Span<byte> destination, IEnumerable<Geometry.PointZ> points, out int bytesWritten) => TryFormatWithoutType(destination, points, TryFormatWithoutBrackets, out bytesWritten);

    private static bool TryFormatWithoutType(Span<byte> destination, IEnumerable<Geometry.PointM> points, out int bytesWritten) => TryFormatWithoutType(destination, points, TryFormatWithoutBrackets, out bytesWritten);

    private static bool TryFormatWithoutType(Span<byte> destination, IEnumerable<Geometry.PointZM> points, out int bytesWritten) => TryFormatWithoutType(destination, points, TryFormatWithoutBrackets, out bytesWritten);

    private static bool TryFormatWithoutType(Span<byte> destination, IEnumerable<IEnumerable<Geometry.Point>> points, out int bytesWritten) => TryFormatWithoutType(destination, points, TryFormatWithoutBrackets, out bytesWritten);

    private static bool TryFormatWithoutType(Span<byte> destination, IEnumerable<IEnumerable<Geometry.PointZ>> points, out int bytesWritten) => TryFormatWithoutType(destination, points, TryFormatWithoutBrackets, out bytesWritten);

    private static bool TryFormatWithoutType(Span<byte> destination, IEnumerable<IEnumerable<Geometry.PointM>> points, out int bytesWritten) => TryFormatWithoutType(destination, points, TryFormatWithoutBrackets, out bytesWritten);

    private static bool TryFormatWithoutType(Span<byte> destination, IEnumerable<IEnumerable<Geometry.PointZM>> points, out int bytesWritten) => TryFormatWithoutType(destination, points, TryFormatWithoutBrackets, out bytesWritten);

    private static bool TryFormatWithoutBrackets(Span<byte> destination, Geometry.Point point, out int bytesWritten)
    {
        if (!System.Buffers.Text.Utf8Formatter.TryFormat(point.X, destination, out var bw))
        {
            bytesWritten = bw;
            return false;
        }

        var total = bw;
        destination = destination[bw..];
        destination[0] = (byte)' ';
        destination = destination[1..];
        total++;

        if (!System.Buffers.Text.Utf8Formatter.TryFormat(point.Y, destination, out bw))
        {
            bytesWritten = total + bw;
            return false;
        }

        bytesWritten = total + bw;
        return true;
    }

    private static bool TryFormatWithoutBrackets(Span<byte> destination, Geometry.PointZ point, out int bytesWritten)
    {
        if (!System.Buffers.Text.Utf8Formatter.TryFormat(point.X, destination, out var bw))
        {
            bytesWritten = bw;
            return false;
        }

        var total = bw;
        destination = destination[bw..];
        destination[0] = (byte)' ';
        destination = destination[1..];
        total++;

        if (!System.Buffers.Text.Utf8Formatter.TryFormat(point.Y, destination, out bw))
        {
            bytesWritten = total + bw;
            return false;
        }

        total += bw;
        destination = destination[bw..];
        destination[0] = (byte)' ';
        destination = destination[1..];
        total++;

        if (!System.Buffers.Text.Utf8Formatter.TryFormat(point.Z, destination, out bw))
        {
            bytesWritten = total + bw;
            return false;
        }

        bytesWritten = total + bw;
        return true;
    }

    private static bool TryFormatWithoutBrackets(Span<byte> destination, Geometry.PointM point, out int bytesWritten)
    {
        if (!System.Buffers.Text.Utf8Formatter.TryFormat(point.X, destination, out var bw))
        {
            bytesWritten = bw;
            return false;
        }

        var total = bw;
        destination = destination[bw..];
        destination[0] = (byte)' ';
        destination = destination[1..];
        total++;

        if (!System.Buffers.Text.Utf8Formatter.TryFormat(point.Y, destination, out bw))
        {
            bytesWritten = total + bw;
            return false;
        }

        total += bw;
        destination = destination[bw..];
        destination[0] = (byte)' ';
        destination = destination[1..];
        total++;

        if (!System.Buffers.Text.Utf8Formatter.TryFormat(point.Measurement, destination, out bw))
        {
            bytesWritten = total + bw;
            return false;
        }

        bytesWritten = total + bw;
        return true;
    }

    private static bool TryFormatWithoutBrackets(Span<byte> destination, Geometry.PointZM point, out int bytesWritten)
    {
        if (!System.Buffers.Text.Utf8Formatter.TryFormat(point.X, destination, out var bw))
        {
            bytesWritten = bw;
            return false;
        }

        var total = bw;
        destination = destination[bw..];
        destination[0] = (byte)' ';
        destination = destination[1..];
        total++;

        if (!System.Buffers.Text.Utf8Formatter.TryFormat(point.Y, destination, out bw))
        {
            bytesWritten = total + bw;
            return false;
        }

        total += bw;
        destination = destination[bw..];
        destination[0] = (byte)' ';
        destination = destination[1..];
        total++;

        if (!System.Buffers.Text.Utf8Formatter.TryFormat(point.Z, destination, out bw))
        {
            bytesWritten = total + bw;
            return false;
        }

        total += bw;
        destination = destination[bw..];
        destination[0] = (byte)' ';
        destination = destination[1..];
        total++;

        if (!System.Buffers.Text.Utf8Formatter.TryFormat(point.Measurement, destination, out bw))
        {
            bytesWritten = total + bw;
            return false;
        }

        bytesWritten = total + bw;
        return true;
    }

    private static bool TryFormat<T>(Span<byte> destination, string type, T point, Func<T, bool> isEmpty, TryFormatValue<T> write, out int bytesWritten) => TryFormat(destination, "POINT", type, point, isEmpty, write, out bytesWritten);

    private static bool TryFormat<T>(Span<byte> destination, string type, IEnumerable<T> points, TryFormatValue<T> write, out int bytesWritten) => TryFormat(destination, "MULTIPOINT", type, points, write, out bytesWritten);

    private static bool TryFormat<T>(Span<byte> destination, string type, Geometry.Polyline<T> points, TryFormatValue<T> write, out int bytesWritten) => TryFormat(destination, geometry: "LINESTRING", type, points, write, out bytesWritten);

    private static bool TryFormat<T>(Span<byte> destination, string type, IEnumerable<Geometry.Polyline<T>> points, TryFormatValue<Geometry.Polyline<T>> write, out int bytesWritten) => TryFormat(destination, "MULTILINESTRING", type, points, write, out bytesWritten);

    private static bool TryFormat<T>(Span<byte> destination, string type, Geometry.Polygon<T> points, TryFormatValue<Geometry.LinearRing<T>> write, out int bytesWritten)
        where T : struct => TryFormat(destination, "POLYGON", type, points, write, out bytesWritten);

    private static bool TryFormat<T>(Span<byte> destination, string type, IEnumerable<Geometry.Polygon<T>> points, TryFormatValue<Geometry.Polygon<T>> write, out int bytesWritten)
        where T : struct => TryFormat(destination, "MULTIPOLYGON", type, points, write, out bytesWritten);

    private static bool TryFormat<T>(Span<byte> destination, string geometry, string type, T element, Func<T, bool> isEmpty, TryFormatValue<T> write, out int total)
    {
        total = CopyTo(geometry, ref destination);
        destination[0] = Space;
        destination = destination[1..];
        total++;

        if (type.Length is not 0)
        {
            total += CopyTo(type, ref destination);
            destination[0] = Space;
            destination = destination[1..];
            total++;
        }

        if (isEmpty(element))
        {
            total += CopyTo("EMPTY", ref destination);
            return true;
        }

        if (write(destination, element, out var count))
        {
            total += count;
            return true;
        }

        total += count;
        return false;
    }

    private static bool TryFormat<T>(Span<byte> destination, string geometry, string type, IEnumerable<T> elements, TryFormatValue<T> write, out int total)
    {
        total = CopyTo(geometry, ref destination);
        destination[0] = Space;
        destination = destination[1..];
        total++;

        if (type.Length is not 0)
        {
            total += CopyTo(type, ref destination);
            destination[0] = Space;
            destination = destination[1..];
            total++;
        }

        using var enumerator = elements.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            total += CopyTo("EMPTY", ref destination);
            return true;
        }

        destination[0] = OpenBracket;
        destination = destination[1..];
        total++;

        bool first = true;
        do
        {
            if (!first)
            {
                destination[0] = Comma;
                destination[1] = Space;
                destination = destination[2..];
                total += 2;
            }

            first = false;
            if (!write(destination, enumerator.Current, out var count))
            {
                total += count;
                return false;
            }

            destination = destination[count..];
            total += count;
        }
        while (enumerator.MoveNext());

        destination[0] = CloseBracket;
        total++;
        return true;
    }

    private static int CopyTo(string source, ref Span<byte> destination)
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        var count = System.Text.Encoding.UTF8.GetBytes(source, destination);
        destination = destination.Slice(count);
        return count;
#else
        if (source.Length is 0)
        {
            return 0;
        }

        for (int i = 0; i < source.Length; i++)
        {
            destination[i] = (byte)source[i];
        }

        destination = destination[source.Length..];
        return source.Length;
#endif
    }
}