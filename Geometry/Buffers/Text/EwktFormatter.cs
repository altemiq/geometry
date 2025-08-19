// -----------------------------------------------------------------------
// <copyright file="EwktFormatter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Text;

/// <summary>
/// The extended WKT formatter.
/// </summary>
public static class EwktFormatter
{
    private delegate bool TryFormatValue<in T>(T value, Span<byte> destination, out int bytesWritten);

    /// <summary>
    /// Formats a <see cref="Geometry.Point"/> as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(Geometry.Point value, int srid, Span<byte> destination, out int bytesWritten) =>
        TryFormat(value, srid, WktFormatter.TryFormat, destination, out bytesWritten);

    /// <summary>
    /// Formats a <see cref="Geometry.PointZ"/> as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(Geometry.PointZ value, int srid, Span<byte> destination, out int bytesWritten) =>
        TryFormat(value, srid, WktFormatter.TryFormat, destination, out bytesWritten);

    /// <summary>
    /// Formats a <see cref="Geometry.PointM"/> as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(Geometry.PointM value, int srid, Span<byte> destination, out int bytesWritten) =>
        TryFormat(value, srid, WktFormatter.TryFormat, destination, out bytesWritten);

    /// <summary>
    /// Formats a <see cref="Geometry.PointZM"/> as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(Geometry.PointZM value, int srid, Span<byte> destination, out int bytesWritten) =>
        TryFormat(value, srid, WktFormatter.TryFormat, destination, out bytesWritten);

    /// <summary>
    /// Formats <see cref="Geometry.Point"/> instances as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(IEnumerable<Geometry.Point> value, int srid, Span<byte> destination, out int bytesWritten) =>
        TryFormat(value, srid, WktFormatter.TryFormat, destination, out bytesWritten);

    /// <summary>
    /// Formats <see cref="Geometry.PointZ"/> instances as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(IEnumerable<Geometry.PointZ> value, int srid, Span<byte> destination, out int bytesWritten) =>
        TryFormat(value, srid, WktFormatter.TryFormat, destination, out bytesWritten);

    /// <summary>
    /// Formats <see cref="Geometry.PointM"/> instances as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(IEnumerable<Geometry.PointM> value, int srid, Span<byte> destination, out int bytesWritten) =>
        TryFormat(value, srid, WktFormatter.TryFormat, destination, out bytesWritten);

    /// <summary>
    /// Formats <see cref="Geometry.PointZM"/> instances as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(IEnumerable<Geometry.PointZM> value, int srid, Span<byte> destination, out int bytesWritten) =>
        TryFormat(value, srid, WktFormatter.TryFormat, destination, out bytesWritten);

    /// <summary>
    /// Formats a <see cref="Geometry.Polyline"/> as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(Geometry.Polyline<Geometry.Point> value, int srid, Span<byte> destination, out int bytesWritten) =>
        TryFormat(value, srid, WktFormatter.TryFormat, destination, out bytesWritten);

    /// <summary>
    /// Formats a <see cref="Geometry.PolylineZ"/> as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(Geometry.Polyline<Geometry.PointZ> value, int srid, Span<byte> destination, out int bytesWritten) =>
        TryFormat(value, srid, WktFormatter.TryFormat, destination, out bytesWritten);

    /// <summary>
    /// Formats a <see cref="Geometry.PolylineM"/> as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(Geometry.Polyline<Geometry.PointM> value, int srid, Span<byte> destination, out int bytesWritten) =>
        TryFormat(value, srid, WktFormatter.TryFormat, destination, out bytesWritten);

    /// <summary>
    /// Formats a <see cref="Geometry.PolylineZM"/> as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(Geometry.Polyline<Geometry.PointZM> value, int srid, Span<byte> destination, out int bytesWritten) =>
        TryFormat(value, srid, WktFormatter.TryFormat, destination, out bytesWritten);

    /// <summary>
    /// Formats <see cref="Geometry.Polyline"/> instances as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(IEnumerable<Geometry.Polyline<Geometry.Point>> value, int srid, Span<byte> destination, out int bytesWritten) =>
        TryFormat(value, srid, WktFormatter.TryFormat, destination, out bytesWritten);

    /// <summary>
    /// Formats <see cref="Geometry.PolylineZ"/> instances as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(IEnumerable<Geometry.Polyline<Geometry.PointZ>> value, int srid, Span<byte> destination, out int bytesWritten) =>
        TryFormat(value, srid, WktFormatter.TryFormat, destination, out bytesWritten);

