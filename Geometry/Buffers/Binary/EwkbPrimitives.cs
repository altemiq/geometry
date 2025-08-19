// -----------------------------------------------------------------------
// <copyright file="EwkbPrimitives.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Binary;

/// <summary>
/// The Extended Well-Known Binary primitives.
/// </summary>
public static class EwkbPrimitives
{
    private delegate int WriteHeaderDelegate<in T>(Span<byte> destination, T value, Func<WkbPrimitives.WkbGeometryType, uint> flags);

    private delegate int WriteDelegate<in T>(Span<byte> destination, T value, bool includeMetadata);

    /// <summary>
    /// Reads the value as a <see cref="Geometry.Point"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An instance of <see cref="Geometry.Point"/> if successful; otherwise <see langword="null"/>.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POINT</c>.</exception>
    public static Geometry.Point ReadPoint(ReadOnlySpan<byte> source) => ReadGeometry(ref source, WkbPrimitives.ReadPoint);

    /// <summary>
    /// Reads the value as a <see cref="Geometry.PointZ"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An instance of <see cref="Geometry.PointZ"/> if successful; otherwise <see langword="null"/>.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POINTZ</c>.</exception>
    public static Geometry.PointZ ReadPointZ(ReadOnlySpan<byte> source) => ReadGeometry(ref source, WkbPrimitives.ReadPointZ);

    /// <summary>
    /// Reads the value as a <see cref="Geometry.PointM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An instance of <see cref="Geometry.PointM"/> if successful; otherwise <see langword="null"/>.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POINTM</c>.</exception>
    public static Geometry.PointM ReadPointM(ReadOnlySpan<byte> source) => ReadGeometry(ref source, WkbPrimitives.ReadPointM);

    /// <summary>
    /// Reads the value as a <see cref="Geometry.PointZM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An instance of <see cref="Geometry.PointZM"/> if successful; otherwise <see langword="null"/>.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POINTZM</c>.</exception>
    public static Geometry.PointZM ReadPointZM(ReadOnlySpan<byte> source) => ReadGeometry(ref source, WkbPrimitives.ReadPointZM);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.Point"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An <see cref="IReadOnlyCollection{T}"/> filled with instances of <see cref="Geometry.Point"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINT</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.Point> ReadMultiPoint(ReadOnlySpan<byte> source) => ReadGeometry(ref source, WkbPrimitives.ReadMultiPoint);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PointZ"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An <see cref="IReadOnlyCollection{T}"/> filled with instances of <see cref="Geometry.PointZ"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINTZ</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PointZ> ReadMultiPointZ(ReadOnlySpan<byte> source) => ReadGeometry(ref source, WkbPrimitives.ReadMultiPointZ);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PointM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An <see cref="IReadOnlyCollection{T}"/> filled with instances of <see cref="Geometry.PointM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINTM</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PointM> ReadMultiPointM(ReadOnlySpan<byte> source) => ReadGeometry(ref source, WkbPrimitives.ReadMultiPointM);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.Point"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An <see cref="IReadOnlyCollection{T}"/> filled with instances of <see cref="Geometry.Point"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINTZM</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PointZM> ReadMultiPointZM(ReadOnlySpan<byte> source) => ReadGeometry(ref source, WkbPrimitives.ReadMultiPointZM);

    /// <summary>
    /// Reads the value as <see cref="Geometry.Polyline"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.Polyline"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>LINESTRING</c>.</exception>
    public static Geometry.Polyline ReadLineString(ReadOnlySpan<byte> source) => ReadGeometry(ref source, WkbPrimitives.ReadLineString);

    /// <summary>
    /// Reads the value as <see cref="Geometry.PolylineZ"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolylineZ"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>LINESTRINGZ</c>.</exception>
    public static Geometry.PolylineZ ReadLineStringZ(ReadOnlySpan<byte> source) => ReadGeometry(ref source, WkbPrimitives.ReadLineStringZ);

    /// <summary>
    /// Reads the value as <see cref="Geometry.PolylineM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolylineM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>LINESTRINGM</c>.</exception>
    public static Geometry.PolylineM ReadLineStringM(ReadOnlySpan<byte> source) => ReadGeometry(ref source, WkbPrimitives.ReadLineStringM);

