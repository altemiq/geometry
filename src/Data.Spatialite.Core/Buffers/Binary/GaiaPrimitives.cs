// -----------------------------------------------------------------------
// <copyright file="GaiaPrimitives.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Binary;

/// <summary>
/// The GAIA Binary primitives.
/// </summary>
public static class GaiaPrimitives
{
    private delegate T ReadPointDelegate<out T>(ref ReadOnlySpan<byte> span, bool littleEndian);

    private delegate T CreateFunction<out T>(ref ReadOnlySpan<byte> span, GaiaGeometryType type, bool littleEndian);

    private delegate (double MinX, double MinY, double MaxX, double MaxY) Writer<in T>(ref Span<byte> span, bool isLittleEndian, T value, bool includeMetadata, out int bytesWritten);

    private delegate (double MinX, double MinY, double MaxX, double MaxY) Writer(ref Span<byte> span, bool isLittleEndian, out int bytesWritten);

    private delegate (double X, double T) GetXYFunc<in T>(T point);

    private delegate bool TryGetValue<in T>(T point, out double value);

    /// <summary>
    /// Reads the value as a <see cref="Geometry.Point"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An instance of <see cref="Geometry.Point"/> if successful; otherwise <see langword="null"/>.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POINT</c>.</exception>
    public static Geometry.Point ReadPoint(ReadOnlySpan<byte> source) => ReadPoint(ref source, ReadPoint);

    /// <summary>
    /// Reads the value as a <see cref="Geometry.PointZ"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An instance of <see cref="Geometry.PointZ"/> if successful; otherwise <see langword="null"/>.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POINTZ</c>.</exception>
    public static Geometry.PointZ ReadPointZ(ReadOnlySpan<byte> source) => ReadPoint(ref source, ReadPointZ);

    /// <summary>
    /// Reads the value as a <see cref="Geometry.PointM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An instance of <see cref="Geometry.PointM"/> if successful; otherwise <see langword="null"/>.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POINTM</c>.</exception>
    public static Geometry.PointM ReadPointM(ReadOnlySpan<byte> source) => ReadPoint(ref source, ReadPointM);

    /// <summary>
    /// Reads the value as a <see cref="Geometry.PointZM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An instance of <see cref="Geometry.PointZM"/> if successful; otherwise <see langword="null"/>.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POINTZM</c>.</exception>
    public static Geometry.PointZM ReadPointZM(ReadOnlySpan<byte> source) => ReadPoint(ref source, ReadPointZM);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.Point"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An <see cref="IReadOnlyCollection{T}"/> filled with instances of <see cref="Geometry.Point"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINT</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.Point> ReadMultiPoint(ReadOnlySpan<byte> source) => ReadMulti(source, ReadPoint);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PointZ"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An <see cref="IReadOnlyCollection{T}"/> filled with instances of <see cref="Geometry.PointZ"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINTZ</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PointZ> ReadMultiPointZ(ReadOnlySpan<byte> source) => ReadMulti(source, ReadPointZ);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PointM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An <see cref="IReadOnlyCollection{T}"/> filled with instances of <see cref="Geometry.PointM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINTM</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PointM> ReadMultiPointM(ReadOnlySpan<byte> source) => ReadMulti(source, ReadPointM);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.Point"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An <see cref="IReadOnlyCollection{T}"/> filled with instances of <see cref="Geometry.Point"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINTZM</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PointZM> ReadMultiPointZM(ReadOnlySpan<byte> source) => ReadMulti(source, ReadPointZM);

    /// <summary>
    /// Reads the value as <see cref="Geometry.Polyline"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.Polyline"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>LINESTRING</c>.</exception>
    public static Geometry.Polyline ReadLineString(ReadOnlySpan<byte> source) => ReadLineString(ref source, ReadPolyline);

    /// <summary>
    /// Reads the value as <see cref="Geometry.PolylineZ"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolylineZ"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>LINESTRINGZ</c>.</exception>
    public static Geometry.PolylineZ ReadLineStringZ(ReadOnlySpan<byte> source) => ReadLineString(ref source, ReadPolylineZ);

    /// <summary>
    /// Reads the value as <see cref="Geometry.PolylineM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolylineM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>LINESTRINGM</c>.</exception>
    public static Geometry.PolylineM ReadLineStringM(ReadOnlySpan<byte> source) => ReadLineString(ref source, ReadPolylineM);

    /// <summary>
    /// Reads the value as <see cref="Geometry.PolylineZM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolylineZM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>LINESTRINGZM</c>.</exception>
    public static Geometry.PolylineZM ReadLineStringZM(ReadOnlySpan<byte> source) => ReadLineString(ref source, ReadPolylineZM);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.Polyline"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.Polyline"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTILINESTRING</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.Polyline> ReadMultiLineString(ReadOnlySpan<byte> source) => ReadMulti(source, ReadPolyline);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PolylineZ"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolylineZ"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTILINESTRINGZ</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PolylineZ> ReadMultiLineStringZ(ReadOnlySpan<byte> source) => ReadMulti(source, ReadPolylineZ);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PolylineM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolylineM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTILINESTRINGM</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PolylineM> ReadMultiLineStringM(ReadOnlySpan<byte> source) => ReadMulti(source, ReadPolylineM);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PolylineZM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolylineZM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTILINESTRINGZM</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PolylineZM> ReadMultiLineStringZM(ReadOnlySpan<byte> source) => ReadMulti(source, ReadPolylineZM);

    /// <summary>
    /// Reads the value as <see cref="Geometry.Polygon"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.Polygon"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POLYGON</c>.</exception>
    public static Geometry.Polygon ReadPolygon(ReadOnlySpan<byte> source) => ReadPolygon(ref source, ReadPolygon);

    /// <summary>
    /// Reads the value as <see cref="Geometry.PolygonZ"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolygonZ"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POLYGONZ</c>.</exception>
    public static Geometry.PolygonZ ReadPolygonZ(ReadOnlySpan<byte> source) => ReadPolygon(ref source, ReadPolygonZ);

    /// <summary>
    /// Reads the value as <see cref="Geometry.PolygonM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolygonM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POLYGONM</c>.</exception>
    public static Geometry.PolygonM ReadPolygonM(ReadOnlySpan<byte> source) => ReadPolygon(ref source, ReadPolygonM);

