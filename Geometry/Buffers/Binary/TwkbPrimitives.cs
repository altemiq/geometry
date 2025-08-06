// -----------------------------------------------------------------------
// <copyright file="TwkbPrimitives.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Binary;

/// <summary>
/// The Tiny Well-Known Binary primitives.
/// </summary>
public static class TwkbPrimitives
{
    private enum TinyWkbGeometryType : byte
    {
        /// <summary>
        /// A point geometry.
        /// </summary>
        Point = 1,

        /// <summary>
        /// A line string geometry.
        /// </summary>
        Linestring = 2,

        /// <summary>
        /// A polygon geometry.
        /// </summary>
        Polygon = 3,

        /// <summary>
        /// A multipoint geometry.
        /// </summary>
        MultiPoint = 4,

        /// <summary>
        /// A multi line string geometry.
        /// </summary>
        MultiLinestring = 5,

        /// <summary>
        /// A multi polygon geometry.
        /// </summary>
        MultiPolygon = 6,

        /// <summary>
        /// A geometry collection.
        /// </summary>
        GeometryCollection = 7,
    }

    /// <summary>
    /// Reads the value as a <see cref="Geometry.Point"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An instance of <see cref="Geometry.Point"/> if successful; otherwise <see langword="null"/>.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POINT</c>.</exception>
    public static Geometry.Point ReadPoint(ReadOnlySpan<byte> source) => ReadGeometry<Geometry.Point>(source);

    /// <summary>
    /// Reads the value as a <see cref="Geometry.PointZ"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An instance of <see cref="Geometry.PointZ"/> if successful; otherwise <see langword="null"/>.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POINTZ</c>.</exception>
    public static Geometry.PointZ ReadPointZ(ReadOnlySpan<byte> source) => ReadGeometry<Geometry.PointZ>(source);

    /// <summary>
    /// Reads the value as a <see cref="Geometry.PointM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An instance of <see cref="Geometry.PointM"/> if successful; otherwise <see langword="null"/>.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POINTM</c>.</exception>
    public static Geometry.PointM ReadPointM(ReadOnlySpan<byte> source) => ReadGeometry<Geometry.PointM>(source);

    /// <summary>
    /// Reads the value as a <see cref="Geometry.PointZM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An instance of <see cref="Geometry.PointZM"/> if successful; otherwise <see langword="null"/>.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POINTZM</c>.</exception>
    public static Geometry.PointZM ReadPointZM(ReadOnlySpan<byte> source) => ReadGeometry<Geometry.PointZM>(source);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.Point"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An <see cref="IReadOnlyCollection{T}"/> filled with instances of <see cref="Geometry.Point"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINT</c>.</exception>
    public static IReadOnlyCollection<Geometry.Point> ReadMultiPoint(ReadOnlySpan<byte> source) => ReadGeometry<IReadOnlyCollection<Geometry.Point>>(source);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PointZ"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An <see cref="IReadOnlyCollection{T}"/> filled with instances of <see cref="Geometry.PointZ"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINTZ</c>.</exception>
    public static IReadOnlyCollection<Geometry.PointZ> ReadMultiPointZ(ReadOnlySpan<byte> source) => ReadGeometry<IReadOnlyCollection<Geometry.PointZ>>(source);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PointM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An <see cref="IReadOnlyCollection{T}"/> filled with instances of <see cref="Geometry.PointM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINTM</c>.</exception>
    public static IReadOnlyCollection<Geometry.PointM> ReadMultiPointM(ReadOnlySpan<byte> source) => ReadGeometry<IReadOnlyCollection<Geometry.PointM>>(source);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.Point"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An <see cref="IReadOnlyCollection{T}"/> filled with instances of <see cref="Geometry.Point"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINTZM</c>.</exception>
    public static IReadOnlyCollection<Geometry.PointZM> ReadMultiPointZM(ReadOnlySpan<byte> source) => ReadGeometry<IReadOnlyCollection<Geometry.PointZM>>(source);

    /// <summary>
    /// Reads the value as <see cref="Geometry.Polyline"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.Polyline"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>LINESTRING</c>.</exception>
    public static Geometry.Polyline ReadLineString(ReadOnlySpan<byte> source) => ReadGeometry<Geometry.Polyline>(source);

    /// <summary>
    /// Reads the value as <see cref="Geometry.PolylineZ"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolylineZ"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>LINESTRINGZ</c>.</exception>
    public static Geometry.PolylineZ ReadLineStringZ(ReadOnlySpan<byte> source) => ReadGeometry<Geometry.PolylineZ>(source);

    /// <summary>
    /// Reads the value as <see cref="Geometry.PolylineM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolylineM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>LINESTRINGM</c>.</exception>
    public static Geometry.PolylineM ReadLineStringM(ReadOnlySpan<byte> source) => ReadGeometry<Geometry.PolylineM>(source);

    /// <summary>
    /// Reads the value as <see cref="Geometry.PolylineZM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolylineZM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>LINESTRINGZM</c>.</exception>
    public static Geometry.PolylineZM ReadLineStringZM(ReadOnlySpan<byte> source) => ReadGeometry<Geometry.PolylineZM>(source);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.Polyline"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.Polyline"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTILINESTRING</c>.</exception>
    public static IReadOnlyCollection<Geometry.Polyline> ReadMultiLineString(ReadOnlySpan<byte> source) => ReadGeometry<IReadOnlyCollection<Geometry.Polyline>>(source);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PolylineZ"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolylineZ"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTILINESTRINGZ</c>.</exception>
    public static IReadOnlyCollection<Geometry.PolylineZ> ReadMultiLineStringZ(ReadOnlySpan<byte> source) => ReadGeometry<IReadOnlyCollection<Geometry.PolylineZ>>(source);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PolylineM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolylineM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTILINESTRINGM</c>.</exception>
    public static IReadOnlyCollection<Geometry.PolylineM> ReadMultiLineStringM(ReadOnlySpan<byte> source) => ReadGeometry<IReadOnlyCollection<Geometry.PolylineM>>(source);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PolylineZM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolylineZM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTILINESTRINGZM</c>.</exception>
    public static IReadOnlyCollection<Geometry.PolylineZM> ReadMultiLineStringZM(ReadOnlySpan<byte> source) => ReadGeometry<IReadOnlyCollection<Geometry.PolylineZM>>(source);

    /// <summary>
    /// Reads the value as <see cref="Geometry.Polygon"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.Polygon"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POLYGON</c>.</exception>
    public static Geometry.Polygon ReadPolygon(ReadOnlySpan<byte> source) => ReadGeometry<Geometry.Polygon>(source);