    /// <summary>
    /// Reads the value as <see cref="Geometry.PolylineZM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolylineZM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>LINESTRINGZM</c>.</exception>
    public static Geometry.PolylineZM ReadLineStringZM(ReadOnlySpan<byte> source) => ReadGeometry(ref source, WkbPrimitives.ReadLineStringZM);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.Polyline"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.Polyline"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTILINESTRING</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.Polyline> ReadMultiLineString(ReadOnlySpan<byte> source) => ReadGeometry(ref source, WkbPrimitives.ReadMultiLineString);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PolylineZ"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolylineZ"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTILINESTRINGZ</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PolylineZ> ReadMultiLineStringZ(ReadOnlySpan<byte> source) => ReadGeometry(ref source, WkbPrimitives.ReadMultiLineStringZ);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PolylineM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolylineM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTILINESTRINGM</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PolylineM> ReadMultiLineStringM(ReadOnlySpan<byte> source) => ReadGeometry(ref source, WkbPrimitives.ReadMultiLineStringM);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PolylineZM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolylineZM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTILINESTRINGZM</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PolylineZM> ReadMultiLineStringZM(ReadOnlySpan<byte> source) => ReadGeometry(ref source, WkbPrimitives.ReadMultiLineStringZM);

    /// <summary>
    /// Reads the value as <see cref="Geometry.Polygon"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.Polygon"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POLYGON</c>.</exception>
    public static Geometry.Polygon ReadPolygon(ReadOnlySpan<byte> source) => ReadGeometry(ref source, WkbPrimitives.ReadPolygon);

    /// <summary>
    /// Reads the value as <see cref="Geometry.PolygonZ"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolygonZ"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POLYGONZ</c>.</exception>
    public static Geometry.PolygonZ ReadPolygonZ(ReadOnlySpan<byte> source) => ReadGeometry(ref source, WkbPrimitives.ReadPolygonZ);

    /// <summary>
    /// Reads the value as <see cref="Geometry.PolygonM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolygonM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POLYGONM</c>.</exception>
    public static Geometry.PolygonM ReadPolygonM(ReadOnlySpan<byte> source) => ReadGeometry(ref source, WkbPrimitives.ReadPolygonM);

    /// <summary>
    /// Reads the value as <see cref="Geometry.PolygonZM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolygonZM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POLYGONZM</c>.</exception>
    public static Geometry.PolygonZM ReadPolygonZM(ReadOnlySpan<byte> source) => ReadGeometry(ref source, WkbPrimitives.ReadPolygonZM);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.Polygon"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.Polygon"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOLYGON</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.Polygon> ReadMultiPolygon(ReadOnlySpan<byte> source) => ReadGeometry(ref source, WkbPrimitives.ReadMultiPolygon);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PolygonZ"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolygonZ"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOLYGONZ</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PolygonZ> ReadMultiPolygonZ(ReadOnlySpan<byte> source) => ReadGeometry(ref source, WkbPrimitives.ReadMultiPolygonZ);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PolygonM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolygonM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOLYGONM</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PolygonM> ReadMultiPolygonM(ReadOnlySpan<byte> source) => ReadGeometry(ref source, WkbPrimitives.ReadMultiPolygonM);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PolygonZM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolygonZM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOLYGONZM</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PolygonZM> ReadMultiPolygonZM(ReadOnlySpan<byte> source) => ReadGeometry(ref source, WkbPrimitives.ReadMultiPolygonZM);

    /// <summary>
    /// Reads the value as a geometry.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An instance of <see cref="Geometry.Point"/>, <see cref="Geometry.PointZ"/> <see cref="Geometry.PointZM"/>, <see cref="Geometry.Polyline"/>, <see cref="Geometry.PolylineZ"/> <see cref="Geometry.PolylineZM"/>, or an <see cref="IEnumerable{T}"/> fill with one of those if successful; otherwise <see langword="null"/>.</returns>
    public static Geometry.IGeometry ReadGeometry(ReadOnlySpan<byte> source) => ReadGeometry(ref source, WkbPrimitives.ReadGeometry);

