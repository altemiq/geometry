// -----------------------------------------------------------------------
// <copyright file="WktParser.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Text;

/// <summary>
/// The WKT parser.
/// </summary>
public static class WktParser
{
    private enum GeometryType
    {
        Point,
        LineString,
        Polygon,
        MultiPoint,
        MultiLineString,
        MultiPolygon,
    }

    /// <summary>
    /// Parses a <see cref="Geometry.Point"/> at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out Geometry.Point value, out int bytesConsumed) =>
        TryParse(source, out value, out bytesConsumed, result => result is (GeometryType.Point, false, false, 2), GetPoint);

    /// <summary>
    /// Parses a <see cref="Geometry.PointZ"/> at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out Geometry.PointZ value, out int bytesConsumed) =>
        TryParse(source, out value, out bytesConsumed, result => result is (GeometryType.Point, true, false, 3), GetPointZ);

    /// <summary>
    /// Parses a <see cref="Geometry.PointM"/> at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out Geometry.PointM value, out int bytesConsumed) =>
        TryParse(source, out value, out bytesConsumed, result => result is (GeometryType.Point, false, true, 3), GetPointM);

    /// <summary>
    /// Parses a <see cref="Geometry.PointZM"/> at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, out Geometry.PointZM value, out int bytesConsumed) =>
        TryParse(source, out value, out bytesConsumed, result => result is (GeometryType.Point, true, true, 4), GetPointZM);

    /// <summary>
    /// Parses <see cref="Geometry.Point"/> instances at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out IReadOnlyCollection<Geometry.Point>? value, out int bytesConsumed) =>
        TryParse(source, out value, out bytesConsumed, result => result is (GeometryType.MultiPoint, false, false, 2), GetPoint);

    /// <summary>
    /// Parses <see cref="Geometry.PointZ"/> instances at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out IReadOnlyCollection<Geometry.PointZ>? value, out int bytesConsumed) =>
        TryParse(source, out value, out bytesConsumed, result => result is (GeometryType.MultiPoint, true, false, 3), GetPointZ);

    /// <summary>
    /// Parses <see cref="Geometry.PointM"/> instances at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out IReadOnlyCollection<Geometry.PointM>? value, out int bytesConsumed) =>
        TryParse(source, out value, out bytesConsumed, result => result is (GeometryType.MultiPoint, false, true, 3), GetPointM);

    /// <summary>
    /// Parses <see cref="Geometry.PointZM"/> instances at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out IReadOnlyCollection<Geometry.PointZM>? value, out int bytesConsumed) =>
        TryParse(source, out value, out bytesConsumed, result => result is (GeometryType.MultiPoint, true, true, 4), GetPointZM);

    /// <summary>
    /// Parses a <see cref="Geometry.Polyline"/> at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.Polyline? value, out int bytesConsumed) =>
        TryParse(source, out value, out bytesConsumed, result => result is (GeometryType.LineString, false, false, 2), GetPoint, GetLineString);

    /// <summary>
    /// Parses a <see cref="Geometry.PolylineZ"/> at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.PolylineZ? value, out int bytesConsumed) =>
        TryParse(source, out value, out bytesConsumed, result => result is (GeometryType.LineString, true, false, 3), GetPointZ, GetLineStringZ);

    /// <summary>
    /// Parses a <see cref="Geometry.PolylineM"/> at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.PolylineM? value, out int bytesConsumed) =>
        TryParse(source, out value, out bytesConsumed, result => result is (GeometryType.LineString, false, true, 3), GetPointM, GetLineStringM);

    /// <summary>
    /// Parses a <see cref="Geometry.PolylineZM"/> at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.PolylineZM? value, out int bytesConsumed) =>
        TryParse(source, out value, out bytesConsumed, result => result is (GeometryType.LineString, true, true, 4), GetPointZM, GetLineStringZM);

    /// <summary>
    /// Parses <see cref="Geometry.Polyline"/> instances at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out IReadOnlyCollection<Geometry.Polyline>? value, out int bytesConsumed) =>
        TryParse(source, out value, out bytesConsumed, result => result is (GeometryType.MultiLineString, false, false, 2), GetPoint, GetLineString);

    /// <summary>
    /// Parses <see cref="Geometry.PolylineZ"/> instances at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out IReadOnlyCollection<Geometry.PolylineZ>? value, out int bytesConsumed) =>
        TryParse(source, out value, out bytesConsumed, result => result is (GeometryType.MultiLineString, true, false, 3), GetPointZ, GetLineStringZ);