    /// <summary>
    /// Reads the value as <see cref="Geometry.PolygonZ"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolygonZ"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POLYGONZ</c>.</exception>
    public static Geometry.PolygonZ ReadPolygonZ(ReadOnlySpan<byte> source) => ReadGeometry<Geometry.PolygonZ>(source);

    /// <summary>
    /// Reads the value as <see cref="Geometry.PolygonM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolygonM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POLYGONM</c>.</exception>
    public static Geometry.PolygonM ReadPolygonM(ReadOnlySpan<byte> source) => ReadGeometry<Geometry.PolygonM>(source);

    /// <summary>
    /// Reads the value as <see cref="Geometry.PolygonZM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolygonZM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POLYGONZM</c>.</exception>
    public static Geometry.PolygonZM ReadPolygonZM(ReadOnlySpan<byte> source) => ReadGeometry<Geometry.PolygonZM>(source);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.Polygon"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.Polygon"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOLYGON</c>.</exception>
    public static IReadOnlyCollection<Geometry.Polygon> ReadMultiPolygon(ReadOnlySpan<byte> source) => ReadGeometry<IReadOnlyCollection<Geometry.Polygon>>(source);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PolygonZ"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolygonZ"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOLYGONZ</c>.</exception>
    public static IReadOnlyCollection<Geometry.PolygonZ> ReadMultiPolygonZ(ReadOnlySpan<byte> source) => ReadGeometry<IReadOnlyCollection<Geometry.PolygonZ>>(source);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PolygonM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolygonM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOLYGONM</c>.</exception>
    public static IReadOnlyCollection<Geometry.PolygonM> ReadMultiPolygonM(ReadOnlySpan<byte> source) => ReadGeometry<IReadOnlyCollection<Geometry.PolygonM>>(source);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PolygonZM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolygonZM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOLYGONZM</c>.</exception>
    public static IReadOnlyCollection<Geometry.PolygonZM> ReadMultiPolygonZM(ReadOnlySpan<byte> source) => ReadGeometry<IReadOnlyCollection<Geometry.PolygonZM>>(source);

    /// <summary>
    /// Reads the value as a geometry.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An instance of <see cref="Geometry.Point"/>, <see cref="Geometry.PointZ"/> <see cref="Geometry.PointZM"/>, <see cref="Geometry.Polyline"/>, <see cref="Geometry.PolylineZ"/> <see cref="Geometry.PolylineZM"/>, or an <see cref="IEnumerable{T}"/> fill with one of those if successful; otherwise <see langword="null"/>.</returns>
    public static object ReadGeometry(ReadOnlySpan<byte> source)
    {
        var header = TinyWkbRecordHeader.Read(ref source);
        return ReadGeometry(source, header);
    }

    /// <summary>
    /// Writes a <see cref="Geometry.Point"/> instance into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="precision">The X/Y precision value.</param>
    /// <param name="boundingBox">Set to <see langword="true"/> to write the bounding box.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePoint(Span<byte> destination, Geometry.Point value, int precision, bool boundingBox = false) => WritePoint(destination, value, precision, default, default, boundingBox, GetXY, GetZ, GetM, IsNullOrEmpty);

    /// <summary>
    /// Writes <see cref="Geometry.Point"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="precision">The X/Y precision value.</param>
    /// <param name="boundingBox">Set to <see langword="true"/> to write the bounding box.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePoint(Span<byte> destination, IEnumerable<Geometry.Point> value, int precision, bool boundingBox = false) => WritePoints(destination, [.. value], precision, default, default, boundingBox, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes a <see cref="Geometry.PointZ"/> instance into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="precisionXY">The X/Y precision value.</param>
    /// <param name="precisionZ">The Z precision value.</param>
    /// <param name="boundingBox">Set to <see langword="true"/> to write the bounding box.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZ(Span<byte> destination, Geometry.PointZ value, int precisionXY, int precisionZ, bool boundingBox = false) => WritePoint(destination, value, precisionXY, precisionZ, default, boundingBox, GetXY, GetZ, GetM, IsNullOrEmpty);

    /// <summary>
    /// Writes <see cref="Geometry.PointZ"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="precisionXY">The X/Y precision value.</param>
    /// <param name="precisionZ">The Z precision value.</param>
    /// <param name="boundingBox">Set to <see langword="true"/> to write the bounding box.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZ(Span<byte> destination, IEnumerable<Geometry.PointZ> value, int precisionXY, int precisionZ, bool boundingBox = false) => WritePoints(destination, [.. value], precisionXY, precisionZ, default, boundingBox, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes a <see cref="Geometry.PointM"/> instance into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="precisionXY">The X/Y precision value.</param>
    /// <param name="precisionM">The m precision value.</param>
    /// <param name="boundingBox">Set to <see langword="true"/> to write the bounding box.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointM(Span<byte> destination, Geometry.PointM value, int precisionXY, int precisionM, bool boundingBox = false) => WritePoint(destination, value, precisionXY, default, precisionM, boundingBox, GetXY, GetZ, GetM, IsNullOrEmpty);

    /// <summary>
    /// Writes <see cref="Geometry.PointM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="precisionXY">The X/Y precision value.</param>
    /// <param name="precisionM">The M precision value.</param>
    /// <param name="boundingBox">Set to <see langword="true"/> to write the bounding box.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointM(Span<byte> destination, IEnumerable<Geometry.PointM> value, int precisionXY, int precisionM, bool boundingBox = false) => WritePoints(destination, [.. value], precisionXY, default, precisionM, boundingBox, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes a <see cref="Geometry.PointZM"/> instance into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="precisionXY">The X/Y precision value.</param>
    /// <param name="precisionZ">The Z precision value.</param>
    /// <param name="precisionM">The m precision value.</param>
    /// <param name="boundingBox">Set to <see langword="true"/> to write the bounding box.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZM(Span<byte> destination, Geometry.PointZM value, int precisionXY, int precisionZ, int precisionM, bool boundingBox = false) => WritePoint(destination, value, precisionXY, precisionZ, precisionM, boundingBox, GetXY, GetZ, GetM, IsNullOrEmpty);