    /// <summary>
    /// Reads the SRID from the value.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>The SRID from the value.</returns>
    public static int ReadSrid(ReadOnlySpan<byte> source)
    {
        var (byteOrder, geometryType) = WkbPrimitives.ReadByteOrderAndGeometryType(ref source);
        return ReadSrid(source, (int)geometryType, byteOrder);
    }

    /// <summary>
    /// Writes a <see cref="Geometry.Point"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointBigEndian(Span<byte> destination, Geometry.Point value, int srid) => Write(destination, value, srid, littleEndian: false, WkbPrimitives.WriteHeaderBigEndian, WkbPrimitives.WritePointBigEndian);

    /// <summary>
    /// Writes <see cref="Geometry.Point"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointBigEndian(Span<byte> destination, IEnumerable<Geometry.Point> values, int srid) => Write(destination, values, srid, littleEndian: false, WkbPrimitives.WriteHeaderBigEndian, WkbPrimitives.WritePointBigEndian);

    /// <summary>
    /// Writes a <see cref="Geometry.Point"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointLittleEndian(Span<byte> destination, Geometry.Point value, int srid) => Write(destination, value, srid, littleEndian: true, WkbPrimitives.WriteHeaderLittleEndian, WkbPrimitives.WritePointLittleEndian);

    /// <summary>
    /// Writes <see cref="Geometry.Point"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointLittleEndian(Span<byte> destination, IEnumerable<Geometry.Point> values, int srid) =>
        Write(destination, values, srid, littleEndian: true, WkbPrimitives.WriteHeaderLittleEndian, WkbPrimitives.WritePointLittleEndian);

    /// <summary>
    /// Writes a <see cref="Geometry.PointZ"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZBigEndian(Span<byte> destination, Geometry.PointZ value, int srid) => Write(destination, value, srid, littleEndian: false, WkbPrimitives.WriteHeaderBigEndian, WkbPrimitives.WritePointZBigEndian);

    /// <summary>
    /// Writes <see cref="Geometry.PointZ"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZBigEndian(Span<byte> destination, IEnumerable<Geometry.PointZ> values, int srid) =>
        Write(destination, values, srid, littleEndian: false, WkbPrimitives.WriteHeaderBigEndian, WkbPrimitives.WritePointZBigEndian);

    /// <summary>
    /// Writes a <see cref="Geometry.PointZ"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZLittleEndian(Span<byte> destination, Geometry.PointZ value, int srid) => Write(destination, value, srid, littleEndian: true, WkbPrimitives.WriteHeaderLittleEndian, WkbPrimitives.WritePointZLittleEndian);

    /// <summary>
    /// Writes <see cref="Geometry.PointZ"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZLittleEndian(Span<byte> destination, IEnumerable<Geometry.PointZ> values, int srid) =>
        Write(destination, values, srid, littleEndian: true, WkbPrimitives.WriteHeaderLittleEndian, WkbPrimitives.WritePointZLittleEndian);

    /// <summary>
    /// Writes a <see cref="Geometry.PointM"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointMBigEndian(Span<byte> destination, Geometry.PointM value, int srid) => Write(destination, value, srid, littleEndian: false, WkbPrimitives.WriteHeaderBigEndian, WkbPrimitives.WritePointMBigEndian);

    /// <summary>
    /// Writes <see cref="Geometry.PointM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointMBigEndian(Span<byte> destination, IEnumerable<Geometry.PointM> values, int srid) =>
        Write(destination, values, srid, littleEndian: false, WkbPrimitives.WriteHeaderBigEndian, WkbPrimitives.WritePointMBigEndian);

    /// <summary>
    /// Writes a <see cref="Geometry.PointM"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointMLittleEndian(Span<byte> destination, Geometry.PointM value, int srid) => Write(destination, value, srid, littleEndian: true, WkbPrimitives.WriteHeaderLittleEndian, WkbPrimitives.WritePointMLittleEndian);