    /// <summary>
    /// Parses <see cref="Geometry.PolylineM"/> instances at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out IReadOnlyCollection<Geometry.PolylineM>? value, out int bytesConsumed) =>
        TryParse(source, out value, out bytesConsumed, result => result is (GeometryType.MultiLineString, false, true, 3), GetPointM, GetLineStringM);

    /// <summary>
    /// Parses <see cref="Geometry.PolylineZM"/> instances at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out IReadOnlyCollection<Geometry.PolylineZM>? value, out int bytesConsumed) =>
        TryParse(source, out value, out bytesConsumed, result => result is (GeometryType.MultiLineString, true, true, 4), GetPointZM, GetLineStringZM);

    /// <summary>
    /// Parses a <see cref="Geometry.Polygon"/> at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.Polygon? value, out int bytesConsumed) =>
        TryParse(source, out value, out bytesConsumed, result => result is (GeometryType.Polygon, false, false, 2), GetPoint, GetPolygon);

    /// <summary>
    /// Parses a <see cref="Geometry.PolygonZ"/> at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.PolygonZ? value, out int bytesConsumed) =>
        TryParse(source, out value, out bytesConsumed, result => result is (GeometryType.Polygon, true, false, 3), GetPointZ, GetPolygonZ);

    /// <summary>
    /// Parses a <see cref="Geometry.PolygonM"/> at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.PolygonM? value, out int bytesConsumed) =>
        TryParse(source, out value, out bytesConsumed, result => result is (GeometryType.Polygon, false, true, 3), GetPointM, GetPolygonM);

    /// <summary>
    /// Parses a <see cref="Geometry.PolygonZM"/> at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Geometry.PolygonZM? value, out int bytesConsumed) =>
        TryParse(source, out value, out bytesConsumed, result => result is (GeometryType.Polygon, true, true, 4), GetPointZM, GetPolygonZM);

    /// <summary>
    /// Parses <see cref="Geometry.Polygon"/> instances at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out IReadOnlyCollection<Geometry.Polygon>? value, out int bytesConsumed) =>
        TryParse(source, out value, out bytesConsumed, result => result is (GeometryType.MultiPolygon, false, false, 2), GetPoint, GetPolygon);

    /// <summary>
    /// Parses <see cref="Geometry.PolygonZ"/> instances at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out IReadOnlyCollection<Geometry.PolygonZ>? value, out int bytesConsumed) =>
        TryParse(source, out value, out bytesConsumed, result => result is (GeometryType.MultiPolygon, true, false, 3), GetPointZ, GetPolygonZ);

    /// <summary>
    /// Parses <see cref="Geometry.PolygonM"/> instances at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out IReadOnlyCollection<Geometry.PolygonM>? value, out int bytesConsumed) =>
        TryParse(source, out value, out bytesConsumed, result => result is (GeometryType.MultiPolygon, false, true, 3), GetPointM, GetPolygonM);

    /// <summary>
    /// Parses <see cref="Geometry.PolygonZM"/> instances at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out IReadOnlyCollection<Geometry.PolygonZM>? value, out int bytesConsumed) =>
        TryParse(source, out value, out bytesConsumed, result => result is (GeometryType.MultiPolygon, true, true, 4), GetPointZM, GetPolygonZM);