    /// <summary>
    /// Writes <see cref="Geometry.PointZM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="precisionXY">The X/Y precision value.</param>
    /// <param name="precisionZ">The Z precision value.</param>
    /// <param name="precisionM">The M precision value.</param>
    /// <param name="boundingBox">Set to <see langword="true"/> to write the bounding box.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZM(Span<byte> destination, IEnumerable<Geometry.PointZM> value, int precisionXY, int precisionZ, int precisionM, bool boundingBox = false) => WritePoints(destination, [.. value], precisionXY, precisionZ, precisionM, boundingBox, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes a <see cref="Geometry.Point"/> instance into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="precision">The X/Y precision value.</param>
    /// <param name="boundingBox">Set to <see langword="true"/> to write the bounding box.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineString(Span<byte> destination, Geometry.Polyline<Geometry.Point> value, int precision, bool boundingBox = false) => WritePolyline(destination, value, precision, default, default, boundingBox, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes <see cref="Geometry.Polyline"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="precision">The X/Y precision value.</param>
    /// <param name="boundingBox">Set to <see langword="true"/> to write the bounding box.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Polyline"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineString(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.Point>> value, int precision, bool boundingBox = false) => WritePolylines(destination, [.. value], precision, default, default, boundingBox, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes a <see cref="Geometry.PointZ"/> instance into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="precisionXY">The X/Y precision value.</param>
    /// <param name="precisionZ">The Z precision value.</param>
    /// <param name="boundingBox">Set to <see langword="true"/> to write the bounding box.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZ(Span<byte> destination, Geometry.Polyline<Geometry.PointZ> value, int precisionXY, int precisionZ, bool boundingBox = false) => WritePolyline(destination, value, precisionXY, precisionZ, default, boundingBox, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes <see cref="Geometry.PolylineZ"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="precisionXY">The X/Y precision value.</param>
    /// <param name="precisionZ">The Z precision value.</param>
    /// <param name="boundingBox">Set to <see langword="true"/> to write the bounding box.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZ(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointZ>> value, int precisionXY, int precisionZ, bool boundingBox = false) => WritePolylines(destination, [.. value], precisionXY, precisionZ, default, boundingBox, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes a <see cref="Geometry.PointM"/> instance into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="precisionXY">The X/Y precision value.</param>
    /// <param name="precisionM">The m precision value.</param>
    /// <param name="boundingBox">Set to <see langword="true"/> to write the bounding box.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringM(Span<byte> destination, Geometry.Polyline<Geometry.PointM> value, int precisionXY, int precisionM, bool boundingBox = false) => WritePolyline(destination, value, precisionXY, default, precisionM, boundingBox, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes <see cref="Geometry.PolylineM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="precisionXY">The X/Y precision value.</param>
    /// <param name="precisionM">The m precision value.</param>
    /// <param name="boundingBox">Set to <see langword="true"/> to write the bounding box.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringM(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointM>> value, int precisionXY, int precisionM, bool boundingBox = false) => WritePolylines(destination, [.. value], precisionXY, default, precisionM, boundingBox, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes a <see cref="Geometry.PointZM"/> instance into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="precisionXY">The X/Y precision value.</param>
    /// <param name="precisionZ">The Z precision value.</param>
    /// <param name="precisionM">The m precision value.</param>
    /// <param name="boundingBox">Set to <see langword="true"/> to write the bounding box.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZM(Span<byte> destination, Geometry.Polyline<Geometry.PointZM> value, int precisionXY, int precisionZ, int precisionM, bool boundingBox = false) => WritePolyline(destination, value, precisionXY, precisionZ, precisionM, boundingBox, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes <see cref="Geometry.PolylineZM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="precisionXY">The X/Y precision value.</param>
    /// <param name="precisionZ">The Z precision value.</param>
    /// <param name="precisionM">The m precision value.</param>
    /// <param name="boundingBox">Set to <see langword="true"/> to write the bounding box.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZM(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointZM>> value, int precisionXY, int precisionZ, int precisionM, bool boundingBox = false) => WritePolylines(destination, [.. value], precisionXY, precisionZ, precisionM, boundingBox, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes a <see cref="Geometry.Point"/> instance into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="precision">The X/Y precision value.</param>
    /// <param name="boundingBox">Set to <see langword="true"/> to write the bounding box.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygon(Span<byte> destination, Geometry.Polygon<Geometry.Point> value, int precision, bool boundingBox = false) => WritePolygon(destination, value, precision, default, default, boundingBox, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes <see cref="Geometry.Polygon"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="precision">The X/Y precision value.</param>
    /// <param name="boundingBox">Set to <see langword="true"/> to write the bounding box.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Polygon"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygon(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.Point>> value, int precision, bool boundingBox = false) => WritePolygons(destination, [.. value], precision, default, default, boundingBox, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes a <see cref="Geometry.PointZ"/> instance into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="precisionXY">The X/Y precision value.</param>
    /// <param name="precisionZ">The Z precision value.</param>
    /// <param name="boundingBox">Set to <see langword="true"/> to write the bounding box.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZ(Span<byte> destination, Geometry.Polygon<Geometry.PointZ> value, int precisionXY, int precisionZ, bool boundingBox = false) => WritePolygon(destination, value, precisionXY, precisionZ, default, boundingBox, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes <see cref="Geometry.PolygonZ"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="precisionXY">The X/Y precision value.</param>
    /// <param name="precisionZ">The Z precision value.</param>
    /// <param name="boundingBox">Set to <see langword="true"/> to write the bounding box.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZ(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointZ>> value, int precisionXY, int precisionZ, bool boundingBox = false) => WritePolygons(destination, [.. value], precisionXY, precisionZ, default, boundingBox, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes a <see cref="Geometry.PointM"/> instance into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="precisionXY">The X/Y precision value.</param>
    /// <param name="precisionM">The m precision value.</param>
    /// <param name="boundingBox">Set to <see langword="true"/> to write the bounding box.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonM(Span<byte> destination, Geometry.Polygon<Geometry.PointM> value, int precisionXY, int precisionM, bool boundingBox = false) => WritePolygon(destination, value, precisionXY, default, precisionM, boundingBox, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes <see cref="Geometry.PolygonM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="precisionXY">The X/Y precision value.</param>
    /// <param name="precisionM">The m precision value.</param>
    /// <param name="boundingBox">Set to <see langword="true"/> to write the bounding box.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonM(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointM>> value, int precisionXY, int precisionM, bool boundingBox = false) => WritePolygons(destination, [.. value], precisionXY, default, precisionM, boundingBox, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes a <see cref="Geometry.PointZM"/> instance into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="precisionXY">The X/Y precision value.</param>
    /// <param name="precisionZ">The Z precision value.</param>
    /// <param name="precisionM">The m precision value.</param>
    /// <param name="boundingBox">Set to <see langword="true"/> to write the bounding box.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZM(Span<byte> destination, Geometry.Polygon<Geometry.PointZM> value, int precisionXY, int precisionZ, int precisionM, bool boundingBox = false) => WritePolygon(destination, value, precisionXY, precisionZ, precisionM, boundingBox, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes <see cref="Geometry.PolygonZM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="precisionXY">The X/Y precision value.</param>
    /// <param name="precisionZ">The Z precision value.</param>
    /// <param name="precisionM">The m precision value.</param>
    /// <param name="boundingBox">Set to <see langword="true"/> to write the bounding box.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZM(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointZM>> value, int precisionXY, int precisionZ, int precisionM, bool boundingBox = false) => WritePolygons(destination, [.. value], precisionXY, precisionZ, precisionM, boundingBox, GetXY, GetZ, GetM);