    /// <summary>
    /// Writes <see cref="Geometry.PointM"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointMLittleEndian(Span<byte> destination, IEnumerable<Geometry.PointM> values, int srid) =>
        Write(destination, values, srid, littleEndian: true, WkbPrimitives.WriteHeaderLittleEndian, WkbPrimitives.WritePointMLittleEndian);

    /// <summary>
    /// Writes a <see cref="Geometry.PointZM"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZMBigEndian(Span<byte> destination, Geometry.PointZM value, int srid) => Write(destination, value, srid, littleEndian: false, WkbPrimitives.WriteHeaderBigEndian, WkbPrimitives.WritePointZMBigEndian);

    /// <summary>
    /// Writes <see cref="Geometry.PointZM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZMBigEndian(Span<byte> destination, IEnumerable<Geometry.PointZM> values, int srid) =>
        Write(destination, values, srid, littleEndian: false, WkbPrimitives.WriteHeaderBigEndian, WkbPrimitives.WritePointZMBigEndian);

    /// <summary>
    /// Writes a <see cref="Geometry.PointZM"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZMLittleEndian(Span<byte> destination, Geometry.PointZM value, int srid) => Write(destination, value, srid, littleEndian: true, WkbPrimitives.WriteHeaderLittleEndian, WkbPrimitives.WritePointZMLittleEndian);

    /// <summary>
    /// Writes <see cref="Geometry.PointZM"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZMLittleEndian(Span<byte> destination, IEnumerable<Geometry.PointZM> values, int srid) =>
        Write(destination, values, srid, littleEndian: true, WkbPrimitives.WriteHeaderLittleEndian, WkbPrimitives.WritePointZMLittleEndian);

    /// <summary>
    /// Writes a <see cref="Geometry.Polyline"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Polyline"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringBigEndian(Span<byte> destination, Geometry.Polyline<Geometry.Point> value, int srid) =>
        Write(destination, value, srid, littleEndian: false, WkbPrimitives.WriteHeaderBigEndian, WkbPrimitives.WriteLineStringBigEndian);

    /// <summary>
    /// Writes <see cref="Geometry.Polyline"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Polyline"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringBigEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.Point>> values, int srid) =>
        Write(destination, values, srid, littleEndian: false, WkbPrimitives.WriteHeaderBigEndian, WkbPrimitives.WriteLineStringBigEndian);

    /// <summary>
    /// Writes a <see cref="Geometry.Polyline"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Polyline"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringLittleEndian(Span<byte> destination, Geometry.Polyline<Geometry.Point> value, int srid) =>
        Write(destination, value, srid, littleEndian: true, WkbPrimitives.WriteHeaderLittleEndian, WkbPrimitives.WriteLineStringLittleEndian);

    /// <summary>
    /// Writes <see cref="Geometry.Point"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringLittleEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.Point>> values, int srid) =>
        Write(destination, values, srid, littleEndian: true, WkbPrimitives.WriteHeaderLittleEndian, WkbPrimitives.WriteLineStringLittleEndian);

    /// <summary>
    /// Writes a <see cref="Geometry.PolylineZ"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZBigEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointZ> value, int srid) =>
        Write(destination, value, srid, littleEndian: false, WkbPrimitives.WriteHeaderBigEndian, WkbPrimitives.WriteLineStringZBigEndian);

    /// <summary>
    /// Writes <see cref="Geometry.PolylineZ"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZBigEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointZ>> values, int srid) =>
        Write(destination, values, srid, littleEndian: false, WkbPrimitives.WriteHeaderBigEndian, WkbPrimitives.WriteLineStringZBigEndian);

    /// <summary>
    /// Writes a <see cref="Geometry.PolylineZ"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZLittleEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointZ> value, int srid) =>
        Write(destination, value, srid, littleEndian: true, WkbPrimitives.WriteHeaderLittleEndian, WkbPrimitives.WriteLineStringZLittleEndian);

    /// <summary>
    /// Writes <see cref="Geometry.PointZ"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZLittleEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointZ>> values, int srid) =>
        Write(destination, values, srid, littleEndian: true, WkbPrimitives.WriteHeaderLittleEndian, WkbPrimitives.WriteLineStringZLittleEndian);

