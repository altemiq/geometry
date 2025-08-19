// -----------------------------------------------------------------------
// <copyright file="EwktParser.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Text;

/// <summary>
/// The Extended WKT parser.
/// </summary>
public static class EwktParser
{
    private delegate bool TryParseDelegate<T>(ReadOnlySpan<byte> source, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out T? value, out int bytesConsumed)
        where T : Geometry.IGeometry;

    /// <summary>
    /// Parses a <see cref="Geometry.Point"/> at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out int srid, out Geometry.Point value, out int bytesConsumed) =>
        TryParse(source, out srid, WktParser.TryParse, out value, out bytesConsumed);

    /// <summary>
    /// Parses a <see cref="Geometry.PointZ"/> at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out int srid, out Geometry.PointZ value, out int bytesConsumed) =>
        TryParse(source, out srid, WktParser.TryParse, out value, out bytesConsumed);

    /// <summary>
    /// Parses a <see cref="Geometry.PointM"/> at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out int srid, out Geometry.PointM value, out int bytesConsumed) =>
        TryParse(source, out srid, WktParser.TryParse, out value, out bytesConsumed);

    /// <summary>
    /// Parses a <see cref="Geometry.PointZM"/> at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out int srid, out Geometry.PointZM value, out int bytesConsumed) =>
        TryParse(source, out srid, WktParser.TryParse, out value, out bytesConsumed);

    /// <summary>
    /// Parses <see cref="Geometry.Point"/> instances at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out int srid, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.IMultiGeometry<Geometry.Point>? value, out int bytesConsumed) =>
        TryParse(source, out srid, WktParser.TryParse, out value, out bytesConsumed);

    /// <summary>
    /// Parses <see cref="Geometry.PointZ"/> instances at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out int srid, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.IMultiGeometry<Geometry.PointZ>? value, out int bytesConsumed) =>
        TryParse(source, out srid, WktParser.TryParse, out value, out bytesConsumed);

    /// <summary>
    /// Parses <see cref="Geometry.PointM"/> instances at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out int srid, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.IMultiGeometry<Geometry.PointM>? value, out int bytesConsumed) =>
        TryParse(source, out srid, WktParser.TryParse, out value, out bytesConsumed);

    /// <summary>
    /// Parses <see cref="Geometry.PointZM"/> instances at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out int srid, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.IMultiGeometry<Geometry.PointZM>? value, out int bytesConsumed) =>
        TryParse(source, out srid, WktParser.TryParse, out value, out bytesConsumed);

    /// <summary>
    /// Parses a <see cref="Geometry.Polyline"/> at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out int srid, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.Polyline? value, out int bytesConsumed) =>
        TryParse(source, out srid, WktParser.TryParse, out value, out bytesConsumed);

    /// <summary>
    /// Parses a <see cref="Geometry.PolylineZ"/> at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out int srid, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.PolylineZ? value, out int bytesConsumed) =>
        TryParse(source, out srid, WktParser.TryParse, out value, out bytesConsumed);

    /// <summary>
    /// Parses a <see cref="Geometry.PolylineM"/> at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out int srid, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.PolylineM? value, out int bytesConsumed) =>
        TryParse(source, out srid, WktParser.TryParse, out value, out bytesConsumed);

    /// <summary>
    /// Parses a <see cref="Geometry.PolylineZM"/> at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out int srid, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.PolylineZM? value, out int bytesConsumed) =>
        TryParse(source, out srid, WktParser.TryParse, out value, out bytesConsumed);

    /// <summary>
    /// Parses <see cref="Geometry.Polyline"/> instances at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out int srid, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.IMultiGeometry<Geometry.Polyline>? value, out int bytesConsumed) =>
        TryParse(source, out srid, WktParser.TryParse, out value, out bytesConsumed);

    /// <summary>
    /// Parses <see cref="Geometry.PolylineZ"/> instances at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out int srid, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.IMultiGeometry<Geometry.PolylineZ>? value, out int bytesConsumed) =>
        TryParse(source, out srid, WktParser.TryParse, out value, out bytesConsumed);