    /// <summary>
    /// Formats <see cref="Geometry.PolylineM"/> instances as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(IEnumerable<Geometry.Polyline<Geometry.PointM>> value, int srid, Span<byte> destination, out int bytesWritten) =>
        TryFormat(value, srid, WktFormatter.TryFormat, destination, out bytesWritten);

    /// <summary>
    /// Formats <see cref="Geometry.PolylineZM"/> instances as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(IEnumerable<Geometry.Polyline<Geometry.PointZM>> value, int srid, Span<byte> destination, out int bytesWritten) =>
        TryFormat(value, srid, WktFormatter.TryFormat, destination, out bytesWritten);

    /// <summary>
    /// Formats a <see cref="Geometry.Polygon"/> as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(Geometry.Polygon<Geometry.Point> value, int srid, Span<byte> destination, out int bytesWritten) =>
        TryFormat(value, srid, WktFormatter.TryFormat, destination, out bytesWritten);

    /// <summary>
    /// Formats a <see cref="Geometry.PolygonZ"/> as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(Geometry.Polygon<Geometry.PointZ> value, int srid, Span<byte> destination, out int bytesWritten) =>
        TryFormat(value, srid, WktFormatter.TryFormat, destination, out bytesWritten);

    /// <summary>
    /// Formats a <see cref="Geometry.PolygonM"/> as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(Geometry.Polygon<Geometry.PointM> value, int srid, Span<byte> destination, out int bytesWritten) =>
        TryFormat(value, srid, WktFormatter.TryFormat, destination, out bytesWritten);

    /// <summary>
    /// Formats a <see cref="Geometry.PolygonZM"/> as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(Geometry.Polygon<Geometry.PointZM> value, int srid, Span<byte> destination, out int bytesWritten) =>
        TryFormat(value, srid, WktFormatter.TryFormat, destination, out bytesWritten);

    /// <summary>
    /// Formats <see cref="Geometry.Polygon"/> instances as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(IEnumerable<Geometry.Polygon<Geometry.Point>> value, int srid, Span<byte> destination, out int bytesWritten) =>
        TryFormat(value, srid, WktFormatter.TryFormat, destination, out bytesWritten);

    /// <summary>
    /// Formats <see cref="Geometry.PolygonZ"/> instances as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(IEnumerable<Geometry.Polygon<Geometry.PointZ>> value, int srid, Span<byte> destination, out int bytesWritten) =>
        TryFormat(value, srid, WktFormatter.TryFormat, destination, out bytesWritten);

    /// <summary>
    /// Formats <see cref="Geometry.PolygonM"/> instances as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(IEnumerable<Geometry.Polygon<Geometry.PointM>> value, int srid, Span<byte> destination, out int bytesWritten) =>
        TryFormat(value, srid, WktFormatter.TryFormat, destination, out bytesWritten);

    /// <summary>
    /// Formats <see cref="Geometry.PolygonZM"/> instances as a UTF8 string.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(IEnumerable<Geometry.Polygon<Geometry.PointZM>> value, int srid, Span<byte> destination, out int bytesWritten) =>
        TryFormat(value, srid, WktFormatter.TryFormat, destination, out bytesWritten);

    /// <summary>
    /// Formats the SRID as a UTF8 string.
    /// </summary>
    /// <param name="srid">The SRID value.</param>
    /// <param name="destination">The buffer to write the UTF8-formatted value to.</param>
    /// <param name="bytesWritten">When the method returns, contains the length of the formatted text in bytes.</param>
    /// <returns><see langword="true"/> if the formatting operation succeeds; <see langword="false"/> if <paramref name="destination"/> is too small.</returns>
    public static bool TryFormat(int srid, Span<byte> destination, out int bytesWritten)
    {
        destination[0] = (byte)'S';
        destination[1] = (byte)'R';
        destination[2] = (byte)'I';
        destination[3] = (byte)'D';
        destination[4] = (byte)'=';

        bytesWritten = 5;
        destination = destination[bytesWritten..];
        if (!System.Buffers.Text.Utf8Formatter.TryFormat(srid, destination, out var sridBytesWritten))
        {
            bytesWritten = 0;
            return false;
        }

        destination[sridBytesWritten] = (byte)';';
        bytesWritten += sridBytesWritten;
        bytesWritten++;

        return true;
    }

    private static bool TryFormat<T>(T geometry, int srid, TryFormatValue<T> func, Span<byte> destination, out int bytesWritten)
    {
        if (!TryFormat(srid, destination, out bytesWritten))
        {
            return false;
        }

        destination = destination[bytesWritten..];

        if (!func(geometry, destination, out var geometryBytesWritten))
        {
            bytesWritten = 0;
            return false;
        }

        bytesWritten += geometryBytesWritten;
        return true;
    }
}