    /// <summary>
    /// Writes a <see cref="Geometry.PolylineM"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringMBigEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointM> value, int srid) =>
        Write(destination, value, srid, littleEndian: false, WkbPrimitives.WriteHeaderBigEndian, WkbPrimitives.WriteLineStringMBigEndian);

    /// <summary>
    /// Writes <see cref="Geometry.PolylineM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringMBigEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointM>> values, int srid) =>
        Write(destination, values, srid, littleEndian: false, WkbPrimitives.WriteHeaderBigEndian, WkbPrimitives.WriteLineStringMBigEndian);

    /// <summary>
    /// Writes a <see cref="Geometry.PolylineM"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringMLittleEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointM> value, int srid) =>
        Write(destination, value, srid, littleEndian: true, WkbPrimitives.WriteHeaderLittleEndian, WkbPrimitives.WriteLineStringMLittleEndian);

    /// <summary>
    /// Writes <see cref="Geometry.PointM"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringMLittleEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointM>> values, int srid) =>
        Write(destination, values, srid, littleEndian: true, WkbPrimitives.WriteHeaderLittleEndian, WkbPrimitives.WriteLineStringMLittleEndian);

    /// <summary>
    /// Writes a <see cref="Geometry.PolylineZM"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZMBigEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointZM> value, int srid) =>
        Write(destination, value, srid, littleEndian: false, WkbPrimitives.WriteHeaderBigEndian, WkbPrimitives.WriteLineStringZMBigEndian);

    /// <summary>
    /// Writes <see cref="Geometry.PolylineZM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZMBigEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointZM>> values, int srid) =>
        Write(destination, values, srid, littleEndian: false, WkbPrimitives.WriteHeaderBigEndian, WkbPrimitives.WriteLineStringZMBigEndian);

    /// <summary>
    /// Writes a <see cref="Geometry.PolylineZM"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZMLittleEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointZM> value, int srid) =>
        Write(destination, value, srid, littleEndian: true, WkbPrimitives.WriteHeaderLittleEndian, WkbPrimitives.WriteLineStringZMLittleEndian);

    /// <summary>
    /// Writes <see cref="Geometry.PointZM"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZMLittleEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointZM>> values, int srid) =>
        Write(destination, values, srid, littleEndian: true, WkbPrimitives.WriteHeaderLittleEndian, WkbPrimitives.WriteLineStringZMLittleEndian);

    /// <summary>
    /// Writes a <see cref="Geometry.Polygon"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Polygon"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonBigEndian(Span<byte> destination, Geometry.Polygon<Geometry.Point> value, int srid) =>
        Write(destination, value, srid, littleEndian: false, WkbPrimitives.WriteHeaderBigEndian, WkbPrimitives.WritePolygonBigEndian);

    /// <summary>
    /// Writes <see cref="Geometry.Polygon"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Polygon"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonBigEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.Point>> values, int srid) =>
        Write(destination, values, srid, littleEndian: false, WkbPrimitives.WriteHeaderBigEndian, WkbPrimitives.WritePolygonBigEndian);

    /// <summary>
    /// Writes a <see cref="Geometry.Polygon"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Polygon"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonLittleEndian(Span<byte> destination, Geometry.Polygon<Geometry.Point> value, int srid) =>
        Write(destination, value, srid, littleEndian: true, WkbPrimitives.WriteHeaderLittleEndian, WkbPrimitives.WritePolygonLittleEndian);

    /// <summary>
    /// Writes <see cref="Geometry.Point"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonLittleEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.Point>> values, int srid) =>
        Write(destination, values, srid, littleEndian: true, WkbPrimitives.WriteHeaderLittleEndian, WkbPrimitives.WritePolygonLittleEndian);

    /// <summary>
    /// Writes a <see cref="Geometry.PolygonZ"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZBigEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointZ> value, int srid) =>
        Write(destination, value, srid, littleEndian: false, WkbPrimitives.WriteHeaderBigEndian, WkbPrimitives.WritePolygonZBigEndian);