    /// <summary>
    /// Parses a geometry at the start of a string.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <param name="value">When the method returns, contains the value parsed from <paramref name="source"/>, if the parsing operation succeeded.</param>
    /// <param name="bytesConsumed">If the parsing operation was successful, contains the length in bytes of the parsed substring when the method returns. If the method fails, <paramref name="bytesConsumed"/> is set to 0.</param>
    /// <returns><see langword="true"/> for success; <see langword="false"/> if the string was not syntactically valid or an overflow or underflow occurred.</returns>
    public static bool TryParse(ReadOnlySpan<byte> source, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out object? value, out int bytesConsumed)
    {
        if (TryGetTypeAndDimensions(source, out var result, out var parser))
        {
            value = result switch
            {
                (GeometryType.Point, false, false, 2) => GetPoint(2, ref parser, GetPoint),
                (GeometryType.Point, true, false, 3) => GetPoint(3, ref parser, GetPointZ),
                (GeometryType.Point, false, true, 3) => GetPoint(3, ref parser, GetPointM),
                (GeometryType.Point, true, true, 4) => GetPoint(4, ref parser, GetPointZM),
                (GeometryType.LineString, false, false, 2) => GetLineString(2, ref parser, GetPoint, GetLineString),
                (GeometryType.LineString, true, false, 3) => GetLineString(3, ref parser, GetPointZ, GetLineStringZ),
                (GeometryType.LineString, false, true, 3) => GetLineString(3, ref parser, GetPointM, GetLineStringM),
                (GeometryType.LineString, true, true, 4) => GetLineString(4, ref parser, GetPointZM, GetLineStringZM),
                (GeometryType.Polygon, false, false, 2) => GetPolygon(2, ref parser, GetPoint, GetPolygon),
                (GeometryType.Polygon, true, false, 3) => GetPolygon(3, ref parser, GetPointZ, GetPolygonZ),
                (GeometryType.Polygon, false, true, 3) => GetPolygon(3, ref parser, GetPointM, GetPolygonM),
                (GeometryType.Polygon, true, true, 4) => GetPolygon(4, ref parser, GetPointZM, GetPolygonZM),
                (GeometryType.MultiPoint, false, false, 2) => GetMultiPoint(2, ref parser, GetPoint),
                (GeometryType.MultiPoint, true, false, 3) => GetMultiPoint(3, ref parser, GetPointZ),
                (GeometryType.MultiPoint, false, true, 3) => GetMultiPoint(3, ref parser, GetPointM),
                (GeometryType.MultiPoint, true, true, 4) => GetMultiPoint(4, ref parser, GetPointZM),
                (GeometryType.MultiLineString, false, false, 2) => GetMultiLineString(2, ref parser, GetPoint, GetLineString),
                (GeometryType.MultiLineString, true, false, 3) => GetMultiLineString(3, ref parser, GetPointZ, GetLineStringZ),
                (GeometryType.MultiLineString, false, true, 3) => GetMultiLineString(3, ref parser, GetPointM, GetLineStringM),
                (GeometryType.MultiLineString, true, true, 4) => GetMultiLineString(4, ref parser, GetPointZM, GetLineStringZM),
                (GeometryType.MultiPolygon, false, false, 2) => GetMultiPolygon(2, ref parser, GetPoint, GetPolygon),
                (GeometryType.MultiPolygon, true, false, 3) => GetMultiPolygon(3, ref parser, GetPointZ, GetPolygonZ),
                (GeometryType.MultiPolygon, false, true, 3) => GetMultiPolygon(3, ref parser, GetPointM, GetPolygonM),
                (GeometryType.MultiPolygon, true, true, 4) => GetMultiPolygon(4, ref parser, GetPointZM, GetPolygonZM),
                _ => throw new Geometry.InvalidGeometryTypeException(),
            };

            bytesConsumed = parser.Position;
            return true;
        }

        bytesConsumed = 0;
        value = default;
        return false;
    }

    private static bool TryParse<T>(ReadOnlySpan<byte> source, out T value, out int bytesConsumed, Func<(GeometryType Type, bool HasZ, bool HasM, int Dimensions), bool> check, Func<double[], T> creator)
        where T : struct
    {
        if (TryGetTypeAndDimensions(source, out var result, out var parser) && check(result))
        {
            value = GetPoint(result.Dimensions, ref parser, creator);
            bytesConsumed = parser.Position;
            return true;
        }

        bytesConsumed = 0;
        value = default;
        return false;
    }

    private static bool TryParse<T>(ReadOnlySpan<byte> source, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out IReadOnlyCollection<T>? value, out int bytesConsumed, Func<(GeometryType Type, bool HasZ, bool HasM, int Dimensions), bool> check, Func<double[], T> creator)
        where T : struct
    {
        if (TryGetTypeAndDimensions(source, out var result, out var parser) && check(result))
        {
            value = GetMultiPoint(result.Dimensions, ref parser, creator);
            bytesConsumed = parser.Position;
            return true;
        }

        bytesConsumed = 0;
        value = default;
        return false;
    }