    /// <summary>
    /// Parses <see cref="Geometry.PolylineM"/> instances at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out int srid, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.IMultiGeometry<Geometry.PolylineM>? value, out int bytesConsumed) =>
        TryParse(source, out srid, WktParser.TryParse, out value, out bytesConsumed);

    /// <summary>
    /// Parses <see cref="Geometry.PolylineZM"/> instances at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out int srid, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.IMultiGeometry<Geometry.PolylineZM>? value, out int bytesConsumed) =>
        TryParse(source, out srid, WktParser.TryParse, out value, out bytesConsumed);

    /// <summary>
    /// Parses a <see cref="Geometry.Polygon"/> at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out int srid, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.Polygon? value, out int bytesConsumed) =>
        TryParse(source, out srid, WktParser.TryParse, out value, out bytesConsumed);

    /// <summary>
    /// Parses a <see cref="Geometry.PolygonZ"/> at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out int srid, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.PolygonZ? value, out int bytesConsumed) =>
        TryParse(source, out srid, WktParser.TryParse, out value, out bytesConsumed);

    /// <summary>
    /// Parses a <see cref="Geometry.PolygonM"/> at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out int srid, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.PolygonM? value, out int bytesConsumed) =>
        TryParse(source, out srid, WktParser.TryParse, out value, out bytesConsumed);

    /// <summary>
    /// Parses a <see cref="Geometry.PolygonZM"/> at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out int srid, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.PolygonZM? value, out int bytesConsumed) =>
        TryParse(source, out srid, WktParser.TryParse, out value, out bytesConsumed);

    /// <summary>
    /// Parses <see cref="Geometry.Polygon"/> instances at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out int srid, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.IMultiGeometry<Geometry.Polygon>? value, out int bytesConsumed) =>
        TryParse(source, out srid, WktParser.TryParse, out value, out bytesConsumed);

    /// <summary>
    /// Parses <see cref="Geometry.PolygonZ"/> instances at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out int srid, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.IMultiGeometry<Geometry.PolygonZ>? value, out int bytesConsumed) =>
        TryParse(source, out srid, WktParser.TryParse, out value, out bytesConsumed);

    /// <summary>
    /// Parses <see cref="Geometry.PolygonM"/> instances at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out int srid, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.IMultiGeometry<Geometry.PolygonM>? value, out int bytesConsumed) =>
        TryParse(source, out srid, WktParser.TryParse, out value, out bytesConsumed);

    /// <summary>
    /// Parses <see cref="Geometry.PolygonZM"/> instances at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out int srid, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.IMultiGeometry<Geometry.PolygonZM>? value, out int bytesConsumed) =>
        TryParse(source, out srid, WktParser.TryParse, out value, out bytesConsumed);

    /// <summary>
    /// Parses a geometry at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="srid">The SRID value.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out int srid, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.IGeometry? value, out int bytesConsumed) =>
        TryParse(source, out srid, WktParser.TryParse, out value, out bytesConsumed);

    private static bool TryParse<T>(ReadOnlySpan<byte> source, out int srid, TryParseDelegate<T> func, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out T? geometry, out int bytesConsumed)
        where T : Geometry.IGeometry
    {
        // parse the SRID
        if (!TryGetSrid(source, out srid, out var sridBytesConsumed))
        {
            geometry = default;
            bytesConsumed = 0;
            return false;
        }

        source = source[sridBytesConsumed..];

        if (func(source, out geometry, out bytesConsumed))
        {
            bytesConsumed += sridBytesConsumed;
            return true;
        }

        return false;

        static bool TryGetSrid(ReadOnlySpan<byte> span, out int srid, out int bytesConsumed)
        {
#pragma warning disable SA1008
            srid = 0;
            bytesConsumed = 0;
            return (span.IndexOf((byte)'='), span.IndexOf((byte)';')) switch
            {
                (>= 0, >= 0) indexes => System.Buffers.Text.Utf8Parser.TryParse(span[(indexes.Item1 + 1)..indexes.Item2], out srid, out bytesConsumed),
                _ => false,
            };
#pragma warning restore SA1008
        }
    }
}