    /// <summary>
    /// Writes <see cref="Geometry.PolygonZ"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZBigEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointZ>> values, int srid) =>
        Write(destination, values, srid, littleEndian: false, WkbPrimitives.WriteHeaderBigEndian, WkbPrimitives.WritePolygonZBigEndian);

    /// <summary>
    /// Writes a <see cref="Geometry.PolygonZ"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZLittleEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointZ> value, int srid) =>
        Write(destination, value, srid, littleEndian: true, WkbPrimitives.WriteHeaderLittleEndian, WkbPrimitives.WritePolygonZLittleEndian);

    /// <summary>
    /// Writes <see cref="Geometry.PointZ"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZLittleEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointZ>> values, int srid) =>
        Write(destination, values, srid, littleEndian: true, WkbPrimitives.WriteHeaderLittleEndian, WkbPrimitives.WritePolygonZLittleEndian);

    /// <summary>
    /// Writes a <see cref="Geometry.PolygonM"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonMBigEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointM> value, int srid) =>
        Write(destination, value, srid, littleEndian: false, WkbPrimitives.WriteHeaderBigEndian, WkbPrimitives.WritePolygonMBigEndian);

    /// <summary>
    /// Writes <see cref="Geometry.PolygonM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonMBigEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointM>> values, int srid) =>
        Write(destination, values, srid, littleEndian: false, WkbPrimitives.WriteHeaderBigEndian, WkbPrimitives.WritePolygonMBigEndian);

    /// <summary>
    /// Writes a <see cref="Geometry.PolygonM"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonMLittleEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointM> value, int srid) =>
        Write(destination, value, srid, littleEndian: true, WkbPrimitives.WriteHeaderLittleEndian, WkbPrimitives.WritePolygonMLittleEndian);

    /// <summary>
    /// Writes <see cref="Geometry.PointM"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonMLittleEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointM>> values, int srid) =>
        Write(destination, values, srid, littleEndian: true, WkbPrimitives.WriteHeaderLittleEndian, WkbPrimitives.WritePolygonMLittleEndian);

    /// <summary>
    /// Writes a <see cref="Geometry.PolygonZM"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZMBigEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointZM> value, int srid) =>
        Write(destination, value, srid, littleEndian: false, WkbPrimitives.WriteHeaderBigEndian, WkbPrimitives.WritePolygonZMBigEndian);

    /// <summary>
    /// Writes <see cref="Geometry.PolygonZM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZMBigEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointZM>> values, int srid) =>
        Write(destination, values, srid, littleEndian: false, WkbPrimitives.WriteHeaderBigEndian, WkbPrimitives.WritePolygonZMBigEndian);

    /// <summary>
    /// Writes a <see cref="Geometry.PolygonZM"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZMLittleEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointZM> value, int srid) =>
        Write(destination, value, srid, littleEndian: true, WkbPrimitives.WriteHeaderLittleEndian, WkbPrimitives.WritePolygonZMLittleEndian);

    /// <summary>
    /// Writes <see cref="Geometry.PointZM"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZMLittleEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointZM>> values, int srid) =>
        Write(destination, values, srid, littleEndian: true, WkbPrimitives.WriteHeaderLittleEndian, WkbPrimitives.WritePolygonZMLittleEndian);

    private static T ReadGeometry<T>(ref ReadOnlySpan<byte> source, WkbPrimitives.CreateFunction<T> func)
        where T : Geometry.IGeometry
    {
        var (byteOrder, geometryType) = WkbPrimitives.ReadByteOrderAndGeometryType(ref source);
        return ReadGeometry(source[4..], byteOrder, geometryType, func);
    }