    private static Geometry.Point CreatePoint(double[] coordinates) => new(coordinates[0], coordinates[1]);

    private static Geometry.PointZ CreatePointZ(double[] coordinates) => new(coordinates[0], coordinates[1], coordinates[2]);

    private static Geometry.PointM CreatePointM(double[] coordinates) => new(coordinates[0], coordinates[1], coordinates[2]);

    private static Geometry.PointZM CreatePointZM(double[] coordinates) => new(coordinates[0], coordinates[1], coordinates[2], coordinates[3]);

    private static bool IsNullOrEmpty(Geometry.Point point) => point is { IsEmpty: true };

    private static bool IsNullOrEmpty(Geometry.PointZ point) => point is { IsEmpty: true };

    private static bool IsNullOrEmpty(Geometry.PointM point) => point is { IsEmpty: true };

    private static bool IsNullOrEmpty(Geometry.PointZM point) => point is { IsEmpty: true };

    private static (double X, double Y) GetXY(Geometry.Point point) => (point.X, point.Y);

    private static (double X, double Y) GetXY(Geometry.PointZ point) => (point.X, point.Y);

    private static (double X, double Y) GetXY(Geometry.PointM point) => (point.X, point.Y);

    private static (double X, double Y) GetXY(Geometry.PointZM point) => (point.X, point.Y);

    private static double GetZ(Geometry.Point point) => throw new Geometry.InvalidGeometryTypeException();

    private static double GetZ(Geometry.PointZ point) => point.Z;

    private static double GetZ(Geometry.PointM point) => throw new Geometry.InvalidGeometryTypeException();

    private static double GetZ(Geometry.PointZM point) => point.Z;

    private static double GetM(Geometry.Point point) => throw new Geometry.InvalidGeometryTypeException();

    private static double GetM(Geometry.PointZ point) => throw new Geometry.InvalidGeometryTypeException();

    private static double GetM(Geometry.PointM point) => point.Measurement;

    private static double GetM(Geometry.PointZM point) => point.Measurement;

    private static T ReadGeometry<T>(ReadOnlySpan<byte> source) => (T)(ReadGeometry(source) ?? throw new InvalidDataException());

    private static object ReadGeometry(ReadOnlySpan<byte> source, TinyWkbRecordHeader header) => ReadGeometry(source, header, out _);

    private static object ReadGeometry(ReadOnlySpan<byte> source, TinyWkbRecordHeader header, out IList<long>? idList)
    {
        idList = null;

        if (header.HasSize)
        {
            _ = ReadVarUInt64(ref source);
        }

        if (header.HasEmptyGeometry)
        {
            return (header.GeometryType, header.HasZ, header.HasM) switch
            {
                (TinyWkbGeometryType.Point, false, false) => Geometry.Point.Empty,
                (TinyWkbGeometryType.Point, true, false) => Geometry.PointZ.Empty,
                (TinyWkbGeometryType.Point, false, true) => Geometry.PointM.Empty,
                (TinyWkbGeometryType.Point, true, true) => Geometry.PointZM.Empty,
                (TinyWkbGeometryType.Linestring, false, false) => new Geometry.Polyline(),
                (TinyWkbGeometryType.Linestring, true, false) => new Geometry.PolylineZ(),
                (TinyWkbGeometryType.Linestring, false, true) => new Geometry.PolylineM(),
                (TinyWkbGeometryType.Linestring, true, true) => new Geometry.PolylineZM(),
                (TinyWkbGeometryType.Polygon, false, false) => new Geometry.Polygon(),
                (TinyWkbGeometryType.Polygon, true, false) => new Geometry.PolygonZ(),
                (TinyWkbGeometryType.Polygon, false, true) => new Geometry.PolygonM(),
                (TinyWkbGeometryType.Polygon, true, true) => new Geometry.PolygonZM(),

                (TinyWkbGeometryType.MultiPoint, false, false) => Enumerable.Empty<Geometry.Point>(),
                (TinyWkbGeometryType.MultiPoint, true, false) => Enumerable.Empty<Geometry.PointZ>(),
                (TinyWkbGeometryType.MultiPoint, false, true) => Enumerable.Empty<Geometry.PointM>(),
                (TinyWkbGeometryType.MultiPoint, true, true) => Enumerable.Empty<Geometry.PointZM>(),

                (TinyWkbGeometryType.MultiLinestring, false, false) => Enumerable.Empty<Geometry.Polyline>(),
                (TinyWkbGeometryType.MultiLinestring, true, false) => Enumerable.Empty<Geometry.PolylineZ>(),
                (TinyWkbGeometryType.MultiLinestring, false, true) => Enumerable.Empty<Geometry.PolylineM>(),
                (TinyWkbGeometryType.MultiLinestring, true, true) => Enumerable.Empty<Geometry.PolylineZM>(),
                (TinyWkbGeometryType.MultiPolygon, false, false) => Enumerable.Empty<Geometry.Polygon>(),
                (TinyWkbGeometryType.MultiPolygon, true, false) => Enumerable.Empty<Geometry.PolygonZ>(),
                (TinyWkbGeometryType.MultiPolygon, false, true) => Enumerable.Empty<Geometry.PolygonM>(),
                (TinyWkbGeometryType.MultiPolygon, true, true) => Enumerable.Empty<Geometry.PolygonZM>(),

                (TinyWkbGeometryType.GeometryCollection, _, _) => Enumerable.Empty<object>(),

                _ => throw new Geometry.InvalidGeometryTypeException(),
            };
        }

        var internalReader = new Reader(source, header);

        if (header.HasBoundingBox)
        {
            _ = internalReader.ReadBoundingBox();
        }