    private static bool TryParse<TPolyline, TPoint>(
        ReadOnlySpan<byte> source,
        [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TPolyline? value,
        out int bytesConsumed,
        Func<(GeometryType Type, bool HasZ, bool HasM, int Dimensions), bool> check,
        Func<double[], TPoint> createPoint,
        Func<IEnumerable<TPoint>, TPolyline> createPolyline)
        where TPolyline : Geometry.Polyline<TPoint>, new()
        where TPoint : struct
    {
        if (TryGetTypeAndDimensions(source, out var result, out var parser) && check(result))
        {
            value = GetLineString(result.Dimensions, ref parser, createPoint, createPolyline);
            bytesConsumed = parser.Position;
            return true;
        }

        bytesConsumed = 0;
        value = default;
        return false;
    }

    private static bool TryParse<TPolyline, TPoint>(
        ReadOnlySpan<byte> source,
        [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out IReadOnlyCollection<TPolyline>? value,
        out int bytesConsumed,
        Func<(GeometryType Type, bool HasZ, bool HasM, int Dimensions), bool> check,
        Func<double[], TPoint> createPoint,
        Func<IEnumerable<TPoint>, TPolyline> createPolyline)
        where TPolyline : Geometry.Polyline<TPoint>, new()
        where TPoint : struct
    {
        if (TryGetTypeAndDimensions(source, out var result, out var parser) && check(result))
        {
            value = [.. GetMultiLineString(result.Dimensions, ref parser, createPoint, createPolyline)];
            bytesConsumed = parser.Position;
            return true;
        }

        bytesConsumed = 0;
        value = default;
        return false;
    }

    private static bool TryParse<TPolygon, TPoint>(
        ReadOnlySpan<byte> source,
        [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TPolygon? value,
        out int bytesConsumed,
        Func<(GeometryType Type, bool HasZ, bool HasM, int Dimensions), bool> check,
        Func<double[], TPoint> createPoint,
        Func<IEnumerable<IEnumerable<TPoint>>, TPolygon> createPolygon)
        where TPolygon : Geometry.Polygon<TPoint>, new()
        where TPoint : struct
    {
        if (TryGetTypeAndDimensions(source, out var result, out var parser) && check(result))
        {
            value = GetPolygon(result.Dimensions, ref parser, createPoint, createPolygon);
            bytesConsumed = parser.Position;
            return true;
        }

        bytesConsumed = 0;
        value = default;
        return false;
    }

    private static bool TryParse<TPolygon, TPoint>(
        ReadOnlySpan<byte> source,
        [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out IReadOnlyCollection<TPolygon>? value,
        out int bytesConsumed,
        Func<(GeometryType Type, bool HasZ, bool HasM, int Dimensions), bool> check,
        Func<double[], TPoint> createPoint,
        Func<IEnumerable<IEnumerable<TPoint>>, TPolygon> createPolygon)
        where TPolygon : Geometry.Polygon<TPoint>, new()
        where TPoint : struct
    {
        if (TryGetTypeAndDimensions(source, out var result, out var parser) && check(result))
        {
            value = GetMultiPolygon(result.Dimensions, ref parser, createPoint, createPolygon);
            bytesConsumed = parser.Position;
            return true;
        }

        bytesConsumed = 0;
        value = default;
        return false;
    }

    private static bool TryGetTypeAndDimensions(ReadOnlySpan<byte> wkt, out (GeometryType Type, bool HasZ, bool HasM, int Dimensions) result, out Parser reader)
    {
        const byte UpperZ = DimensionTypes.Z;
        const byte UpperM = DimensionTypes.M;
        const byte LowerZ = UpperZ + 32;
        const byte LowerM = UpperM + 32;

        reader = Parser.Create(wkt);
        if (!reader.Read() || reader.TokenType is not Parser.WktTokenType.Type)
        {
            result = default;
            return false;
        }

        var type = reader.GetValue();
        if (!reader.Read() || reader.TokenType is not Parser.WktTokenType.Dimensions)
        {
            result = (GetGeometryType(type), false, false, 2);
            return true;
        }

        var (hasZ, hasM, dimensions) = reader.GetValue() switch
        {
            [LowerZ or UpperZ, LowerM or UpperM] => (true, true, 4),
            [LowerZ or UpperZ] => (true, false, 3),
            [LowerM or UpperM] => (false, true, 3),
            _ => (false, false, 2),
        };

        _ = reader.Read();
        result = (GetGeometryType(type), hasZ, hasM, dimensions);
        return true;

        static GeometryType GetGeometryType(ReadOnlySpan<byte> span)
        {
            return span switch
            {
                [(byte)'P', (byte)'O', (byte)'I', (byte)'N', (byte)'T', ..] => GeometryType.Point,
                [(byte)'L', (byte)'I', (byte)'N', (byte)'E', (byte)'S', (byte)'T', (byte)'R', (byte)'I', (byte)'N', (byte)'G', ..] => GeometryType.LineString,
                [(byte)'P', (byte)'O', (byte)'L', (byte)'Y', (byte)'G', (byte)'O', (byte)'N', ..] => GeometryType.Polygon,
                [(byte)'M', (byte)'U', (byte)'L', (byte)'T', (byte)'I', (byte)'P', (byte)'O', (byte)'I', (byte)'N', (byte)'T', ..] => GeometryType.MultiPoint,
                [(byte)'M', (byte)'U', (byte)'L', (byte)'T', (byte)'I', (byte)'L', (byte)'I', (byte)'N', (byte)'E', (byte)'S', (byte)'T', (byte)'R', (byte)'I', (byte)'N', (byte)'G', ..] => GeometryType.MultiLineString,
                [(byte)'M', (byte)'U', (byte)'L', (byte)'T', (byte)'I', (byte)'P', (byte)'O', (byte)'L', (byte)'Y', (byte)'G', (byte)'O', (byte)'N', ..] => GeometryType.MultiPolygon,
                _ => (GeometryType)(-1),
            };
        }
    }

    private static T GetPoint<T>(int values, ref Parser parser, Func<double[], T> createPoint)
        where T : struct => parser.TokenType switch
        {
            Parser.WktTokenType.Empty => default,
            _ => GetCoordinates(values, ref parser, createPoint).Single(),
        };

    private static Geometry.Point GetPoint(double[] coordinates) => new(coordinates);

    private static Geometry.PointZ GetPointZ(double[] coordinates) => new(coordinates);

    private static Geometry.PointM GetPointM(double[] coordinates) => new(coordinates);

    private static Geometry.PointZM GetPointZM(double[] coordinates) => new(coordinates);

    private static IReadOnlyCollection<T> GetMultiPoint<T>(int values, ref Parser parser, Func<double[], T> createPoint)
    {
        if (parser is { TokenType: Parser.WktTokenType.Empty })
        {
            // this is empty
            return [];
        }

        if (!parser.Read())
        {
            return [];
        }

        return parser switch
        {
            { TokenType: Parser.WktTokenType.OpenParenthesis } => [.. GetMultiCoordinatesWithoutRead(values, ref parser, createPoint).Select(pts => pts.Single())],
            { TokenType: Parser.WktTokenType.Number } => [.. GetCoordinatesWithoutRead(values, ref parser, createPoint)],
            _ => [],
        };
    }

    private static TPolyLine GetLineString<TPolyLine, TPoint>(int values, ref Parser parser, Func<double[], TPoint> createPoint, Func<IEnumerable<TPoint>, TPolyLine> createPolyline)
        where TPolyLine : Geometry.Polyline<TPoint>, new()
        where TPoint : struct => parser is { TokenType: Parser.WktTokenType.Empty } ? [] : createPolyline(GetCoordinates(values, ref parser, createPoint));

    private static Geometry.Polyline GetLineString(IEnumerable<Geometry.Point> points) => [.. points];

    private static Geometry.PolylineZ GetLineStringZ(IEnumerable<Geometry.PointZ> points) => [.. points];

    private static Geometry.PolylineM GetLineStringM(IEnumerable<Geometry.PointM> points) => [.. points];

    private static Geometry.PolylineZM GetLineStringZM(IEnumerable<Geometry.PointZM> points) => [.. points];

    private static IEnumerable<TPolyLine> GetMultiLineString<TPolyLine, TPoint>(int values, ref Parser parser, Func<double[], TPoint> createPoint, Func<IEnumerable<TPoint>, TPolyLine> createPolyline)
        where TPolyLine : Geometry.Polyline<TPoint>, new()
        where TPoint : struct => parser.TokenType switch
        {
            Parser.WktTokenType.Empty => [],
            _ => GetMultiCoordinates(values, ref parser, createPoint).Select(createPolyline),
        };

    private static TPolygon GetPolygon<TPolygon, TPoint>(int values, ref Parser parser, Func<double[], TPoint> createPoint, Func<IEnumerable<IEnumerable<TPoint>>, TPolygon> createPolygon)
        where TPolygon : Geometry.Polygon<TPoint>, new()
        where TPoint : struct => parser.TokenType switch
        {
            Parser.WktTokenType.Empty => [],
            _ => createPolygon(GetMultiCoordinates(values, ref parser, createPoint)),
        };

    private static Geometry.Polygon GetPolygon(IEnumerable<IEnumerable<Geometry.Point>> points) => [.. points.Select(static p => new Geometry.LinearRing<Geometry.Point>(p))];

    private static Geometry.PolygonZ GetPolygonZ(IEnumerable<IEnumerable<Geometry.PointZ>> points) => [.. points.Select(static p => new Geometry.LinearRing<Geometry.PointZ>(p))];

    private static Geometry.PolygonM GetPolygonM(IEnumerable<IEnumerable<Geometry.PointM>> points) => [.. points.Select(static p => new Geometry.LinearRing<Geometry.PointM>(p))];

    private static Geometry.PolygonZM GetPolygonZM(IEnumerable<IEnumerable<Geometry.PointZM>> points) => [.. points.Select(static p => new Geometry.LinearRing<Geometry.PointZM>(p))];

    private static List<TPolygon> GetMultiPolygon<TPolygon, TPoint>(int values, ref Parser parser, Func<double[], TPoint> createPoint, Func<IEnumerable<IEnumerable<TPoint>>, TPolygon> createPolygon)
        where TPolygon : Geometry.Polygon<TPoint>, new()
        where TPoint : struct
    {
        if (parser is { TokenType: Parser.WktTokenType.Empty })
        {
            // this is empty
            return [];
        }

        List<TPolygon> polygons = [];
        while (parser.Read())
        {
            if (parser is { TokenType: Parser.WktTokenType.CloseParenthesis })
            {
                break;
            }

            if (parser is { TokenType: Parser.WktTokenType.OpenParenthesis })
            {
                polygons.Add(GetPolygon(values, ref parser, createPoint, createPolygon));
            }
        }

        return polygons;
    }

    private static IEnumerable<T> GetCoordinates<T>(int values, ref Parser parser, Func<double[], T> createPoint) => parser.Read() ? GetCoordinatesWithoutRead(values, ref parser, createPoint) : [];

    private static IEnumerable<T> GetCoordinatesWithoutRead<T>(int values, ref Parser parser, Func<double[], T> createPoint)
    {
        IList<T> points = [];
        var coordinates = new double[values];
        var current = 0;
        do
        {
            if (parser is { TokenType: Parser.WktTokenType.Comma })
            {
                points.Add(createPoint(coordinates));
                coordinates = new double[values];
                current = 0;
            }
            else if (parser is { TokenType: Parser.WktTokenType.CloseParenthesis })
            {
                points.Add(createPoint(coordinates));
                break;
            }

            if (parser is { TokenType: Parser.WktTokenType.Number })
            {
                if (System.Buffers.Text.Utf8Parser.TryParse(parser.GetValue(), out double coordinate, out _))
                {
                    coordinates[current] = coordinate;
                    current++;
                }
                else
                {
                    throw new FormatException();
                }
            }
        }
        while (parser.Read());

        return points;
    }

    private static IEnumerable<IEnumerable<T>> GetMultiCoordinates<T>(int values, ref Parser parser, Func<double[], T> createPoint) => parser.Read() ? GetMultiCoordinatesWithoutRead(values, ref parser, createPoint) : [];

    private static IEnumerable<IEnumerable<T>> GetMultiCoordinatesWithoutRead<T>(int values, ref Parser parser, Func<double[], T> createPoint)
    {
        IList<IEnumerable<T>> coordinates = [];
        do
        {
            switch (parser.TokenType)
            {
                // we've got to the end
                case Parser.WktTokenType.CloseParenthesis:
                    return coordinates;
                case Parser.WktTokenType.OpenParenthesis:
                    coordinates.Add(GetCoordinates(values, ref parser, createPoint));
                    break;
            }
        }
        while (parser.Read());

        return coordinates;
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    private ref struct Parser
    {
        private readonly ReadOnlySpan<byte> wkt;

        private Parser(ReadOnlySpan<byte> wkt) => this.wkt = wkt;

        public enum WktTokenType
        {
            Start,
            Type,
            Dimensions,
            Empty,
            Comma,
            OpenParenthesis,
            CloseParenthesis,
            WhiteSpace,
            Number,
            Eof,
        }

        public WktTokenType TokenType { get; private set; }

        public int Position { get; private set; }

        public static Parser Create(ReadOnlySpan<byte> wkt) => new(wkt);

        public bool Read()
        {
            switch (this.TokenType)
            {
                case WktTokenType.Start:
                    this.Position = 0;
                    this.TokenType = WktTokenType.Type;
                    return true;
                case WktTokenType.Type:
                    // read the dimensions
                    this.Position = this.GetDimensionIndex();
                    this.TokenType = this.IsToken()
                        ? this.GetTokenType()
                        : WktTokenType.Dimensions;

                    return true;
                default:
                    this.TokenType = this.MoveToNextToken();
                    return this.TokenType is not WktTokenType.Eof;
            }
        }

        public readonly ReadOnlySpan<byte> GetValue()
        {
            if (this.TokenType is WktTokenType.Type)
            {
                // read the type
                return this.ReadType();
            }

            var endIndex = this.GetNextToken();
            var span = this.wkt[this.Position..endIndex];
            while (span.Length > 0 && char.IsWhiteSpace((char)span[0]))
            {
                span = span[1..];
            }

            while (span.Length > 0 && char.IsWhiteSpace((char)span[^1]))
            {
                span = span[..^1];
            }

            return span;
        }

        private static bool IsToken(byte value) => value is (byte)',' or (byte)'(' or (byte)')' or (byte)'E';

        private static bool IsTokenOrWhiteSpace(byte value) => IsToken(value) || char.IsWhiteSpace((char)value);

        private static bool IsTokenOrDigit(byte value) => IsToken(value) || IsDigit(value);

        private static bool IsDigit(byte value) => value is (byte)'-' || value is (byte)'+' || char.IsDigit((char)value);

        private readonly ReadOnlySpan<byte> ReadType()
        {
            var endIndex = this.GetDimensionIndex();
            return this.wkt[this.Position..endIndex];
        }

        private readonly int GetDimensionIndex()
        {
            var span = this.wkt;
            return FindEnd(span, GetIndex(span, this.Position)) + 1;

            static int GetIndex(ReadOnlySpan<byte> wkt, int index)
            {
                for (var i = index; i < wkt.Length; i++)
                {
                    if (wkt[i] is (byte)' ' or (byte)'(')
                    {
                        return i - 1;
                    }
                }

                return -1;
            }

            static int FindEnd(ReadOnlySpan<byte> wkt, int index)
            {
                if (IsNotDimension(ref wkt, index))
                {
                    return index;
                }

                for (var i = index - 1; i >= 0; i--)
                {
                    if (IsNotDimension(ref wkt, i))
                    {
                        return i;
                    }
                }

                return index;

                static bool IsNotDimension(ref ReadOnlySpan<byte> wkt, int k)
                {
                    return wkt[k] is not DimensionTypes.Z and not DimensionTypes.M;
                }
            }
        }

        private WktTokenType MoveToNextToken()
        {
            this.Position = this.GetNextToken();
            return this.Position == this.wkt.Length ? WktTokenType.Eof : this.GetTokenType();
        }

        private readonly int GetNextToken()
        {
            return GetNextTokenCore(
                this.wkt,
                this.Position,
                this.TokenType is WktTokenType.Number ? IsTokenOrWhiteSpace : IsTokenOrDigit);

            static int GetNextTokenCore(ReadOnlySpan<byte> wkt, int position, Func<byte, bool> isToken)
            {
                for (var i = position + 1; i < wkt.Length; i++)
                {
                    if (isToken(wkt[i]))
                    {
                        return i;
                    }
                }

                return wkt.Length;
            }
        }

        private readonly bool IsToken() => IsToken(this.wkt[this.Position]);

        private readonly WktTokenType GetTokenType() => this.wkt[this.Position] switch
        {
            (byte)',' => WktTokenType.Comma,
            (byte)'(' => WktTokenType.OpenParenthesis,
            (byte)')' => WktTokenType.CloseParenthesis,
            (byte)'E' => WktTokenType.Empty,
            var x when char.IsWhiteSpace((char)x) => WktTokenType.WhiteSpace,
            var x when IsDigit(x) => WktTokenType.Number,
            _ => throw new InvalidOperationException("Not a valid token"),
        };
    }

    private static class DimensionTypes
    {
        public const byte Z = (byte)'Z';

        public const byte M = (byte)'M';
    }
}