    private static T ReadGeometry<T>(ReadOnlySpan<byte> span, WkbPrimitives.WkbByteOrder byteOrder, WkbPrimitives.WkbGeometryType geometryType, WkbPrimitives.CreateFunction<T> func)
    {
        return func(ref span, byteOrder, GetGeometryType((int)geometryType));

        static WkbPrimitives.WkbGeometryType GetGeometryType(int type)
        {
            var geometryType = (WkbPrimitives.WkbGeometryType)(type & 0x1FFFFFFF);
            var hasZ = (type & 0x80000000) is not 0;
            var hasM = (type & 0x40000000) is not 0;

            return (geometryType, hasZ, hasM) switch
            {
                (WkbPrimitives.WkbGeometryType.Point, false, false) => WkbPrimitives.WkbGeometryType.Point,
                (WkbPrimitives.WkbGeometryType.Point, true, false) => WkbPrimitives.WkbGeometryType.PointZ,
                (WkbPrimitives.WkbGeometryType.Point, false, true) => WkbPrimitives.WkbGeometryType.PointM,
                (WkbPrimitives.WkbGeometryType.Point, true, true) => WkbPrimitives.WkbGeometryType.PointZM,
                (WkbPrimitives.WkbGeometryType.MultiPoint, false, false) => WkbPrimitives.WkbGeometryType.MultiPoint,
                (WkbPrimitives.WkbGeometryType.MultiPoint, true, false) => WkbPrimitives.WkbGeometryType.MultiPointZ,
                (WkbPrimitives.WkbGeometryType.MultiPoint, false, true) => WkbPrimitives.WkbGeometryType.MultiPointM,
                (WkbPrimitives.WkbGeometryType.MultiPoint, true, true) => WkbPrimitives.WkbGeometryType.MultiPointZM,
                (WkbPrimitives.WkbGeometryType.LineString, false, false) => WkbPrimitives.WkbGeometryType.LineString,
                (WkbPrimitives.WkbGeometryType.LineString, true, false) => WkbPrimitives.WkbGeometryType.LineStringZ,
                (WkbPrimitives.WkbGeometryType.LineString, false, true) => WkbPrimitives.WkbGeometryType.LineStringM,
                (WkbPrimitives.WkbGeometryType.LineString, true, true) => WkbPrimitives.WkbGeometryType.LineStringZM,
                (WkbPrimitives.WkbGeometryType.MultiLineString, false, false) => WkbPrimitives.WkbGeometryType.MultiLineString,
                (WkbPrimitives.WkbGeometryType.MultiLineString, true, false) => WkbPrimitives.WkbGeometryType.MultiLineStringZ,
                (WkbPrimitives.WkbGeometryType.MultiLineString, false, true) => WkbPrimitives.WkbGeometryType.MultiLineStringM,
                (WkbPrimitives.WkbGeometryType.MultiLineString, true, true) => WkbPrimitives.WkbGeometryType.MultiLineStringZM,
                (WkbPrimitives.WkbGeometryType.Polygon, false, false) => WkbPrimitives.WkbGeometryType.Polygon,
                (WkbPrimitives.WkbGeometryType.Polygon, true, false) => WkbPrimitives.WkbGeometryType.PolygonZ,
                (WkbPrimitives.WkbGeometryType.Polygon, false, true) => WkbPrimitives.WkbGeometryType.PolygonM,
                (WkbPrimitives.WkbGeometryType.Polygon, true, true) => WkbPrimitives.WkbGeometryType.PolygonZM,
                (WkbPrimitives.WkbGeometryType.MultiPolygon, false, false) => WkbPrimitives.WkbGeometryType.MultiPolygon,
                (WkbPrimitives.WkbGeometryType.MultiPolygon, true, false) => WkbPrimitives.WkbGeometryType.MultiPolygonZ,
                (WkbPrimitives.WkbGeometryType.MultiPolygon, false, true) => WkbPrimitives.WkbGeometryType.MultiPolygonM,
                (WkbPrimitives.WkbGeometryType.MultiPolygon, true, true) => WkbPrimitives.WkbGeometryType.MultiPolygonZM,
                (WkbPrimitives.WkbGeometryType.Geometry, false, false) => WkbPrimitives.WkbGeometryType.Geometry,
                (WkbPrimitives.WkbGeometryType.Geometry, true, false) => WkbPrimitives.WkbGeometryType.GeometryZ,
                (WkbPrimitives.WkbGeometryType.Geometry, false, true) => WkbPrimitives.WkbGeometryType.GeometryM,
                (WkbPrimitives.WkbGeometryType.Geometry, true, true) => WkbPrimitives.WkbGeometryType.GeometryZM,
                (WkbPrimitives.WkbGeometryType.GeometryCollection, false, false) => WkbPrimitives.WkbGeometryType.GeometryCollection,
                (WkbPrimitives.WkbGeometryType.GeometryCollection, true, false) => WkbPrimitives.WkbGeometryType.GeometryCollectionZ,
                (WkbPrimitives.WkbGeometryType.GeometryCollection, false, true) => WkbPrimitives.WkbGeometryType.GeometryCollectionM,
                (WkbPrimitives.WkbGeometryType.GeometryCollection, true, true) => WkbPrimitives.WkbGeometryType.GeometryCollectionZM,
                _ => throw new NotSupportedException(),
            };
        }
    }