        return (header.GeometryType, header.HasZ, header.HasM) switch
        {
            (TinyWkbGeometryType.Point, false, false) => internalReader.ReadPoints(1, CreatePoint).SingleOrDefault(),
            (TinyWkbGeometryType.Point, true, false) => internalReader.ReadPoints(1, CreatePointZ).SingleOrDefault(),
            (TinyWkbGeometryType.Point, false, true) => internalReader.ReadPoints(1, CreatePointM).SingleOrDefault(),
            (TinyWkbGeometryType.Point, true, true) => internalReader.ReadPoints(1, CreatePointZM).SingleOrDefault(),
            (TinyWkbGeometryType.MultiPoint, false, false) => internalReader.ReadPoints(out idList, header.HasIdList, CreatePoint),
            (TinyWkbGeometryType.MultiPoint, true, false) => internalReader.ReadPoints(out idList, header.HasIdList, CreatePointZ),
            (TinyWkbGeometryType.MultiPoint, false, true) => internalReader.ReadPoints(out idList, header.HasIdList, CreatePointM),
            (TinyWkbGeometryType.MultiPoint, true, true) => internalReader.ReadPoints(out idList, header.HasIdList, CreatePointZM),

            (TinyWkbGeometryType.Linestring, false, false) => new Geometry.Polyline(internalReader.ReadPoints(CreatePoint)),
            (TinyWkbGeometryType.Linestring, true, false) => new Geometry.PolylineZ(internalReader.ReadPoints(CreatePointZ)),
            (TinyWkbGeometryType.Linestring, false, true) => new Geometry.PolylineM(internalReader.ReadPoints(CreatePointM)),
            (TinyWkbGeometryType.Linestring, true, true) => new Geometry.PolylineZM(internalReader.ReadPoints(CreatePointZM)),
            (TinyWkbGeometryType.MultiLinestring, false, false) => ReadMultiLineStringCore(internalReader, header.HasIdList, CreatePoint).Select(p => new Geometry.Polyline(p)).ToArray(),
            (TinyWkbGeometryType.MultiLinestring, true, false) => ReadMultiLineStringCore(internalReader, header.HasIdList, CreatePointZ).Select(p => new Geometry.PolylineZ(p)).ToArray(),
            (TinyWkbGeometryType.MultiLinestring, false, true) => ReadMultiLineStringCore(internalReader, header.HasIdList, CreatePointM).Select(p => new Geometry.PolylineM(p)).ToArray(),
            (TinyWkbGeometryType.MultiLinestring, true, true) => ReadMultiLineStringCore(internalReader, header.HasIdList, CreatePointZM).Select(p => new Geometry.PolylineZM(p)).ToArray(),

            (TinyWkbGeometryType.Polygon, false, false) => new Geometry.Polygon(ReadPolygonCore(ref internalReader, CreatePoint)),
            (TinyWkbGeometryType.Polygon, true, false) => new Geometry.PolygonZ(ReadPolygonCore(ref internalReader, CreatePointZ)),
            (TinyWkbGeometryType.Polygon, false, true) => new Geometry.PolygonM(ReadPolygonCore(ref internalReader, CreatePointM)),
            (TinyWkbGeometryType.Polygon, true, true) => new Geometry.PolygonZM(ReadPolygonCore(ref internalReader, CreatePointZM)),
            (TinyWkbGeometryType.MultiPolygon, false, false) => ReadMultiPolygonCore(internalReader, header.HasIdList, CreatePoint).Select(p => new Geometry.Polygon(p)).ToArray(),
            (TinyWkbGeometryType.MultiPolygon, true, false) => ReadMultiPolygonCore(internalReader, header.HasIdList, CreatePointZ).Select(p => new Geometry.PolygonZ(p)).ToArray(),
            (TinyWkbGeometryType.MultiPolygon, false, true) => ReadMultiPolygonCore(internalReader, header.HasIdList, CreatePointM).Select(p => new Geometry.PolygonM(p)).ToArray(),
            (TinyWkbGeometryType.MultiPolygon, true, true) => ReadMultiPolygonCore(internalReader, header.HasIdList, CreatePointZM).Select(p => new Geometry.PolygonZM(p)).ToArray(),

            (TinyWkbGeometryType.GeometryCollection, _, _) => ReadGeometryCollection(internalReader, header.HasIdList),

            _ => throw new Geometry.InvalidGeometryTypeException(),
        };

        static Geometry.LinearRing<T>[] ReadPolygonCore<T>(ref Reader reader, Func<double[], T> pointCreator)
            where T : struct
        {
            var ringCount = reader.ReadCount();
            var rings = new Geometry.LinearRing<T>[ringCount];

            for (var i = 0; i < rings.Length; i++)
            {
                var points = reader.ReadPoints(pointCreator).ToList();
                rings[i] = new(points);
            }

            return rings;
        }

        static T[][] ReadMultiLineStringCore<T>(Reader reader, bool readIdList, Func<double[], T> pointCreator)
        {
            var lineStringCount = reader.ReadCount();
            if (readIdList)
            {
                _ = reader.ReadIdList(lineStringCount);
            }

            var lineStrings = new T[lineStringCount][];

            for (var i = 0; i < lineStrings.Length; i++)
            {
                var points = reader.ReadPoints(pointCreator);
                lineStrings[i] = points;
            }

            return lineStrings;
        }

        static Geometry.LinearRing<T>[][] ReadMultiPolygonCore<T>(Reader reader, bool readIdList, Func<double[], T> pointCreator)
            where T : struct
        {
            var count = reader.ReadCount();
            if (readIdList)
            {
                _ = reader.ReadIdList(count);
            }

            var linearRings = new Geometry.LinearRing<T>[count][];
            for (var i = 0UL; i < count; i++)
            {
                linearRings[i] = ReadPolygonCore(ref reader, pointCreator);
            }

            return linearRings;
        }

        static object?[] ReadGeometryCollection(Reader reader, bool readIdList)
        {
            var geometryCount = reader.ReadCount();
            if (readIdList)
            {
                _ = reader.ReadIdList(geometryCount);
            }

            var geometries = new object?[geometryCount];
            for (var i = 0UL; i < geometryCount; i++)
            {
                geometries[i] = ReadGeometry(reader.AsSpan());
            }

            return geometries;
        }
    }

    private static long EncodeZigZag(long value, int bitLength) => (value << 1) ^ (value >> (bitLength - 1));

    private static long DecodeZigZag(ulong value) => (value & 0x1) is 0x1
        ? -1 * ((long)(value >> 1) + 1)
        : (long)(value >> 1);