    /// <summary>
    /// Reads the value as <see cref="Geometry.PolygonZM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolygonZM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POLYGONZM</c>.</exception>
    public static Geometry.PolygonZM ReadPolygonZM(ReadOnlySpan<byte> source) => ReadPolygon(ref source, ReadPolygonZM);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.Polygon"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.Polygon"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOLYGON</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.Polygon> ReadMultiPolygon(ReadOnlySpan<byte> source) => ReadMulti(source, ReadPolygon);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PolygonZ"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolygonZ"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOLYGONZ</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PolygonZ> ReadMultiPolygonZ(ReadOnlySpan<byte> source) => ReadMulti(source, ReadPolygonZ);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PolygonM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolygonM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOLYGONM</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PolygonM> ReadMultiPolygonM(ReadOnlySpan<byte> source) => ReadMulti(source, ReadPolygonM);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PolygonZM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolygonZM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOLYGONZM</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PolygonZM> ReadMultiPolygonZM(ReadOnlySpan<byte> source) => ReadMulti(source, ReadPolygonZM);

    /// <summary>
    /// Reads the value as a geometry.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An instance of <see cref="Geometry.Point"/>, <see cref="Geometry.PointZ"/> <see cref="Geometry.PointZM"/>, <see cref="Geometry.Polyline"/>, <see cref="Geometry.PolylineZ"/> <see cref="Geometry.PolylineZM"/>, or an <see cref="IEnumerable{T}"/> fill with one of those if successful; otherwise <see langword="null"/>.</returns>
    public static Geometry.IGeometry ReadGeometry(ReadOnlySpan<byte> source)
    {
        var (successful, _, littleEndian, _, type) = ReadHeader(ref source);
        if (!successful)
        {
            throw new InvalidDataException();
        }

        Geometry.IGeometry returnValue = ToBase(type) switch
        {
            GaiaGeometryType.Point => ReadPoint(ref source, type, littleEndian),
            GaiaGeometryType.PointZ => ReadPointZ(ref source, type, littleEndian),
            GaiaGeometryType.PointM => ReadPointM(ref source, type, littleEndian),
            GaiaGeometryType.PointZM => ReadPointZM(ref source, type, littleEndian),
            GaiaGeometryType.MultiPoint => ReadMultiImpl(ref source, littleEndian, ReadPoint),
            GaiaGeometryType.MultiPointZ => ReadMultiImpl(ref source, littleEndian, ReadPointZ),
            GaiaGeometryType.MultiPointM => ReadMultiImpl(ref source, littleEndian, ReadPointM),
            GaiaGeometryType.MultiPointZM => ReadMultiImpl(ref source, littleEndian, ReadPointZM),
            GaiaGeometryType.LineString => ReadPolyline(ref source, type, littleEndian),
            GaiaGeometryType.LineStringZ => ReadPolylineZ(ref source, type, littleEndian),
            GaiaGeometryType.LineStringM => ReadPolylineM(ref source, type, littleEndian),
            GaiaGeometryType.LineStringZM => ReadPolylineZM(ref source, type, littleEndian),
            GaiaGeometryType.MultiLineString => ReadMultiImpl(ref source, littleEndian, ReadPolyline),
            GaiaGeometryType.MultiLineStringZ => ReadMultiImpl(ref source, littleEndian, ReadPolylineZ),
            GaiaGeometryType.MultiLineStringM => ReadMultiImpl(ref source, littleEndian, ReadPolylineM),
            GaiaGeometryType.MultiLineStringZM => ReadMultiImpl(ref source, littleEndian, ReadPolylineZM),
            GaiaGeometryType.Polygon => ReadPolygon(ref source, type, littleEndian),
            GaiaGeometryType.PolygonZ => ReadPolygonZ(ref source, type, littleEndian),
            GaiaGeometryType.PolygonM => ReadPolygonM(ref source, type, littleEndian),
            GaiaGeometryType.PolygonZM => ReadPolygonZM(ref source, type, littleEndian),
            GaiaGeometryType.MultiPolygon => ReadMultiImpl(ref source, littleEndian, ReadPolygon),
            GaiaGeometryType.MultiPolygonZ => ReadMultiImpl(ref source, littleEndian, ReadPolygonZ),
            GaiaGeometryType.MultiPolygonM => ReadMultiImpl(ref source, littleEndian, ReadPolygonM),
            GaiaGeometryType.MultiPolygonZM => ReadMultiImpl(ref source, littleEndian, ReadPolygonZM),
            _ => throw new Geometry.InvalidGeometryTypeException(),
        };

        return CheckEnd(source, returnValue);
    }

    /// <summary>
    /// Writes a <see cref="Geometry.Point"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointBigEndian(Span<byte> destination, Geometry.Point value, int srid) => Write(ref destination, littleEndian: false, value, GetXY, TryGetZ, TryGetM, srid);