    private static int ReadSrid(ReadOnlySpan<byte> source, int type, WkbPrimitives.WkbByteOrder byteOrder)
    {
        if ((type & 0x20000000) is not 0)
        {
            // read the SRID
            return byteOrder switch
            {
                WkbPrimitives.WkbByteOrder.Ndr => System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(source),
                WkbPrimitives.WkbByteOrder.Xdr => System.Buffers.Binary.BinaryPrimitives.ReadInt32BigEndian(source),
                _ => throw new InvalidOperationException(),
            };
        }

        return 0;
    }

    private static int Write<T>(Span<byte> destination, IEnumerable<T> values, int srid, bool littleEndian, WriteHeaderDelegate<ICollection<T>> headerWriter, WriteDelegate<IEnumerable<T>> writer)
    {
        // write the header
        ICollection<T> collection = [..values];
        var written = headerWriter(destination, collection, TransformGeometryType);

        // write the SRID
        written += WriteValue(destination[written..], srid, littleEndian);

        // write the count
        written += WriteValue(destination[written..], (uint)collection.Count, littleEndian);

        // write the geometry
        written += writer(destination[written..], collection, includeMetadata: false);
        return written;
    }

    private static int Write<T>(Span<byte> destination, T value, int srid, bool littleEndian, WriteHeaderDelegate<T> headerWriter, WriteDelegate<T> writer)
    {
        // write the header
        var written = headerWriter(destination, value, TransformGeometryType);

        // write the SRID
        written += WriteValue(destination[written..], srid, littleEndian);

        if (value is System.Collections.ICollection { Count: var count })
        {
            // write the count
            written += WriteValue(destination[written..], (uint)count, littleEndian);
        }

        // write the geometry
        written += writer(destination[written..], value, includeMetadata: false);
        return written;
    }

    private static int WriteValue(Span<byte> span, int value, bool littleEndian)
    {
        if (littleEndian)
        {
            System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(span, value);
        }
        else
        {
            System.Buffers.Binary.BinaryPrimitives.WriteInt32BigEndian(span, value);
        }

        return sizeof(uint);
    }

    private static int WriteValue(Span<byte> span, uint value, bool littleEndian)
    {
        if (littleEndian)
        {
            System.Buffers.Binary.BinaryPrimitives.WriteUInt32LittleEndian(span, value);
        }
        else
        {
            System.Buffers.Binary.BinaryPrimitives.WriteUInt32BigEndian(span, value);
        }

        return sizeof(uint);
    }

    private static uint TransformGeometryType(WkbPrimitives.WkbGeometryType wkbGeometryType)
    {
        var (type, hasZ, hasM) = wkbGeometryType switch
        {
            < WkbPrimitives.WkbGeometryType.GeometryZ => ((uint)wkbGeometryType % 1000U, false, false),
            < WkbPrimitives.WkbGeometryType.GeometryM => ((uint)wkbGeometryType % 1000U, true, false),
            < WkbPrimitives.WkbGeometryType.GeometryZM => ((uint)wkbGeometryType % 1000U, false, true),
            >= WkbPrimitives.WkbGeometryType.GeometryZM => ((uint)wkbGeometryType % 1000U, true, true),
        };

        if (hasZ)
        {
            type |= 0x80000000;
        }

        if (hasM)
        {
            type |= 0x40000000;
        }

        type |= 0x20000000;

        return type;
    }
}