    private static ulong ToTarget(ReadOnlySpan<byte> bytes, int sizeBites)
    {
        var shift = 0;
        var result = 0UL;

        foreach (ulong byteValue in bytes)
        {
            var temp = byteValue & 0x7f;
            result |= temp << shift;

            if (shift > sizeBites)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes), Properties.Resources.ByteArrayTooLarge);
            }

            if ((byteValue & 0x80) is not 0x80)
            {
                return result;
            }

            shift += 7;
        }

        throw new ArgumentException(Properties.Resources.CannotDecodeVarInt, nameof(bytes));
    }

    private static double Scale(this int precision) => Math.Pow(10, precision);

    private static double Descale(this int precision) => Math.Pow(10, -precision);

    private static int WritePoint<T>(Span<byte> destination, T point, int precisionXY, int? precisionZ, int? precisionM, bool boundingBox, Func<T, (double X, double Y)> getXY, Func<T, double> getZ, Func<T, double> getM, Func<T?, bool> isNullOrEmpty)
    {
        var header = new TinyWkbRecordHeader(
            TinyWkbGeometryType.Point,
            precisionXY,
            hasEmptyGeometry: isNullOrEmpty(point),
            hasBoundingBox: boundingBox,
            precisionZ: precisionZ,
            precisionM: precisionM);
        var writer = Writer.Initialise(destination, header);
        if (header.HasEmptyGeometry)
        {
            return writer.BytesWritten;
        }

        writer.Point(point, getXY, getZ, getM);
        return writer.BytesWritten;
    }

    private static int WritePoints<T>(Span<byte> destination, IReadOnlyCollection<T> points, int precisionXY, int? precisionZ, int? precisionM, bool boundingBox, Func<T, (double X, double Y)> getXY, Func<T, double> getZ, Func<T, double> getM)
    {
        var count = points.Count;
        var header = new TinyWkbRecordHeader(
            TinyWkbGeometryType.MultiPoint,
            precisionXY,
            hasEmptyGeometry: count is 0,
            hasBoundingBox: boundingBox,
            precisionZ: precisionZ,
            precisionM: precisionM);
        var writer = Writer.Initialise(destination, header);
        writer.Points(points, count, getXY, getZ, getM);
        return writer.BytesWritten;
    }

    private static int WritePolyline<T>(Span<byte> destination, Geometry.Polyline<T> points, int precisionXY, int? precisionZ, int? precisionM, bool boundingBox, Func<T, (double X, double Y)> getXY, Func<T, double> getZ, Func<T, double> getM)
    {
        var count = points.Count;
        var header = new TinyWkbRecordHeader(
            TinyWkbGeometryType.Linestring,
            precisionXY,
            hasEmptyGeometry: count is 0,
            hasBoundingBox: boundingBox,
            precisionZ: precisionZ,
            precisionM: precisionM);
        var writer = Writer.Initialise(destination, header);
        writer.Points(points, count, getXY, getZ, getM);
        return writer.BytesWritten;
    }

    private static int WritePolylines<T>(Span<byte> destination, IReadOnlyCollection<Geometry.Polyline<T>> lines, int precisionXY, int? precisionZ, int? precisionM, bool boundingBox, Func<T, (double X, double Y)> getXY, Func<T, double> getZ, Func<T, double> getM)
    {
        var count = lines.Count;
        var header = new TinyWkbRecordHeader(
            TinyWkbGeometryType.MultiLinestring,
            precisionXY,
            hasEmptyGeometry: count is 0,
            hasBoundingBox: boundingBox,
            precisionZ: precisionZ,
            precisionM: precisionM);
        var writer = Writer.Initialise(destination, header);
        writer.PointCollections(lines, count, getXY, getZ, getM);
        return writer.BytesWritten;
    }

    private static int WritePolygon<T>(Span<byte> destination, Geometry.Polygon<T> polygon, int precisionXY, int? precisionZ, int? precisionM, bool boundingBox, Func<T, (double X, double Y)> getXY, Func<T, double> getZ, Func<T, double> getM)
        where T : struct
    {
        var count = polygon.Count;
        var header = new TinyWkbRecordHeader(
            TinyWkbGeometryType.Polygon,
            precisionXY,
            hasEmptyGeometry: count is 0,
            hasBoundingBox: boundingBox,
            precisionZ: precisionZ,
            precisionM: precisionM);
        var writer = Writer.Initialise(destination, header);
        writer.PointCollections(polygon, count, getXY, getZ, getM);
        return writer.BytesWritten;
    }

    private static int WritePolygons<T>(Span<byte> destination, IReadOnlyCollection<Geometry.Polygon<T>> polygons, int precisionXY, int? precisionZ, int? precisionM, bool boundingBox, Func<T, (double X, double Y)> getXY, Func<T, double> getZ, Func<T, double> getM)
        where T : struct
    {
        var count = polygons.Count;
        var header = new TinyWkbRecordHeader(
            TinyWkbGeometryType.MultiPolygon,
            precisionXY,
            hasEmptyGeometry: count is 0,
            hasBoundingBox: boundingBox,
            precisionZ: precisionZ,
            precisionM: precisionM);
        var writer = Writer.Initialise(destination, header);
        if (count is 0)
        {
            return writer.BytesWritten;
        }

        writer.Count((ulong)count);

        foreach (var polygon in polygons)
        {
            writer.PointCollections(polygon, polygon.Count, getXY, getZ, getM);
        }

        return writer.BytesWritten;
    }

    private static ulong ReadUInt64(ReadOnlySpan<byte> source) => ToTarget(source, 64);

    private static ulong ReadVarUInt64(ref ReadOnlySpan<byte> source)
    {
        var read = ReadVarIntCount(source, 9);
        var value = ReadUInt64(source[..read]);
        source = source[read..];
        return value;
    }

    private static int ReadVarIntCount(ReadOnlySpan<byte> source, int count)
    {
        for (var i = 0; i < count; i++)
        {
            if ((source[i] & 0x80) is 0)
            {
                return i + 1;
            }
        }

        throw new ArgumentOutOfRangeException(nameof(source));
    }

    private readonly struct TinyWkbRecordHeader
    {
        private readonly int value;

        public TinyWkbRecordHeader(
            TinyWkbGeometryType geometryType,
            int precisionXY = 7,
            bool hasEmptyGeometry = false,
            bool hasBoundingBox = true,
            bool hasSize = false,
            bool hasIdList = false,
            int? precisionZ = default,
            int? precisionM = default)
            : this()
        {
            if (precisionXY is < -7 or > 7)
            {
                throw new ArgumentOutOfRangeException(nameof(precisionXY));
            }

            if (precisionZ is < 0 or > 7)
            {
                throw new ArgumentOutOfRangeException(nameof(precisionZ));
            }

            if (precisionM is < 0 or > 7)
            {
                throw new ArgumentOutOfRangeException(nameof(precisionM));
            }

            // encode xy precision.
            var p = (int)EncodeZigZag(precisionXY, 4);

            // We don't write bounding boxes for points.
            if (geometryType is TinyWkbGeometryType.Point)
            {
                hasBoundingBox = false;
            }

            // No id-lists, for single instance geometries
            if (geometryType < TinyWkbGeometryType.MultiPoint)
            {
                hasIdList = false;
            }

            var metadata = 0;
            if (hasBoundingBox)
            {
                metadata |= (int)Metadata.HasBoundingBox;
            }

            if (hasSize)
            {
                metadata |= (int)Metadata.HasSize;
            }

            if (hasIdList)
            {
                metadata |= (int)Metadata.HasIdList;
            }

            if (precisionZ.HasValue || precisionM.HasValue)
            {
                metadata |= (int)Metadata.HasExtendedPrecisionInformation;
            }

            if (hasEmptyGeometry)
            {
                metadata |= (int)Metadata.HasEmptyGeometry;
            }

            if (precisionZ.HasValue)
            {
                metadata |= (int)Metadata.HasZ;
                metadata |= (0x07 & precisionZ.Value) << 18;
            }

            if (precisionM.HasValue)
            {
                metadata |= (int)Metadata.HasM;
                metadata |= (0x07 & precisionM.Value) << 21;
            }

            this.value = (int)geometryType | (p << 4) | metadata;
        }

        private TinyWkbRecordHeader(int header, int metadata, int epi = 0) => this.value = header | (metadata << 8) | (epi << 16);

        [Flags]
        private enum Metadata
        {
            None = 0,
            HasBoundingBox = 1 << 8,
            HasSize = 1 << 9,
            HasIdList = 1 << 10,
            HasExtendedPrecisionInformation = 1 << 11,
            HasEmptyGeometry = 1 << 12,
            HasZ = 1 << 16,
            HasM = 1 << 17,
        }

        public TinyWkbGeometryType GeometryType => (TinyWkbGeometryType)(this.value & 0x0F);

        public int PrecisionXY => (int)DecodeZigZag((ulong)((this.value & 0xF0) >> 4));

        public bool HasBoundingBox => (this.value & (int)Metadata.HasBoundingBox) is not 0;

        public bool HasSize => (this.value & (int)Metadata.HasSize) is not 0;

        public bool HasIdList => (this.value & (int)Metadata.HasIdList) is not 0;

        public bool HasEmptyGeometry => (this.value & (int)Metadata.HasEmptyGeometry) is not 0;

        public bool HasZ => (this.value & (int)Metadata.HasZ) is not 0;

        public bool HasM => (this.value & (int)Metadata.HasM) is not 0;

        public int PrecisionZ => 0x07 & (this.value >> 18);

        public int PrecisionM => 0x07 & (this.value >> 21);

        private bool HasExtendedPrecisionInformation => (this.value & (int)Metadata.HasExtendedPrecisionInformation) is not 0;

        public static TinyWkbRecordHeader Read(ref ReadOnlySpan<byte> source)
        {
            var header = source[0];
            var metadata = source[1];
            if (((metadata << 8) & (int)Metadata.HasExtendedPrecisionInformation) is not 0)
            {
                var epi = source[2];
                source = source[3..];
                return new(header, metadata, epi);
            }

            source = source[2..];
            return new(header, metadata);
        }

        public static int WriteHeader(TinyWkbRecordHeader header, Span<byte> destination)
        {
            System.Buffers.Binary.BinaryPrimitives.WriteUInt16LittleEndian(destination, (ushort)(0xFFFF & header.value));
            if (!header.HasExtendedPrecisionInformation)
            {
                return 2;
            }

            destination[2] = (byte)(0xFF & (header.value >> 16));
            return 3;
        }
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    private ref struct Reader
    {
        private readonly int precisionXY;
        private readonly double descaleXY;

        private readonly bool hasZ;
        private readonly int precisionZ;
        private readonly double descaleZ;

        private readonly bool hasM;
        private readonly int precisionM;
        private readonly double descaleM;

        private readonly long[] previousCoordinates;

        private ReadOnlySpan<byte> source;

        public Reader(ReadOnlySpan<byte> source, TinyWkbRecordHeader header)
        {
            this.source = source;

            this.precisionXY = header.PrecisionXY;
            this.descaleXY = this.precisionXY.Descale();

            this.hasZ = header.HasZ;
            this.precisionZ = this.hasZ ? header.PrecisionZ : 0;
            this.descaleZ = this.precisionZ.Descale();

            this.hasM = header.HasM;
            this.precisionM = this.hasM ? header.PrecisionM : 0;
            this.descaleM = this.precisionM.Descale();

            var coordinateCount = 2 + (this.hasZ ? 1 : 0) + (this.hasM ? 1 : 0);
            this.previousCoordinates = new long[coordinateCount];
        }

        public readonly ReadOnlySpan<byte> AsSpan() => this.source;

        public ulong ReadCount() => ReadVarUInt64(ref this.source);

        public object ReadBoundingBox()
        {
            var (x, width) = (ReadVarInt64(ref this.source), ReadVarInt64(ref this.source));
            var (y, height) = (ReadVarInt64(ref this.source), ReadVarInt64(ref this.source));
            var (z, depth) = this.hasZ
                ? (ReadVarInt64(ref this.source), ReadVarInt64(ref this.source))
                : (0L, 0L);

            var (m, length) = this.hasM
                ? (ReadVarInt64(ref this.source), ReadVarInt64(ref this.source))
                : (0L, 0L);

            return (this.hasZ, this.hasM) switch
            {
                (false, false) => Geometry.Envelope.FromXYWH(this.GetValueXY(x), this.GetValueXY(y), this.GetValueXY(width), this.GetValueXY(height)),
                (true, false) => Geometry.EnvelopeZ.FromXYZWHD(this.GetValueXY(x), this.GetValueXY(y), this.GetValueZ(z), this.GetValueXY(width), this.GetValueXY(height), this.GetValueZ(depth)),
                (false, true) => Geometry.EnvelopeM.FromXYMWHL(this.GetValueXY(x), this.GetValueXY(y), this.GetValueM(m), this.GetValueXY(width), this.GetValueXY(height), this.GetValueM(length)),
                (true, true) => Geometry.EnvelopeZM.FromXYZMWHDL(this.GetValueXY(x), this.GetValueXY(y), this.GetValueZ(z), this.GetValueM(m), this.GetValueXY(width), this.GetValueXY(height), this.GetValueZ(depth), this.GetValueM(length)),
            };
        }

        public T[] ReadPoints<T>(Func<double[], T> creator) => this.ReadPoints(this.ReadCount(), creator);

        public T[] ReadPoints<T>(ulong count, Func<double[], T> creator) => this.ReadPoints(readIdList: false, count, creator);

        public T[] ReadPoints<T>(out IList<long>? idList, bool readIdList, Func<double[], T> creator) => this.ReadPoints(out idList, readIdList, this.ReadCount(), creator);

        public long[] ReadIdList(ulong count)
        {
            var previousId = 0L;
            var idList = new long[count];
            for (var i = 0; i < idList.Length; i++)
            {
                idList[i] = previousId += ReadVarInt64(ref this.source);
            }

            return idList;
        }

        private static long ReadVarInt64(ref ReadOnlySpan<byte> source)
        {
            var read = ReadVarIntCount(source, 9);
            var value = DecodeZigZag(ToTarget(source, 64));
            source = source[read..];
            return value;
        }

        private static double GetValue(long input, double descale, int precision) => Math.Round(input * descale, precision, MidpointRounding.AwayFromZero);

        private readonly double GetValueXY(long input) => GetValue(input, this.descaleXY, this.precisionXY);

        private readonly double GetValueZ(long input) => GetValue(input, this.descaleZ, this.precisionZ);

        private readonly double GetValueM(long input) => GetValue(input, this.descaleM, this.precisionM);

        private T[] ReadPoints<T>(bool readIdList, ulong count, Func<double[], T> creator) => this.ReadPoints(out _, readIdList, count, creator);

        private T[] ReadPoints<T>(out IList<long>? idList, bool readIdList, ulong count, Func<double[], T> creator)
        {
            if (count is 0)
            {
                idList = null;
                return [];
            }

            // read the ID list
            idList = readIdList ? this.ReadIdList(count) : [];

            var points = new T[count];

            for (var i = 0; i < points.Length; i++)
            {
                for (var j = 0; j < this.previousCoordinates.Length; j++)
                {
                    this.previousCoordinates[j] += ReadVarInt64(ref this.source);
                }

                points[i] = (this.hasZ, this.hasM) switch
                {
                    (false, false) => creator([this.GetValueXY(this.previousCoordinates[0]), this.GetValueXY(this.previousCoordinates[1])]),
                    (true, false) => creator([this.GetValueXY(this.previousCoordinates[0]), this.GetValueXY(this.previousCoordinates[1]), this.GetValueZ(this.previousCoordinates[2])]),
                    (false, true) => creator([this.GetValueXY(this.previousCoordinates[0]), this.GetValueXY(this.previousCoordinates[1]), this.GetValueM(this.previousCoordinates[2])]),
                    (true, true) => creator([this.GetValueXY(this.previousCoordinates[0]), this.GetValueXY(this.previousCoordinates[1]), this.GetValueZ(this.previousCoordinates[2]), this.GetValueM(this.previousCoordinates[3])]),
                };
            }

            return points;
        }
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    private ref struct Writer
    {
        private readonly int startLength;
        private readonly double scaleXY;

        private readonly bool hasZ;
        private readonly double scaleZ;

        private readonly bool hasM;
        private readonly double scaleM;

        private long previousX;
        private long previousY;
        private long previousZ;
        private long previousM;

        private Span<byte> writer;

        private Writer(Span<byte> writer, TinyWkbRecordHeader header)
        {
            this.writer = writer;
            this.startLength = this.writer.Length;

            var precisionXY = header.PrecisionXY;
            this.scaleXY = precisionXY.Scale();

            this.hasZ = header.HasZ;
            var precisionZ = this.hasZ ? header.PrecisionZ : 0;
            this.scaleZ = precisionZ.Scale();

            this.hasM = header.HasM;
            var precisionM = this.hasM ? header.PrecisionM : 0;
            this.scaleM = precisionM.Scale();
        }

        public readonly int BytesWritten => this.startLength - this.writer.Length;

        public static Writer Initialise(Span<byte> destination, TinyWkbRecordHeader header)
        {
            var returnWriter = new Writer(destination, header);
            var written = TinyWkbRecordHeader.WriteHeader(header, destination);
            returnWriter.writer = returnWriter.writer[written..];
            return returnWriter;
        }

        public void Point<T>(T point, Func<T, (double X, double Y)> getXY, Func<T, double> getZ, Func<T, double> getM)
        {
            var (x, y) = getXY(point);
            this.Int64(Encode(x, this.scaleXY, ref this.previousX));

            this.Int64(Encode(y, this.scaleXY, ref this.previousY));

            if (this.hasZ)
            {
                this.Int64(Encode(getZ(point), this.scaleZ, ref this.previousZ));
            }

            if (this.hasM)
            {
                this.Int64(Encode(getM(point), this.scaleM, ref this.previousM));
            }

            static long Encode(double value, double scale, ref long lastScaledValue)
            {
                var longValue = (long)System.Math.Round(value * scale, 0, MidpointRounding.AwayFromZero);
                var encodedValue = longValue - lastScaledValue;
                lastScaledValue = longValue;
                return encodedValue;
            }
        }

        public void Points<T>(IEnumerable<T>? points, int count, Func<T, (double X, double Y)> getXY, Func<T, double> getZ, Func<T, double> getM)
        {
            if (count is 0 || points is null)
            {
                return;
            }

            this.Count((ulong)count);

            foreach (var point in points)
            {
                this.Point(point, getXY, getZ, getM);
            }
        }

        public void PointCollections<T>(IEnumerable<IReadOnlyCollection<T>>? pointCollections, int count, Func<T, (double X, double Y)> getXY, Func<T, double> getZ, Func<T, double> getM)
        {
            if (count is 0 || pointCollections is null)
            {
                return;
            }

            this.Count((ulong)count);

            foreach (var pointCollection in pointCollections)
            {
                this.Points(pointCollection, pointCollection.Count, getXY, getZ, getM);
            }
        }

        public void Count(ulong count) => this.UInt64(count);

        private static int UInt64(Span<byte> destination, ulong value)
        {
            var current = 0;
            do
            {
                var v = value & 0x7F;
                value >>= 7;

                if (value is not 0)
                {
                    v |= 0x80;
                }

                destination[current++] = (byte)v;
            }
            while (value is not 0);

            return current;
        }

        private void Int64(long value)
        {
            var bytesWritten = UInt64(this.writer, (ulong)EncodeZigZag(value, 64));
            this.writer = this.writer[bytesWritten..];
        }

        private void UInt64(ulong value)
        {
            var bytesWritten = UInt64(this.writer, value);
            this.writer = this.writer[bytesWritten..];
        }
    }
}