    /// <summary>
    /// Writes <see cref="Geometry.Point"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointBigEndian(Span<byte> destination, IEnumerable<Geometry.Point> values, int srid) =>
        Write(ref destination, littleEndian: false, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, values, GaiaGeometryType.MultiPoint, GetPointType, Write, out bytesWritten));

    /// <summary>
    /// Writes a <see cref="Geometry.Point"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointLittleEndian(Span<byte> destination, Geometry.Point value, int srid) => Write(ref destination, littleEndian: true, value, GetXY, TryGetZ, TryGetM, srid);

    /// <summary>
    /// Writes <see cref="Geometry.Point"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointLittleEndian(Span<byte> destination, IEnumerable<Geometry.Point> values, int srid) =>
        Write(ref destination, littleEndian: true, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, values, GaiaGeometryType.MultiPoint, GetPointType, Write, out bytesWritten));

    /// <summary>
    /// Writes a <see cref="Geometry.PointZ"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZBigEndian(Span<byte> destination, Geometry.PointZ value, int srid) => Write(ref destination, littleEndian: false, value, GetXY, TryGetZ, TryGetM, srid);

    /// <summary>
    /// Writes <see cref="Geometry.PointZ"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZBigEndian(Span<byte> destination, IEnumerable<Geometry.PointZ> values, int srid) =>
        Write(ref destination, littleEndian: false, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, values, GaiaGeometryType.MultiPoint, GetPointType, Write, out bytesWritten));

    /// <summary>
    /// Writes a <see cref="Geometry.PointZ"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZLittleEndian(Span<byte> destination, Geometry.PointZ value, int srid) => Write(ref destination, littleEndian: true, value, GetXY, TryGetZ, TryGetM, srid);

    /// <summary>
    /// Writes <see cref="Geometry.PointZ"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZLittleEndian(Span<byte> destination, IEnumerable<Geometry.PointZ> values, int srid) =>
        Write(ref destination, littleEndian: true, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, values, GaiaGeometryType.MultiPoint, GetPointType, Write, out bytesWritten));

    /// <summary>
    /// Writes a <see cref="Geometry.PointM"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointMBigEndian(Span<byte> destination, Geometry.PointM value, int srid) => Write(ref destination, littleEndian: false, value, GetXY, TryGetZ, TryGetM, srid);

    /// <summary>
    /// Writes <see cref="Geometry.PointM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointMBigEndian(Span<byte> destination, IEnumerable<Geometry.PointM> values, int srid) =>
        Write(ref destination, littleEndian: false, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, values, GaiaGeometryType.MultiPoint, GetPointType, Write, out bytesWritten));

    /// <summary>
    /// Writes a <see cref="Geometry.PointM"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointMLittleEndian(Span<byte> destination, Geometry.PointM value, int srid) => Write(ref destination, littleEndian: true, value, GetXY, TryGetZ, TryGetM, srid);

    /// <summary>
    /// Writes <see cref="Geometry.PointM"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointMLittleEndian(Span<byte> destination, IEnumerable<Geometry.PointM> values, int srid) =>
        Write(ref destination, littleEndian: true, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, values, GaiaGeometryType.MultiPoint, GetPointType, Write, out bytesWritten));

    /// <summary>
    /// Writes a <see cref="Geometry.PointZM"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZMBigEndian(Span<byte> destination, Geometry.PointZM value, int srid) => Write(ref destination, littleEndian: false, value, GetXY, TryGetZ, TryGetM, srid);

    /// <summary>
    /// Writes <see cref="Geometry.PointZM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZMBigEndian(Span<byte> destination, IEnumerable<Geometry.PointZM> values, int srid) =>
        Write(ref destination, littleEndian: false, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, values, GaiaGeometryType.MultiPoint, GetPointType, Write, out bytesWritten));

    /// <summary>
    /// Writes a <see cref="Geometry.PointZM"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZMLittleEndian(Span<byte> destination, Geometry.PointZM value, int srid) => Write(ref destination, littleEndian: true, value, GetXY, TryGetZ, TryGetM, srid);

    /// <summary>
    /// Writes <see cref="Geometry.PointZM"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZMLittleEndian(Span<byte> destination, IEnumerable<Geometry.PointZM> values, int srid) =>
        Write(ref destination, littleEndian: true, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, values, GaiaGeometryType.MultiPoint, GetPointType, Write, out bytesWritten));

    /// <summary>
    /// Writes a <see cref="Geometry.Polyline"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Polyline"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringBigEndian(Span<byte> destination, Geometry.Polyline<Geometry.Point> value, int srid) =>
        Write(ref destination, littleEndian: false, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, value, includeMetadata: true, Write, out bytesWritten));

    /// <summary>
    /// Writes <see cref="Geometry.Polyline"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Polyline"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringBigEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.Point>> values, int srid) =>
        Write(ref destination, littleEndian: false, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, values, GaiaGeometryType.MultiLineString, GetPolylineType, Write, out bytesWritten));

    /// <summary>
    /// Writes a <see cref="Geometry.Polyline"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Polyline"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringLittleEndian(Span<byte> destination, Geometry.Polyline<Geometry.Point> value, int srid) =>
        Write(ref destination, littleEndian: true, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, value, includeMetadata: true, Write, out bytesWritten));

    /// <summary>
    /// Writes <see cref="Geometry.Point"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringLittleEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.Point>> values, int srid) =>
        Write(ref destination, littleEndian: true, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, values, GaiaGeometryType.MultiLineString, GetPolylineType, Write, out bytesWritten));

    /// <summary>
    /// Writes a <see cref="Geometry.PolylineZ"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZBigEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointZ> value, int srid) =>
        Write(ref destination, littleEndian: false, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, value, includeMetadata: true, Write, out bytesWritten));

    /// <summary>
    /// Writes <see cref="Geometry.PolylineZ"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZBigEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointZ>> values, int srid) =>
        Write(ref destination, littleEndian: false, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, values, GaiaGeometryType.MultiLineString, GetPolylineType, Write, out bytesWritten));

    /// <summary>
    /// Writes a <see cref="Geometry.PolylineZ"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZLittleEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointZ> value, int srid) =>
        Write(ref destination, littleEndian: true, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, value, includeMetadata: true, Write, out bytesWritten));

    /// <summary>
    /// Writes <see cref="Geometry.PointZ"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZLittleEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointZ>> values, int srid) =>
        Write(ref destination, littleEndian: true, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, values, GaiaGeometryType.MultiLineString, GetPolylineType, Write, out bytesWritten));

    /// <summary>
    /// Writes a <see cref="Geometry.PolylineM"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringMBigEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointM> value, int srid) =>
        Write(ref destination, littleEndian: false, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, value, includeMetadata: true, Write, out bytesWritten));

    /// <summary>
    /// Writes <see cref="Geometry.PolylineM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringMBigEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointM>> values, int srid) =>
        Write(ref destination, littleEndian: false, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, values, GaiaGeometryType.MultiLineString, GetPolylineType, Write, out bytesWritten));

    /// <summary>
    /// Writes a <see cref="Geometry.PolylineM"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringMLittleEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointM> value, int srid) =>
        Write(ref destination, littleEndian: true, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, value, includeMetadata: true, Write, out bytesWritten));

    /// <summary>
    /// Writes <see cref="Geometry.PointM"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringMLittleEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointM>> values, int srid) =>
        Write(ref destination, littleEndian: true, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, values, GaiaGeometryType.MultiLineString, GetPolylineType, Write, out bytesWritten));

    /// <summary>
    /// Writes a <see cref="Geometry.PolylineZM"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZMBigEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointZM> value, int srid) =>
        Write(ref destination, littleEndian: false, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, value, includeMetadata: true, Write, out bytesWritten));

    /// <summary>
    /// Writes <see cref="Geometry.PolylineZM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZMBigEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointZM>> values, int srid) =>
        Write(ref destination, littleEndian: false, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, values, GaiaGeometryType.MultiLineString, GetPolylineType, Write, out bytesWritten));

    /// <summary>
    /// Writes a <see cref="Geometry.PolylineZM"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZMLittleEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointZM> value, int srid) =>
        Write(ref destination, littleEndian: true, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, value, includeMetadata: true, Write, out bytesWritten));

    /// <summary>
    /// Writes <see cref="Geometry.PointZM"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZMLittleEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointZM>> values, int srid) =>
        Write(ref destination, littleEndian: true, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, values, GaiaGeometryType.MultiLineString, GetPolylineType, Write, out bytesWritten));

    /// <summary>
    /// Writes a <see cref="Geometry.Polygon"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Polygon"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonBigEndian(Span<byte> destination, Geometry.Polygon<Geometry.Point> value, int srid) =>
        Write(ref destination, littleEndian: false, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, value, includeMetadata: true, Write, out bytesWritten));

    /// <summary>
    /// Writes <see cref="Geometry.Polygon"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Polygon"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonBigEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.Point>> values, int srid) =>
        Write(ref destination, littleEndian: false, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, values, GaiaGeometryType.MultiPolygon, GetPolygonType, Write, out bytesWritten));

    /// <summary>
    /// Writes a <see cref="Geometry.Polygon"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Polygon"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonLittleEndian(Span<byte> destination, Geometry.Polygon<Geometry.Point> value, int srid) =>
        Write(ref destination, littleEndian: true, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, value, includeMetadata: true, Write, out bytesWritten));

    /// <summary>
    /// Writes <see cref="Geometry.Point"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonLittleEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.Point>> values, int srid) =>
        Write(ref destination, littleEndian: true, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, values, GaiaGeometryType.MultiPolygon, GetPolygonType, Write, out bytesWritten));

    /// <summary>
    /// Writes a <see cref="Geometry.PolygonZ"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZBigEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointZ> value, int srid) =>
        Write(ref destination, littleEndian: false, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, value, includeMetadata: true, Write, out bytesWritten));

    /// <summary>
    /// Writes <see cref="Geometry.PolygonZ"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZBigEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointZ>> values, int srid) =>
        Write(ref destination, littleEndian: false, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, values, GaiaGeometryType.MultiPolygon, GetPolygonType, Write, out bytesWritten));

    /// <summary>
    /// Writes a <see cref="Geometry.PolygonZ"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZLittleEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointZ> value, int srid) =>
        Write(ref destination, littleEndian: true, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, value, includeMetadata: true, Write, out bytesWritten));

    /// <summary>
    /// Writes <see cref="Geometry.PointZ"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZLittleEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointZ>> values, int srid) =>
        Write(ref destination, littleEndian: true, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, values, GaiaGeometryType.MultiPolygon, GetPolygonType, Write, out bytesWritten));

    /// <summary>
    /// Writes a <see cref="Geometry.PolygonM"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonMBigEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointM> value, int srid) =>
        Write(ref destination, littleEndian: false, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, value, includeMetadata: true, Write, out bytesWritten));

    /// <summary>
    /// Writes <see cref="Geometry.PolygonM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonMBigEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointM>> values, int srid) =>
        Write(ref destination, littleEndian: false, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, values, GaiaGeometryType.MultiPolygon, GetPolygonType, Write, out bytesWritten));

    /// <summary>
    /// Writes a <see cref="Geometry.PolygonM"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonMLittleEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointM> value, int srid) =>
        Write(ref destination, littleEndian: true, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, value, includeMetadata: true, Write, out bytesWritten));

    /// <summary>
    /// Writes <see cref="Geometry.PointM"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonMLittleEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointM>> values, int srid) =>
        Write(ref destination, littleEndian: true, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, values, GaiaGeometryType.MultiPolygon, GetPolygonType, Write, out bytesWritten));

    /// <summary>
    /// Writes a <see cref="Geometry.PolygonZM"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZMBigEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointZM> value, int srid) =>
        Write(ref destination, littleEndian: false, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, value, includeMetadata: true, Write, out bytesWritten));

    /// <summary>
    /// Writes <see cref="Geometry.PolygonZM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZMBigEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointZM>> values, int srid) =>
        Write(ref destination, littleEndian: false, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, values, GaiaGeometryType.MultiPolygon, GetPolygonType, Write, out bytesWritten));

    /// <summary>
    /// Writes a <see cref="Geometry.PolygonZM"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZMLittleEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointZM> value, int srid) =>
        Write(ref destination, littleEndian: true, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, value, includeMetadata: true, Write, out bytesWritten));

    /// <summary>
    /// Writes <see cref="Geometry.PointZM"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="srid">The SRID value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZMLittleEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointZM>> values, int srid) =>
        Write(ref destination, littleEndian: true, srid, (ref span, littleEndian, out bytesWritten) => Write(ref span, littleEndian, values, GaiaGeometryType.MultiPolygon, GetPolygonType, Write, out bytesWritten));

    /// <summary>
    /// Gets a value indicating whether the specified bytes represent a GAIA geometry.
    /// </summary>
    /// <param name="span">The span to check.</param>
    /// <returns><see langword="true"/> if <paramref name="span"/> represents a GAIA geometry; otherwise <see langword="false"/>.</returns>
    internal static bool IsValid(ReadOnlySpan<byte> span)
    {
        (var successful, _, _, _, _) = ReadHeader(ref span);
        return successful;
    }

    private static T ReadPoint<T>(ref ReadOnlySpan<byte> span, CreateFunction<T> func)
    {
        var (successful, _, byteOrder, _, type) = ReadHeader(ref span);
        if (!successful)
        {
            throw new InvalidDataException();
        }

        var point = func(ref span, type, byteOrder);
        return CheckEnd(span, point);
    }

    private static Geometry.Point ReadPoint(ref ReadOnlySpan<byte> span, GaiaGeometryType type, bool littleEndian)
    {
        CheckType(type, GaiaGeometryType.Point);
        return ReadPoint(ref span, littleEndian);
    }

    private static Geometry.Point ReadPoint(ref ReadOnlySpan<byte> span, bool littleEndian)
    {
        var point = new Geometry.Point(ReadDouble(span, littleEndian), ReadDouble(span[8..], littleEndian));
        span = span[16..];
        return point;
    }

    private static Geometry.PointZ ReadPointZ(ref ReadOnlySpan<byte> span, GaiaGeometryType type, bool littleEndian)
    {
        CheckType(type, GaiaGeometryType.PointZ);
        return ReadPointZ(ref span, littleEndian);
    }

    private static Geometry.PointZ ReadPointZ(ref ReadOnlySpan<byte> span, bool littleEndian)
    {
        var point = new Geometry.PointZ(ReadDouble(span, littleEndian), ReadDouble(span[8..], littleEndian), ReadDouble(span[16..], littleEndian));
        span = span[24..];
        return point;
    }

    private static Geometry.PointM ReadPointM(ref ReadOnlySpan<byte> span, GaiaGeometryType type, bool littleEndian)
    {
        CheckType(type, GaiaGeometryType.PointM);
        return ReadPointM(ref span, littleEndian);
    }

    private static Geometry.PointM ReadPointM(ref ReadOnlySpan<byte> span, bool littleEndian)
    {
        var point = new Geometry.PointM(ReadDouble(span, littleEndian), ReadDouble(span[8..], littleEndian), ReadDouble(span[16..], littleEndian));
        span = span[24..];
        return point;
    }

    private static Geometry.PointZM ReadPointZM(ref ReadOnlySpan<byte> span, GaiaGeometryType type, bool littleEndian)
    {
        CheckType(type, GaiaGeometryType.PointZM);
        return ReadPointZM(ref span, littleEndian);
    }

    private static Geometry.PointZM ReadPointZM(ref ReadOnlySpan<byte> span, bool littleEndian)
    {
        var point = new Geometry.PointZM(ReadDouble(span, littleEndian), ReadDouble(span[8..], littleEndian), ReadDouble(span[16..], littleEndian), ReadDouble(span[24..], littleEndian));
        span = span[32..];
        return point;
    }

    private static TPolyline ReadLineString<TPolyline>(ref ReadOnlySpan<byte> span, CreateFunction<TPolyline> func)
    {
        var (successful, _, byteOrder, _, type) = ReadHeader(ref span);
        if (!successful)
        {
            throw new InvalidDataException();
        }

        var lineString = func(ref span, type, byteOrder);
        return CheckEnd(span, lineString);
    }

    private static T[] ReadLineString<T>(ref ReadOnlySpan<byte> span, GaiaGeometryType type, ReadPointDelegate<T> func, bool littleEndian)
    {
        CheckType(ToBase(type), GaiaGeometryType.LineString);
        return ReadPointsImpl(ref span, func, littleEndian);
    }

    private static Geometry.Polyline ReadPolyline(ref ReadOnlySpan<byte> span, GaiaGeometryType type, bool littleEndian) => [.. ReadLineString(ref span, type, ReadPoint, littleEndian)];

    private static Geometry.PolylineZ ReadPolylineZ(ref ReadOnlySpan<byte> span, GaiaGeometryType type, bool littleEndian) => [.. ReadLineString(ref span, type, ReadPointZ, littleEndian)];

    private static Geometry.PolylineM ReadPolylineM(ref ReadOnlySpan<byte> span, GaiaGeometryType type, bool littleEndian) => [.. ReadLineString(ref span, type, ReadPointM, littleEndian)];

    private static Geometry.PolylineZM ReadPolylineZM(ref ReadOnlySpan<byte> span, GaiaGeometryType type, bool littleEndian) => [.. ReadLineString(ref span, type, ReadPointZM, littleEndian)];

    private static TPolygon ReadPolygon<TPolygon>(ref ReadOnlySpan<byte> span, CreateFunction<TPolygon> func)
    {
        var (successful, _, byteOrder, _, type) = ReadHeader(ref span);
        if (!successful)
        {
            throw new InvalidDataException();
        }

        var polygon = func(ref span, type, byteOrder);
        return CheckEnd(span, polygon);
    }

    private static Geometry.LinearRing<T>[] ReadPolygon<T>(ref ReadOnlySpan<byte> span, GaiaGeometryType type, ReadPointDelegate<T> func, bool littleEndian)
        where T : struct
    {
        CheckType(ToBase(type), GaiaGeometryType.Polygon);

        var number = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span);
        span = span[4..];

        var rings = new Geometry.LinearRing<T>[number];
        for (var i = 0; i < number; i++)
        {
            rings[i] = [.. ReadPointsImpl(ref span, func, littleEndian)];
        }

        return rings;
    }

    private static Geometry.Polygon ReadPolygon(ref ReadOnlySpan<byte> span, GaiaGeometryType type, bool littleEndian) => [.. ReadPolygon(ref span, type, ReadPoint, littleEndian)];

    private static Geometry.PolygonZ ReadPolygonZ(ref ReadOnlySpan<byte> span, GaiaGeometryType type, bool littleEndian) => [.. ReadPolygon(ref span, type, ReadPointZ, littleEndian)];

    private static Geometry.PolygonM ReadPolygonM(ref ReadOnlySpan<byte> span, GaiaGeometryType type, bool littleEndian) => [.. ReadPolygon(ref span, type, ReadPointM, littleEndian)];

    private static Geometry.PolygonZM ReadPolygonZM(ref ReadOnlySpan<byte> span, GaiaGeometryType type, bool littleEndian) => [.. ReadPolygon(ref span, type, ReadPointZM, littleEndian)];

    private static T[] ReadPointsImpl<T>(ref ReadOnlySpan<byte> span, ReadPointDelegate<T> func, bool littleEndian)
    {
        var count = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span);
        span = span[4..];
        return ReadPointsImpl(ref span, func, littleEndian, count);
    }

    private static T[] ReadPointsImpl<T>(ref ReadOnlySpan<byte> span, ReadPointDelegate<T> func, bool littleEndian, int count)
    {
        var points = new T[count];

        for (var i = 0; i < count; i++)
        {
            points[i] = func(ref span, littleEndian);
        }

        return points;
    }

    private static Geometry.IMultiGeometry<T> ReadMultiImpl<T>(ref ReadOnlySpan<byte> span, bool littleEndian, CreateFunction<T> creationFunction)
        where T : Geometry.IGeometry
    {
        var count = ReadInt32(span, littleEndian);
        span = span[4..];

        var items = new T[count];
        for (var i = 0; i < count; i++)
        {
            if (span[0] is not GaiaConstants.BlobMark.Entity)
            {
                throw new InvalidOperationException();
            }

            span = span[1..];

            var geometryType = (GaiaGeometryType)ReadInt32(span, littleEndian);
            span = span[4..];
            items[i] = creationFunction(ref span, geometryType, littleEndian);
        }

        return [.. CheckEnd(span, items)];
    }

    [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(value))]
    private static T CheckEnd<T>(ReadOnlySpan<byte> span, [System.Diagnostics.CodeAnalysis.MaybeNull] T value) => span[0] is GaiaConstants.BlobMark.End ? value! : throw new InvalidOperationException(Data.Spatialite.Properties.Resources.LastByteWasNotTheEndMarker);

    private static GaiaGeometryType ToBase(GaiaGeometryType type) => (GaiaGeometryType)((int)type % 1000);

    private static double ReadDouble(ReadOnlySpan<byte> span, bool littleEndian) =>
        BitConverter.Int64BitsToDouble(littleEndian ? System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(span) : System.Buffers.Binary.BinaryPrimitives.ReadInt64BigEndian(span));

    private static int ReadInt32(ReadOnlySpan<byte> span, bool littleEndian) => littleEndian ? System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span) : System.Buffers.Binary.BinaryPrimitives.ReadInt32BigEndian(span);

    private static void CheckType(GaiaGeometryType actual, GaiaGeometryType expected)
    {
        if (actual != expected)
        {
            throw new Geometry.InvalidGeometryTypeException();
        }
    }

    private static Geometry.IMultiGeometry<T> ReadMulti<T>(ReadOnlySpan<byte> span, CreateFunction<T> creationFunction)
        where T : Geometry.IGeometry
    {
        var (successful, _, littleEndian, _, type) = ReadHeader(ref span);
        if (!successful)
        {
            throw new Geometry.InvalidGeometryTypeException();
        }

        // see if this is one of the multi-geometries
        if (ToBase(type) <= GaiaGeometryType.Polygon)
        {
            throw new Geometry.InvalidGeometryTypeException();
        }

        return ReadMultiImpl(ref span, littleEndian, creationFunction);
    }

    private static (bool Successful, int Srid, bool LittleEndian, double[] Envelope, GaiaGeometryType Type) ReadHeader(ref ReadOnlySpan<byte> span)
    {
#pragma warning disable MA0194
        if (span.Length < 45 || span[0] is not GaiaConstants.BlobMark.Start || span[38] is not GaiaConstants.BlobMark.Mbr)
        {
            // cannot be an internal BLOB WKB geometry, or failed to recognize START signature or MBR
            return default;
        }
#pragma warning restore MA0194

        var littleEndian = span[1] is 1;
        var srid = ReadInt32(span[2..], littleEndian);

        var envelope = new[] { ReadDouble(span[6..14], littleEndian), ReadDouble(span[14..22], littleEndian), ReadDouble(span[22..30], littleEndian), ReadDouble(span[30..38], littleEndian), };

        var geometryType = (GaiaGeometryType)ReadInt32(span[39..], littleEndian);

        span = span[43..];

        return (Successful: true, srid, littleEndian, envelope, geometryType);
    }

    private static GaiaGeometryType GetPointType<T>(T point) => point switch
    {
        Geometry.PointZM => GaiaGeometryType.PointZM,
        Geometry.PointM => GaiaGeometryType.PointM,
        Geometry.PointZ => GaiaGeometryType.PointZ,
        Geometry.Point => GaiaGeometryType.Point,
        _ => throw new Geometry.InvalidGeometryTypeException(),
    };

    private static GaiaGeometryType GetPointType<T>(T point, GaiaGeometryType geometryType) => GetPointType(point) + (geometryType - GaiaGeometryType.Point);

    private static GaiaGeometryType GetPolylineType<T>(Geometry.Polyline<T> line, GaiaGeometryType geometryType) => GetPointType(line[0], geometryType);

    private static GaiaGeometryType GetLinearRingType<T>(Geometry.LinearRing<T> line, GaiaGeometryType geometryType)
        where T : struct => GetPointType(line[0], geometryType);

    private static GaiaGeometryType GetPolygonType<T>(Geometry.Polygon<T> polygon, GaiaGeometryType geometryType)
        where T : struct => GetLinearRingType(polygon[0], geometryType);

    private static (double X, double Y) GetXY(Geometry.Point point) => (point.X, point.Y);

    private static (double X, double Y) GetXY(Geometry.PointZ point) => (point.X, point.Y);

    private static (double X, double Y) GetXY(Geometry.PointM point) => (point.X, point.Y);

    private static (double X, double Y) GetXY(Geometry.PointZM point) => (point.X, point.Y);

    private static bool TryGetZ(Geometry.Point point, out double z)
    {
        z = default;
        return false;
    }

    private static bool TryGetZ(Geometry.PointZ point, out double z)
    {
        z = point.Z;
        return true;
    }

    private static bool TryGetZ(Geometry.PointM point, out double z)
    {
        z = default;
        return false;
    }

    private static bool TryGetZ(Geometry.PointZM point, out double z)
    {
        z = point.Z;
        return true;
    }

    private static bool TryGetM(Geometry.Point point, out double m)
    {
        m = default;
        return false;
    }

    private static bool TryGetM(Geometry.PointZ point, out double m)
    {
        m = default;
        return false;
    }

    private static bool TryGetM(Geometry.PointM point, out double m)
    {
        m = point.Measurement;
        return true;
    }

    private static bool TryGetM(Geometry.PointZM point, out double m)
    {
        m = point.Measurement;
        return true;
    }

    private static (double X, double Y) Min((double X, double Y) first, (double X, double Y)? second) => second is not { } secondValue ? first : (Math.Min(first.X, secondValue.X), Math.Min(first.Y, secondValue.Y));

    private static (double X, double Y) Max((double X, double Y) first, (double X, double Y)? second) => second is not { } secondValue ? first : (Math.Max(first.X, secondValue.X), Math.Max(first.Y, secondValue.Y));

    private static (double MinX, double MinY, double MaxX, double MaxY) Envelope((double MinX, double MinY, double MaxX, double MaxY) first, (double MinX, double MinY, double MaxX, double MaxY) second) =>
        (Math.Min(first.MinX, second.MinX), Math.Min(first.MinY, second.MinY), Math.Max(first.MaxX, second.MaxX), Math.Max(first.MaxY, second.MaxY));

    private static void WriteInt32(Span<byte> span, bool littleEndian, int value)
    {
        if (littleEndian)
        {
            System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(span, value);
        }
        else
        {
            System.Buffers.Binary.BinaryPrimitives.WriteInt32BigEndian(span, value);
        }
    }

    private static void WriteDouble(Span<byte> span, bool littleEndian, double value)
    {
        if (littleEndian)
        {
            System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span, BitConverter.DoubleToInt64Bits(value));
        }
        else
        {
            System.Buffers.Binary.BinaryPrimitives.WriteInt64BigEndian(span, BitConverter.DoubleToInt64Bits(value));
        }
    }

    private static (double MinX, double MinY, double MaxX, double MaxY) Write(ref Span<byte> span, bool littleEndian, Geometry.Point point, bool includeMetadata, out int bytesWritten) => Write(ref span, littleEndian, point, includeMetadata, GetXY, TryGetZ, TryGetM, out bytesWritten);

    private static (double MinX, double MinY, double MaxX, double MaxY) Write(ref Span<byte> span, bool littleEndian, Geometry.PointZ point, bool includeMetadata, out int bytesWritten) => Write(ref span, littleEndian, point, includeMetadata, GetXY, TryGetZ, TryGetM, out bytesWritten);

    private static (double MinX, double MinY, double MaxX, double MaxY) Write(ref Span<byte> span, bool littleEndian, Geometry.PointM point, bool includeMetadata, out int bytesWritten) => Write(ref span, littleEndian, point, includeMetadata, GetXY, TryGetZ, TryGetM, out bytesWritten);

    private static (double MinX, double MinY, double MaxX, double MaxY) Write(ref Span<byte> span, bool littleEndian, Geometry.PointZM point, bool includeMetadata, out int bytesWritten) => Write(ref span, littleEndian, point, includeMetadata, GetXY, TryGetZ, TryGetM, out bytesWritten);

    private static (double MinX, double MinY, double MaxX, double MaxY) Write(ref Span<byte> span, bool littleEndian, Geometry.Polyline<Geometry.Point> ring, bool includeMetadata, out int bytesWritten) => Write(ref span, littleEndian, ring, GaiaGeometryType.LineString, includeMetadata, GetPointType, Write, out bytesWritten);

    private static (double MinX, double MinY, double MaxX, double MaxY) Write(ref Span<byte> span, bool littleEndian, Geometry.Polyline<Geometry.PointZ> ring, bool includeMetadata, out int bytesWritten) => Write(ref span, littleEndian, ring, GaiaGeometryType.LineString, includeMetadata, GetPointType, Write, out bytesWritten);

    private static (double MinX, double MinY, double MaxX, double MaxY) Write(ref Span<byte> span, bool littleEndian, Geometry.Polyline<Geometry.PointM> ring, bool includeMetadata, out int bytesWritten) => Write(ref span, littleEndian, ring, GaiaGeometryType.LineString, includeMetadata, GetPointType, Write, out bytesWritten);

    private static (double MinX, double MinY, double MaxX, double MaxY) Write(ref Span<byte> span, bool littleEndian, Geometry.Polyline<Geometry.PointZM> ring, bool includeMetadata, out int bytesWritten) => Write(ref span, littleEndian, ring, GaiaGeometryType.LineString, includeMetadata, GetPointType, Write, out bytesWritten);

    private static (double MinX, double MinY, double MaxX, double MaxY) Write(ref Span<byte> span, bool littleEndian, Geometry.Polygon<Geometry.Point> polygon, bool includeMetadata, out int bytesWritten) => Write(ref span, littleEndian, polygon, GaiaGeometryType.Polygon, includeMetadata, GetLinearRingType, Write, out bytesWritten);

    private static (double MinX, double MinY, double MaxX, double MaxY) Write(ref Span<byte> span, bool littleEndian, Geometry.Polygon<Geometry.PointZ> polygon, bool includeMetadata, out int bytesWritten) => Write(ref span, littleEndian, polygon, GaiaGeometryType.Polygon, includeMetadata, GetLinearRingType, Write, out bytesWritten);

    private static (double MinX, double MinY, double MaxX, double MaxY) Write(ref Span<byte> span, bool littleEndian, Geometry.Polygon<Geometry.PointM> polygon, bool includeMetadata, out int bytesWritten) => Write(ref span, littleEndian, polygon, GaiaGeometryType.Polygon, includeMetadata, GetLinearRingType, Write, out bytesWritten);

    private static (double MinX, double MinY, double MaxX, double MaxY) Write(ref Span<byte> span, bool littleEndian, Geometry.Polygon<Geometry.PointZM> ring, bool includeMetadata, out int bytesWritten) => Write(ref span, littleEndian, ring, GaiaGeometryType.LineString, includeMetadata, GetLinearRingType, Write, out bytesWritten);

    private static (double MinX, double MinY, double MaxX, double MaxY) Write(ref Span<byte> span, bool littleEndian, Geometry.LinearRing<Geometry.Point> ring, bool includeMetadata, out int bytesWritten) => Write(ref span, littleEndian, ring, GaiaGeometryType.LineString, includeMetadata, GetPointType, Write, out bytesWritten);

    private static (double MinX, double MinY, double MaxX, double MaxY) Write(ref Span<byte> span, bool littleEndian, Geometry.LinearRing<Geometry.PointZ> ring, bool includeMetadata, out int bytesWritten) => Write(ref span, littleEndian, ring, GaiaGeometryType.LineString, includeMetadata, GetPointType, Write, out bytesWritten);

    private static (double MinX, double MinY, double MaxX, double MaxY) Write(ref Span<byte> span, bool littleEndian, Geometry.LinearRing<Geometry.PointM> ring, bool includeMetadata, out int bytesWritten) => Write(ref span, littleEndian, ring, GaiaGeometryType.LineString, includeMetadata, GetPointType, Write, out bytesWritten);

    private static (double MinX, double MinY, double MaxX, double MaxY) Write(ref Span<byte> span, bool littleEndian, Geometry.LinearRing<Geometry.PointZM> ring, bool includeMetadata, out int bytesWritten) => Write(ref span, littleEndian, ring, GaiaGeometryType.LineString, includeMetadata, GetPointType, Write, out bytesWritten);

    private static int Write(ref Span<byte> span, bool littleEndian, int srid, Writer writer) => WriteHeader(ref span, littleEndian, srid, writer) + WriteFooter(ref span);

    private static int Write<T>(ref Span<byte> span, bool littleEndian, T point, GetXYFunc<T> getXY, TryGetValue<T> getZ, TryGetValue<T> getM, int srid)
    {
        var totalWritten = WriteHeader(ref span, littleEndian, srid, point, point, getXY);
        _ = Write(ref span, littleEndian, point, includeMetadata: true, getXY, getZ, getM, out var bytesWritten);
        return totalWritten + bytesWritten + WriteFooter(ref span);
    }

    private static (double MinX, double MinY, double MaxX, double MaxY) Write<T>(ref Span<byte> span, bool littleEndian, T point, bool includeMetadata, GetXYFunc<T> getXYFunc, TryGetValue<T> getZ, TryGetValue<T> getM, out int bytesWritten)
    {
        var (x, y) = getXYFunc(point);
        var components = new List<double> { x, y };
        var hasZ = false;
        var hasM = false;

        if (getZ(point, out var z))
        {
            hasZ = true;
            components.Add(z);
        }

        if (getM(point, out var m))
        {
            hasM = true;
            components.Add(m);
        }

        var geometryType = (hasZ, hasM) switch
        {
            (true, true) => GaiaGeometryType.PointZM,
            (false, true) => GaiaGeometryType.PointM,
            (true, false) => GaiaGeometryType.PointZ,
            (false, false) => GaiaGeometryType.Point,
        };

        var totalBytes = 0;
        if (includeMetadata)
        {
            WriteInt32(span, littleEndian, (int)geometryType);
            span = span[sizeof(GaiaGeometryType)..];
            totalBytes += sizeof(GaiaGeometryType);
        }

        for (var i = 0; i < components.Count; i++)
        {
            WriteDouble(span, littleEndian, components[i]);
            span = span[sizeof(double)..];
            totalBytes += sizeof(double);
        }

        bytesWritten = totalBytes;
        return (x, y, x, y);
    }

    private static (double MinX, double MinY, double MaxX, double MaxY) Write<T>(ref Span<byte> span, bool littleEndian, Geometry.Polyline<T> points, bool includeMetadata, Writer<T> writer, out int bytesWritten) => Write(ref span, littleEndian, points, GaiaGeometryType.LineString, includeMetadata, GetPointType, writer, out bytesWritten);

    private static (double MinX, double MinY, double MaxX, double MaxY) Write<T>(ref Span<byte> span, bool littleEndian, IEnumerable<T> points, GaiaGeometryType geometryType, bool includeMetadata, Func<T, GaiaGeometryType, GaiaGeometryType> getGeometryType, Writer<T> writer, out int bytesWritten)
    {
        var totalBytes = 0;
        var originalSpan = span;
        var typePosition = includeMetadata ? 0 : -1;
        var countPosition = includeMetadata ? sizeof(GaiaGeometryType) : 0;
        if (points is IList<T> list)
        {
            if (includeMetadata)
            {
                // write the type
                geometryType = getGeometryType(list[0], geometryType);
                WriteInt32(span, littleEndian, (int)geometryType);
                span = span[sizeof(GaiaGeometryType)..];
                totalBytes += sizeof(GaiaGeometryType);
                typePosition = -1;
            }

            WriteInt32(span, littleEndian, list.Count);
            span = span[sizeof(int)..];
            totalBytes += sizeof(int);
            countPosition = -1;
        }

        var count = 0;
        var min = (X: double.MaxValue, Y: double.MaxValue);
        var max = (X: double.MinValue, Y: double.MinValue);
        foreach (var point in points)
        {
            if (typePosition is 0)
            {
                geometryType = GetPointType(point) + (geometryType - GaiaGeometryType.Point);
                WriteInt32(originalSpan[typePosition..], littleEndian, (int)geometryType);
                totalBytes += sizeof(GaiaGeometryType);
                typePosition = -1;
            }

            var (minX, minY, maxX, maxY) = writer(ref span, littleEndian, point, includeMetadata: false, out var written);
            totalBytes += written;
            min = Min(min, (minX, minY));
            max = Max(max, (maxX, maxY));
            count++;
        }

        if (countPosition >= 0)
        {
            WriteInt32(originalSpan[countPosition..], littleEndian, count);
            totalBytes += sizeof(int);
        }

        bytesWritten = totalBytes;
        return min.X is double.MaxValue ? default : (MinX: min.X, MinY: min.Y, MaxX: max.X, MaxY: max.Y);
    }

    private static (double MinX, double MinY, double MaxX, double MaxY) Write<T>(ref Span<byte> span, bool littleEndian, Geometry.Polygon<T> polygon, bool includeMetadata, Writer<T> writer, out int bytesWritten)
        where T : struct
    {
        var totalBytes = 0;
        if (includeMetadata)
        {
            var point = polygon.Points.Count is 0
                ? default
                : polygon.Points[0];
            var geometryType = GetPointType(point) + (GaiaGeometryType.Polygon - GaiaGeometryType.Point);
            WriteInt32(span, littleEndian, (int)geometryType);
            span = span[sizeof(GaiaGeometryType)..];
            totalBytes += sizeof(GaiaGeometryType);
        }

        WriteInt32(span, littleEndian, polygon.Count);
        span = span[sizeof(int)..];
        totalBytes += sizeof(int);
        var envelope = (MinX: double.MaxValue, MinY: double.MaxValue, MaxX: double.MinValue, MaxY: double.MinValue);
        foreach (var ring in polygon)
        {
            envelope = Envelope(envelope, Write(ref span, littleEndian, ring, GaiaGeometryType.LineString, includeMetadata: false, GetPointType, writer, out var written));
            totalBytes += written;
        }

        bytesWritten = totalBytes;
        return envelope;
    }

    private static (double MinX, double MinY, double MaxX, double MaxY) Write<T>(ref Span<byte> span, bool littleEndian, IEnumerable<T> values, GaiaGeometryType geometryType, Func<T, GaiaGeometryType, GaiaGeometryType> getType, Writer<T> writer, out int bytesWritten)
    {
        var totalBytes = 0;
        var originalSpan = span;
        var typePosition = 0;
        var countPosition = typePosition + sizeof(GaiaGeometryType);
        if (values is IList<T> list)
        {
            // write the type
            geometryType = getType(list[0], geometryType);

            WriteInt32(span, littleEndian, (int)geometryType);
            totalBytes += 4;
            typePosition = -1;

            WriteInt32(span[4..], littleEndian, list.Count);
            countPosition = -1;
            totalBytes += 4;
        }

        // write the element out
        span = span[8..];
        var count = 0;
        var envelope = (MinX: double.MaxValue, MinY: double.MaxValue, MaxX: double.MinValue, MaxY: double.MinValue);
        foreach (var value in values)
        {
            if (typePosition is 0)
            {
                WriteInt32(originalSpan.Slice(typePosition, sizeof(int)), littleEndian, (int)getType(value, geometryType));
                totalBytes += 4;
                typePosition = -1;
            }

            span[0] = GaiaConstants.BlobMark.Entity;
            totalBytes++;
            span = span[1..];
            envelope = Envelope(envelope, writer(ref span, littleEndian, value, includeMetadata: true, out var written));
            totalBytes += written;
            count++;
        }

        if (countPosition is not -1)
        {
            WriteInt32(originalSpan.Slice(countPosition, sizeof(int)), littleEndian, count);
            totalBytes += 4;
        }

        bytesWritten = totalBytes;
        return envelope;
    }

    private static int WriteHeader<T>(ref Span<byte> span, bool isLittleEndian, int srid, T min, T max, GetXYFunc<T> getXY)
    {
        var (minX, minY) = getXY(min);
        var (maxX, maxY) = getXY(max);
        return WriteHeader(
            ref span,
            isLittleEndian,
            srid,
            (ref _, _, out bytesWritten) =>
            {
                bytesWritten = 0;
                return (minX, minY, maxX, maxY);
            });
    }

    private static int WriteHeader(ref Span<byte> span, bool isLittleEndian, int srid, Writer writer)
    {
        var original = span;
        span[0] = GaiaConstants.BlobMark.Start;
        span[1] = isLittleEndian ? (byte)1 : (byte)0;

        // SRID
        WriteInt32(span[2..6], isLittleEndian, srid);

        span[38] = GaiaConstants.BlobMark.Mbr;

        span = span[39..];

        // write the geometry
        var (minX, minY, maxX, maxY) = writer(ref span, isLittleEndian, out var bytesWritten);

        // write the envelope
        WriteDouble(original[6..14], isLittleEndian, minX);
        WriteDouble(original[14..22], isLittleEndian, minY);
        WriteDouble(original[22..30], isLittleEndian, maxX);
        WriteDouble(original[30..38], isLittleEndian, maxY);

        return bytesWritten + 39;
    }

    private static int WriteFooter(ref Span<byte> span)
    {
        span[0] = GaiaConstants.BlobMark.End;
        span = span[1..];
        return 1;
    }
}