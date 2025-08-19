// -----------------------------------------------------------------------
// <copyright file="WkbPrimitives.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Binary;

using static System.Buffers.Binary.BinaryPrimitives;

using SizeAndType = System.ValueTuple<int, bool, bool, uint>;
using XY = System.ValueTuple<double, double>;

/// <summary>
/// The Well-Known Binary primitives.
/// </summary>
public static class WkbPrimitives
{
    /// <summary>
    /// Creates a geometry instance.
    /// </summary>
    /// <typeparam name="T">The type of geometry.</typeparam>
    /// <param name="span">The read-only span to read.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    /// <returns>The created geometry.</returns>
    internal delegate T CreateFunction<out T>(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType);

    private delegate T CreatePointFunction<out T>(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder);

    /// <summary>
    /// The Well-Known Binary byte order.
    /// </summary>
    internal enum WkbByteOrder : byte
    {
        /// <summary>
        /// Big Endian.
        /// </summary>
        Xdr = 0,

        /// <summary>
        /// Little Endian.
        /// </summary>
        Ndr = 1,
    }

    /// <summary>
    /// The WKB integer codes.
    /// </summary>
    internal enum WkbGeometryType : uint
    {
        /// <summary>
        /// 2D geometry.
        /// </summary>
        Geometry = 0,

        /// <summary>
        /// 2D point.
        /// </summary>
        Point = 1,

        /// <summary>
        /// 2D line string.
        /// </summary>
        LineString = 2,

        /// <summary>
        /// 2D polygon.
        /// </summary>
        Polygon = 3,

        /// <summary>
        /// 2D multi-point.
        /// </summary>
        MultiPoint = 4,

        /// <summary>
        /// 2D multi-line string.
        /// </summary>
        MultiLineString = 5,

        /// <summary>
        /// 2D multi-polygon.
        /// </summary>
        MultiPolygon = 6,

        /// <summary>
        /// 2D geometry collection.
        /// </summary>
        GeometryCollection = 7,

        /// <summary>
        /// 2D circular string.
        /// </summary>
        CircularString = 8,

        /// <summary>
        /// 2D compound curve.
        /// </summary>
        CompoundCurve = 9,

        /// <summary>
        /// 2D curve polygon.
        /// </summary>
        CurvePolygon = 10,

        /// <summary>
        /// 2D multi-curve.
        /// </summary>
        MultiCurve = 11,

        /// <summary>
        /// 2D multi-surface.
        /// </summary>
        MultiSurface = 12,

        /// <summary>
        /// 2D curve.
        /// </summary>
        Curve = 13,

        /// <summary>
        /// 2D surface.
        /// </summary>
        Surface = 14,

        /// <summary>
        /// 2D polyhedral surface.
        /// </summary>
        PolyhedralSurface = 15,

        /// <summary>
        /// 2D TIN.
        /// </summary>
        Tin = 16,

        /// <summary>
        /// Geometry with Z value.
        /// </summary>
        GeometryZ = 1000,

        /// <summary>
        /// Point with Z value.
        /// </summary>
        PointZ = 1001,

        /// <summary>
        /// LineString with Z value.
        /// </summary>
        LineStringZ = 1002,

        /// <summary>
        /// Polygon with Z value.
        /// </summary>
        PolygonZ = 1003,

        /// <summary>
        /// Multipoint with Z value.
        /// </summary>
        MultiPointZ = 1004,

        /// <summary>
        /// Multi-line string with Z value.
        /// </summary>
        MultiLineStringZ = 1005,

        /// <summary>
        /// Multi-polygon with Z value.
        /// </summary>
        MultiPolygonZ = 1006,

        /// <summary>
        /// Geometry collection with Z value.
        /// </summary>
        GeometryCollectionZ = 1007,

        /// <summary>
        /// Circular-string with Z value.
        /// </summary>
        CircularStringZ = 1008,

        /// <summary>
        /// Compound-curve with Z value.
        /// </summary>
        CompoundCurveZ = 1009,

        /// <summary>
        /// Curve-polygon with Z value.
        /// </summary>
        CurvePolygonZ = 1010,

        /// <summary>
        /// Multi-curve with Z value.
        /// </summary>
        MultiCurveZ = 1011,

        /// <summary>
        /// Multi-surface with Z value.
        /// </summary>
        MultiSurfaceZ = 1012,

        /// <summary>
        /// Curve with Z value.
        /// </summary>
        CurveZ = 1013,

        /// <summary>
        /// Surface with Z value.
        /// </summary>
        SurfaceZ = 1014,

        /// <summary>
        /// Polyhedral Surface with Z value.
        /// </summary>
        PolyhedralSurfaceZ = 1015,

        /// <summary>
        /// TIN with Z value.
        /// </summary>
        TinZ = 1016,

        /// <summary>
        /// Geometry with M value.
        /// </summary>
        GeometryM = 2000,

        /// <summary>
        /// Point with M value.
        /// </summary>
        PointM = 2001,

        /// <summary>
        /// LineString with M value.
        /// </summary>
        LineStringM = 2002,

        /// <summary>
        /// Polygon with M value.
        /// </summary>
        PolygonM = 2003,

        /// <summary>
        /// Multipoint with M value.
        /// </summary>
        MultiPointM = 2004,

        /// <summary>
        /// Multi-line string with M value.
        /// </summary>
        MultiLineStringM = 2005,

        /// <summary>
        /// Multi-polygon with M value.
        /// </summary>
        MultiPolygonM = 2006,

        /// <summary>
        /// Geometry collection with M value.
        /// </summary>
        GeometryCollectionM = 2007,

        /// <summary>
        /// Circular-string with M value.
        /// </summary>
        CircularStringM = 2008,

        /// <summary>
        /// Compound-curve with M value.
        /// </summary>
        CompoundCurveM = 2009,

        /// <summary>
        /// Curve-polygon with M value.
        /// </summary>
        CurvePolygonM = 2010,

        /// <summary>
        /// Multi-curve with M value.
        /// </summary>
        MultiCurveM = 2011,

        /// <summary>
        /// Multi-surface with M value.
        /// </summary>
        MultiSurfaceM = 2012,

        /// <summary>
        /// Curve with M value.
        /// </summary>
        CurveM = 2013,

        /// <summary>
        /// Surface with M value.
        /// </summary>
        SurfaceM = 2014,

        /// <summary>
        /// Polyhedral Surface with M value.
        /// </summary>
        PolyhedralSurfaceM = 2015,

        /// <summary>
        /// TIN with M value.
        /// </summary>
        TinM = 2016,

        /// <summary>
        /// Geometry with Z and Z and M values.
        /// </summary>
        GeometryZM = 3000,

        /// <summary>
        /// Point with Z and M values.
        /// </summary>
        PointZM = 3001,

        /// <summary>
        /// LineString with Z and M values.
        /// </summary>
        LineStringZM = 3002,

        /// <summary>
        /// Polygon with Z and M values.
        /// </summary>
        PolygonZM = 3003,

        /// <summary>
        /// Multipoint with Z and M values.
        /// </summary>
        MultiPointZM = 3004,

        /// <summary>
        /// Multi-line string with Z and M values.
        /// </summary>
        MultiLineStringZM = 3005,

        /// <summary>
        /// Multi-polygon with Z and M values.
        /// </summary>
        MultiPolygonZM = 3006,

        /// <summary>
        /// Geometry collection with Z and M values.
        /// </summary>
        GeometryCollectionZM = 3007,

        /// <summary>
        /// Circular-string with Z and M values.
        /// </summary>
        CircularStringZM = 3008,

        /// <summary>
        /// Compound-curve with Z and M values.
        /// </summary>
        CompoundCurveZM = 3009,

        /// <summary>
        /// Curve-polygon with Z and M values.
        /// </summary>
        CurvePolygonZM = 3010,

        /// <summary>
        /// Multi-curve with Z and M values.
        /// </summary>
        MultiCurveZM = 3011,

        /// <summary>
        /// Multi-surface with Z and M values.
        /// </summary>
        MultiSurfaceZM = 3012,

        /// <summary>
        /// Curve with Z and M values.
        /// </summary>
        CurveZM = 3013,

        /// <summary>
        /// Surface with Z and M values.
        /// </summary>
        SurfaceZM = 3014,

        /// <summary>
        /// Polyhedral Surface with Z and M values.
        /// </summary>
        PolyhedralSurfaceZM = 3015,

        /// <summary>
        /// TIN with Z and M values.
        /// </summary>
        TinZM = 3016,
    }

    /// <summary>
    /// Reads the value as a <see cref="Geometry.Point"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An instance of <see cref="Geometry.Point"/> if successful; otherwise <see langword="null"/>.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POINT</c>.</exception>
    public static Geometry.Point ReadPoint(ReadOnlySpan<byte> source) => ReadGeometry(ref source, ReadPoint);

    /// <summary>
    /// Reads the value as a <see cref="Geometry.PointZ"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An instance of <see cref="Geometry.PointZ"/> if successful; otherwise <see langword="null"/>.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POINTZ</c>.</exception>
    public static Geometry.PointZ ReadPointZ(ReadOnlySpan<byte> source) => ReadGeometry(ref source, ReadPointZ);

    /// <summary>
    /// Reads the value as a <see cref="Geometry.PointM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An instance of <see cref="Geometry.PointM"/> if successful; otherwise <see langword="null"/>.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POINTM</c>.</exception>
    public static Geometry.PointM ReadPointM(ReadOnlySpan<byte> source) => ReadGeometry(ref source, ReadPointM);

    /// <summary>
    /// Reads the value as a <see cref="Geometry.PointZM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An instance of <see cref="Geometry.PointZM"/> if successful; otherwise <see langword="null"/>.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POINTZM</c>.</exception>
    public static Geometry.PointZM ReadPointZM(ReadOnlySpan<byte> source) => ReadGeometry(ref source, ReadPointZM);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.Point"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An <see cref="IReadOnlyCollection{T}"/> filled with instances of <see cref="Geometry.Point"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINT</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.Point> ReadMultiPoint(ReadOnlySpan<byte> source) => ReadGeometry(ref source, ReadMultiPoint);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PointZ"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An <see cref="IReadOnlyCollection{T}"/> filled with instances of <see cref="Geometry.PointZ"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINTZ</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PointZ> ReadMultiPointZ(ReadOnlySpan<byte> source) => ReadGeometry(ref source, ReadMultiPointZ);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PointM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An <see cref="IReadOnlyCollection{T}"/> filled with instances of <see cref="Geometry.PointM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINTM</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PointM> ReadMultiPointM(ReadOnlySpan<byte> source) => ReadGeometry(ref source, ReadMultiPointM);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.Point"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An <see cref="IReadOnlyCollection{T}"/> filled with instances of <see cref="Geometry.Point"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINTZM</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PointZM> ReadMultiPointZM(ReadOnlySpan<byte> source) => ReadGeometry(ref source, ReadMultiPointZM);

    /// <summary>
    /// Reads the value as <see cref="Geometry.Polyline"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.Polyline"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>LINESTRING</c>.</exception>
    public static Geometry.Polyline ReadLineString(ReadOnlySpan<byte> source) => ReadGeometry(ref source, ReadLineString);

    /// <summary>
    /// Reads the value as <see cref="Geometry.PolylineZ"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolylineZ"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>LINESTRINGZ</c>.</exception>
    public static Geometry.PolylineZ ReadLineStringZ(ReadOnlySpan<byte> source) => ReadGeometry(ref source, ReadLineStringZ);

    /// <summary>
    /// Reads the value as <see cref="Geometry.PolylineM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolylineM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>LINESTRINGM</c>.</exception>
    public static Geometry.PolylineM ReadLineStringM(ReadOnlySpan<byte> source) => ReadGeometry(ref source, ReadLineStringM);

    /// <summary>
    /// Reads the value as <see cref="Geometry.PolylineZM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolylineZM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>LINESTRINGZM</c>.</exception>
    public static Geometry.PolylineZM ReadLineStringZM(ReadOnlySpan<byte> source) => ReadGeometry(ref source, ReadLineStringZM);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.Polyline"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.Polyline"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTILINESTRING</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.Polyline> ReadMultiLineString(ReadOnlySpan<byte> source) => ReadGeometry(ref source, ReadMultiLineString);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PolylineZ"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolylineZ"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTILINESTRINGZ</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PolylineZ> ReadMultiLineStringZ(ReadOnlySpan<byte> source) => ReadGeometry(ref source, ReadMultiLineStringZ);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PolylineM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolylineM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTILINESTRINGM</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PolylineM> ReadMultiLineStringM(ReadOnlySpan<byte> source) => ReadGeometry(ref source, ReadMultiLineStringM);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PolylineZM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolylineZM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTILINESTRINGZM</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PolylineZM> ReadMultiLineStringZM(ReadOnlySpan<byte> source) => ReadGeometry(ref source, ReadMultiLineStringZM);

    /// <summary>
    /// Reads the value as <see cref="Geometry.Polygon"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.Polygon"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POLYGON</c>.</exception>
    public static Geometry.Polygon ReadPolygon(ReadOnlySpan<byte> source) => ReadGeometry(ref source, ReadPolygon);

    /// <summary>
    /// Reads the value as <see cref="Geometry.PolygonZ"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolygonZ"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POLYGONZ</c>.</exception>
    public static Geometry.PolygonZ ReadPolygonZ(ReadOnlySpan<byte> source) => ReadGeometry(ref source, ReadPolygonZ);

    /// <summary>
    /// Reads the value as <see cref="Geometry.PolygonM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolygonM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POLYGONM</c>.</exception>
    public static Geometry.PolygonM ReadPolygonM(ReadOnlySpan<byte> source) => ReadGeometry(ref source, ReadPolygonM);

    /// <summary>
    /// Reads the value as <see cref="Geometry.PolygonZM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolygonZM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>POLYGONZM</c>.</exception>
    public static Geometry.PolygonZM ReadPolygonZM(ReadOnlySpan<byte> source) => ReadGeometry(ref source, ReadPolygonZM);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.Polygon"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.Polygon"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOLYGON</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.Polygon> ReadMultiPolygon(ReadOnlySpan<byte> source) => ReadGeometry(ref source, ReadMultiPolygon);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PolygonZ"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolygonZ"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOLYGONZ</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PolygonZ> ReadMultiPolygonZ(ReadOnlySpan<byte> source) => ReadGeometry(ref source, ReadMultiPolygonZ);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PolygonM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolygonM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOLYGONM</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PolygonM> ReadMultiPolygonM(ReadOnlySpan<byte> source) => ReadGeometry(ref source, ReadMultiPolygonM);

    /// <summary>
    /// Reads the value as a collection of <see cref="Geometry.PolygonZM"/>.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>A <see cref="Geometry.PolygonZM"/> if successful.</returns>
    /// <exception cref="Geometry.InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOLYGONZM</c>.</exception>
    public static Geometry.IMultiGeometry<Geometry.PolygonZM> ReadMultiPolygonZM(ReadOnlySpan<byte> source) => ReadGeometry(ref source, ReadMultiPolygonZM);

    /// <summary>
    /// Reads the value as a geometry.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>An instance of <see cref="Geometry.Point"/>, <see cref="Geometry.PointZ"/> <see cref="Geometry.PointZM"/>, <see cref="Geometry.Polyline"/>, <see cref="Geometry.PolylineZ"/> <see cref="Geometry.PolylineZM"/>, or an <see cref="IEnumerable{T}"/> fill with one of those if successful; otherwise <see langword="null"/>.</returns>
    public static Geometry.IGeometry ReadGeometry(ReadOnlySpan<byte> source) => ReadGeometry(ref source, ReadGeometry);

    /// <summary>
    /// Writes a <see cref="Geometry.Point"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointBigEndian(Span<byte> destination, Geometry.Point value) => WritePointBigEndian(destination, value, includeMetadata: true);

    /// <summary>
    /// Writes <see cref="Geometry.Point"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointBigEndian(Span<byte> destination, IEnumerable<Geometry.Point> values) => WritePointBigEndian(destination, values, includeMetadata: true);

    /// <summary>
    /// Writes a <see cref="Geometry.Point"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointLittleEndian(Span<byte> destination, Geometry.Point value) => WritePointLittleEndian(destination, value, includeMetadata: true);

    /// <summary>
    /// Writes <see cref="Geometry.Point"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointLittleEndian(Span<byte> destination, IEnumerable<Geometry.Point> values) => WritePointLittleEndian(destination, values, includeMetadata: true);

    /// <summary>
    /// Writes a <see cref="Geometry.PointZ"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZBigEndian(Span<byte> destination, Geometry.PointZ value) => WritePointZBigEndian(destination, value, includeMetadata: true);

    /// <summary>
    /// Writes <see cref="Geometry.PointZ"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZBigEndian(Span<byte> destination, IEnumerable<Geometry.PointZ> values) => WritePointZBigEndian(destination, values, includeMetadata: true);

    /// <summary>
    /// Writes a <see cref="Geometry.PointZ"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZLittleEndian(Span<byte> destination, Geometry.PointZ value) => WritePointZLittleEndian(destination, value, includeMetadata: true);

    /// <summary>
    /// Writes <see cref="Geometry.PointZ"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZLittleEndian(Span<byte> destination, IEnumerable<Geometry.PointZ> values) => WritePointZLittleEndian(destination, values, includeMetadata: true);

    /// <summary>
    /// Writes a <see cref="Geometry.PointM"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointMBigEndian(Span<byte> destination, Geometry.PointM value) => WritePointMBigEndian(destination, value, includeMetadata: true);

    /// <summary>
    /// Writes <see cref="Geometry.PointM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointMBigEndian(Span<byte> destination, IEnumerable<Geometry.PointM> values) => WritePointMBigEndian(destination, values, includeMetadata: true);

    /// <summary>
    /// Writes a <see cref="Geometry.PointM"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointMLittleEndian(Span<byte> destination, Geometry.PointM value) => WritePointMLittleEndian(destination, value, includeMetadata: true);

    /// <summary>
    /// Writes <see cref="Geometry.PointM"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointMLittleEndian(Span<byte> destination, IEnumerable<Geometry.PointM> values) => WritePointMLittleEndian(destination, values, includeMetadata: true);

    /// <summary>
    /// Writes a <see cref="Geometry.PointZM"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZMBigEndian(Span<byte> destination, Geometry.PointZM value) => WritePointZMBigEndian(destination, value, includeMetadata: true);

    /// <summary>
    /// Writes <see cref="Geometry.PointZM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZMBigEndian(Span<byte> destination, IEnumerable<Geometry.PointZM> values) => WritePointZMBigEndian(destination, values, includeMetadata: true);

    /// <summary>
    /// Writes a <see cref="Geometry.PointZM"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZMLittleEndian(Span<byte> destination, Geometry.PointZM value) => WritePointZMLittleEndian(destination, value, includeMetadata: true);

    /// <summary>
    /// Writes <see cref="Geometry.PointZM"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePointZMLittleEndian(Span<byte> destination, IEnumerable<Geometry.PointZM> values) => WritePointZMLittleEndian(destination, values, includeMetadata: true);

    /// <summary>
    /// Writes a <see cref="Geometry.Polyline"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Polyline"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringBigEndian(Span<byte> destination, Geometry.Polyline<Geometry.Point> value) => WriteLineStringBigEndian(destination, value, includeMetadata: true);

    /// <summary>
    /// Writes <see cref="Geometry.Polyline"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Polyline"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringBigEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.Point>> values) => WriteLineStringBigEndian(destination, values, includeMetadata: true);

    /// <summary>
    /// Writes a <see cref="Geometry.Polyline"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Polyline"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringLittleEndian(Span<byte> destination, Geometry.Polyline<Geometry.Point> value) => WriteLineStringLittleEndian(destination, value, includeMetadata: true);

    /// <summary>
    /// Writes <see cref="Geometry.Point"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringLittleEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.Point>> values) => WriteLineStringLittleEndian(destination, values, includeMetadata: true);

    /// <summary>
    /// Writes a <see cref="Geometry.PolylineZ"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZBigEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointZ> value) => WriteLineStringZBigEndian(destination, value, includeMetadata: true);

    /// <summary>
    /// Writes <see cref="Geometry.PolylineZ"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZBigEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointZ>> values) => WriteLineStringZBigEndian(destination, values, includeMetadata: true);

    /// <summary>
    /// Writes a <see cref="Geometry.PolylineZ"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZLittleEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointZ> value) => WriteLineStringZLittleEndian(destination, value, includeMetadata: true);

    /// <summary>
    /// Writes <see cref="Geometry.PointZ"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZLittleEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointZ>> values) => WriteLineStringZLittleEndian(destination, values, includeMetadata: true);

    /// <summary>
    /// Writes a <see cref="Geometry.PolylineM"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringMBigEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointM> value) => WriteLineStringMBigEndian(destination, value, includeMetadata: true);

    /// <summary>
    /// Writes <see cref="Geometry.PolylineM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringMBigEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointM>> values) => WriteLineStringMBigEndian(destination, values, includeMetadata: true);

    /// <summary>
    /// Writes a <see cref="Geometry.PolylineM"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringMLittleEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointM> value) => WriteLineStringMLittleEndian(destination, value, includeMetadata: true);

    /// <summary>
    /// Writes <see cref="Geometry.PointM"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringMLittleEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointM>> values) => WriteLineStringMLittleEndian(destination, values, includeMetadata: true);

    /// <summary>
    /// Writes a <see cref="Geometry.PolylineZM"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZMBigEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointZM> value) => WriteLineStringZMBigEndian(destination, value, includeMetadata: true);

    /// <summary>
    /// Writes <see cref="Geometry.PolylineZM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZMBigEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointZM>> values) => WriteLineStringZMBigEndian(destination, values, includeMetadata: true);

    /// <summary>
    /// Writes a <see cref="Geometry.PolylineZM"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZMLittleEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointZM> value) => WriteLineStringZMLittleEndian(destination, value, includeMetadata: true);

    /// <summary>
    /// Writes <see cref="Geometry.PointZM"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WriteLineStringZMLittleEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointZM>> values) => WriteLineStringZMLittleEndian(destination, values, includeMetadata: true);

    /// <summary>
    /// Writes a <see cref="Geometry.Polygon"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Polygon"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonBigEndian(Span<byte> destination, Geometry.Polygon<Geometry.Point> value) => WritePolygonBigEndian(destination, value, includeMetadata: true);

    /// <summary>
    /// Writes <see cref="Geometry.Polygon"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Polygon"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonBigEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.Point>> values) => WritePolygonBigEndian(destination, values, includeMetadata: true);

    /// <summary>
    /// Writes a <see cref="Geometry.Polygon"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Polygon"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonLittleEndian(Span<byte> destination, Geometry.Polygon<Geometry.Point> value) => WritePolygonLittleEndian(destination, value, includeMetadata: true);

    /// <summary>
    /// Writes <see cref="Geometry.Point"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonLittleEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.Point>> values) => WritePolygonLittleEndian(destination, values, includeMetadata: true);

    /// <summary>
    /// Writes a <see cref="Geometry.PolygonZ"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZBigEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointZ> value) => WritePolygonZBigEndian(destination, value, includeMetadata: true);

    /// <summary>
    /// Writes <see cref="Geometry.PolygonZ"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZBigEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointZ>> values) => WritePolygonZBigEndian(destination, values, includeMetadata: true);

    /// <summary>
    /// Writes a <see cref="Geometry.PolygonZ"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZLittleEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointZ> value) => WritePolygonZLittleEndian(destination, value, includeMetadata: true);

    /// <summary>
    /// Writes <see cref="Geometry.PointZ"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZLittleEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointZ>> values) => WritePolygonZLittleEndian(destination, values, includeMetadata: true);

    /// <summary>
    /// Writes a <see cref="Geometry.PolygonM"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonMBigEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointM> value) => WritePolygonMBigEndian(destination, value, includeMetadata: true);

    /// <summary>
    /// Writes <see cref="Geometry.PolygonM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonMBigEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointM>> values) => WritePolygonMBigEndian(destination, values, includeMetadata: true);

    /// <summary>
    /// Writes a <see cref="Geometry.PolygonM"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonMLittleEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointM> value) => WritePolygonMLittleEndian(destination, value, includeMetadata: true);

    /// <summary>
    /// Writes <see cref="Geometry.PointM"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonMLittleEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointM>> values) => WritePolygonMLittleEndian(destination, values, includeMetadata: true);

    /// <summary>
    /// Writes a <see cref="Geometry.PolygonZM"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZMBigEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointZM> value) => WritePolygonZMBigEndian(destination, value, includeMetadata: true);

    /// <summary>
    /// Writes <see cref="Geometry.PolygonZM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZMBigEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointZM>> values) => WritePolygonZMBigEndian(destination, values, includeMetadata: true);

    /// <summary>
    /// Writes a <see cref="Geometry.PolygonZM"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZMLittleEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointZM> value) => WritePolygonZMLittleEndian(destination, value, includeMetadata: true);

    /// <summary>
    /// Writes <see cref="Geometry.PointZM"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    public static int WritePolygonZMLittleEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointZM>> values) => WritePolygonZMLittleEndian(destination, values, includeMetadata: true);

    /// <summary>
    /// Gets the maximum size required to write the <see cref="Geometry.Point"/>.
    /// </summary>
    /// <param name="value">The value to get the maximum size for.</param>
    /// <returns>The number of bytes that <paramref name="value"/> will take up.</returns>
    public static int GetMaxSize(Geometry.Point value) => GetMaxSize(value, includeMetadata: true, GetSizeAndType);

    /// <summary>
    /// Gets the maximum size required to write the <see cref="Geometry.PointZ"/>.
    /// </summary>
    /// <param name="value">The value to get the maximum size for.</param>
    /// <returns>The number of bytes that <paramref name="value"/> will take up.</returns>
    public static int GetMaxSize(Geometry.PointZ value) => GetMaxSize(value, includeMetadata: true, GetSizeAndType);

    /// <summary>
    /// Gets the maximum size required to write the <see cref="Geometry.PointM"/>.
    /// </summary>
    /// <param name="value">The value to get the maximum size for.</param>
    /// <returns>The number of bytes that <paramref name="value"/> will take up.</returns>
    public static int GetMaxSize(Geometry.PointM value) => GetMaxSize(value, includeMetadata: true, GetSizeAndType);

    /// <summary>
    /// Gets the maximum size required to write the <see cref="Geometry.PointZM"/>.
    /// </summary>
    /// <param name="value">The value to get the maximum size for.</param>
    /// <returns>The number of bytes that <paramref name="value"/> will take up.</returns>
    public static int GetMaxSize(Geometry.PointZM value) => GetMaxSize(value, includeMetadata: true, GetSizeAndType);

    /// <summary>
    /// Gets the maximum size required to write the <see cref="Geometry.Point"/>.
    /// </summary>
    /// <param name="value">The value to get the maximum size for.</param>
    /// <returns>The number of bytes that <paramref name="value"/> will take up.</returns>
    public static int GetMaxSize(ICollection<Geometry.Point> value) => GetMaxSize(value, includeMetadata: true, GetSizeAndType);

    /// <summary>
    /// Gets the maximum size required to write the <see cref="Geometry.PointZ"/>.
    /// </summary>
    /// <param name="value">The value to get the maximum size for.</param>
    /// <returns>The number of bytes that <paramref name="value"/> will take up.</returns>
    public static int GetMaxSize(ICollection<Geometry.PointZ> value) => GetMaxSize(value, includeMetadata: true, GetSizeAndType);

    /// <summary>
    /// Gets the maximum size required to write the <see cref="Geometry.PointM"/>.
    /// </summary>
    /// <param name="value">The value to get the maximum size for.</param>
    /// <returns>The number of bytes that <paramref name="value"/> will take up.</returns>
    public static int GetMaxSize(ICollection<Geometry.PointM> value) => GetMaxSize(value, includeMetadata: true, GetSizeAndType);

    /// <summary>
    /// Gets the maximum size required to write the <see cref="Geometry.PointZM"/>.
    /// </summary>
    /// <param name="value">The value to get the maximum size for.</param>
    /// <returns>The number of bytes that <paramref name="value"/> will take up.</returns>
    public static int GetMaxSize(ICollection<Geometry.PointZM> value) => GetMaxSize(value, includeMetadata: true, GetSizeAndType);

    /// <summary>
    /// Gets the maximum size required to write the <see cref="Geometry.Polyline"/>.
    /// </summary>
    /// <param name="value">The value to get the maximum size for.</param>
    /// <returns>The number of bytes that <paramref name="value"/> will take up.</returns>
    public static int GetMaxSize(Geometry.Polyline<Geometry.Point> value) => GetMaxSize(value, GetSizeAndType);

    /// <summary>
    /// Gets the maximum size required to write the <see cref="Geometry.PolylineZ"/>.
    /// </summary>
    /// <param name="value">The value to get the maximum size for.</param>
    /// <returns>The number of bytes that <paramref name="value"/> will take up.</returns>
    public static int GetMaxSize(Geometry.Polyline<Geometry.PointZ> value) => GetMaxSize(value, GetSizeAndType);

    /// <summary>
    /// Gets the maximum size required to write the <see cref="Geometry.PolylineM"/>.
    /// </summary>
    /// <param name="value">The value to get the maximum size for.</param>
    /// <returns>The number of bytes that <paramref name="value"/> will take up.</returns>
    public static int GetMaxSize(Geometry.Polyline<Geometry.PointM> value) => GetMaxSize(value, GetSizeAndType);

    /// <summary>
    /// Gets the maximum size required to write the <see cref="Geometry.PolylineZM"/>.
    /// </summary>
    /// <param name="value">The value to get the maximum size for.</param>
    /// <returns>The number of bytes that <paramref name="value"/> will take up.</returns>
    public static int GetMaxSize(Geometry.Polyline<Geometry.PointZM> value) => GetMaxSize(value, GetSizeAndType);

    /// <summary>
    /// Gets the maximum size required to write the <see cref="Geometry.Polyline"/>.
    /// </summary>
    /// <param name="value">The value to get the maximum size for.</param>
    /// <returns>The number of bytes that <paramref name="value"/> will take up.</returns>
    public static int GetMaxSize(ICollection<Geometry.Polyline<Geometry.Point>> value) => GetMaxSize(value, GetSizeAndType);

    /// <summary>
    /// Gets the maximum size required to write the <see cref="Geometry.PolylineZ"/>.
    /// </summary>
    /// <param name="value">The value to get the maximum size for.</param>
    /// <returns>The number of bytes that <paramref name="value"/> will take up.</returns>
    public static int GetMaxSize(ICollection<Geometry.Polyline<Geometry.PointZ>> value) => GetMaxSize(value, GetSizeAndType);

    /// <summary>
    /// Gets the maximum size required to write the <see cref="Geometry.PolylineM"/>.
    /// </summary>
    /// <param name="value">The value to get the maximum size for.</param>
    /// <returns>The number of bytes that <paramref name="value"/> will take up.</returns>
    public static int GetMaxSize(ICollection<Geometry.Polyline<Geometry.PointM>> value) => GetMaxSize(value, GetSizeAndType);

    /// <summary>
    /// Gets the maximum size required to write the <see cref="Geometry.PolylineZM"/>.
    /// </summary>
    /// <param name="value">The value to get the maximum size for.</param>
    /// <returns>The number of bytes that <paramref name="value"/> will take up.</returns>
    public static int GetMaxSize(ICollection<Geometry.Polyline<Geometry.PointZM>> value) => GetMaxSize(value, GetSizeAndType);

    /// <summary>
    /// Gets the maximum size required to write the <see cref="Geometry.Polygon"/>.
    /// </summary>
    /// <param name="value">The value to get the maximum size for.</param>
    /// <returns>The number of bytes that <paramref name="value"/> will take up.</returns>
    public static int GetMaxSize(Geometry.Polygon<Geometry.Point> value) => GetMaxSize(value, GetSizeAndType);

    /// <summary>
    /// Gets the maximum size required to write the <see cref="Geometry.PolygonZ"/>.
    /// </summary>
    /// <param name="value">The value to get the maximum size for.</param>
    /// <returns>The number of bytes that <paramref name="value"/> will take up.</returns>
    public static int GetMaxSize(Geometry.Polygon<Geometry.PointZ> value) => GetMaxSize(value, GetSizeAndType);

    /// <summary>
    /// Gets the maximum size required to write the <see cref="Geometry.PolygonM"/>.
    /// </summary>
    /// <param name="value">The value to get the maximum size for.</param>
    /// <returns>The number of bytes that <paramref name="value"/> will take up.</returns>
    public static int GetMaxSize(Geometry.Polygon<Geometry.PointM> value) => GetMaxSize(value, GetSizeAndType);

    /// <summary>
    /// Gets the maximum size required to write the <see cref="Geometry.PolygonZM"/>.
    /// </summary>
    /// <param name="value">The value to get the maximum size for.</param>
    /// <returns>The number of bytes that <paramref name="value"/> will take up.</returns>
    public static int GetMaxSize(Geometry.Polygon<Geometry.PointZM> value) => GetMaxSize(value, GetSizeAndType);

    /// <summary>
    /// Gets the maximum size required to write the <see cref="Geometry.Polygon"/>.
    /// </summary>
    /// <param name="value">The value to get the maximum size for.</param>
    /// <returns>The number of bytes that <paramref name="value"/> will take up.</returns>
    public static int GetMaxSize(ICollection<Geometry.Polygon<Geometry.Point>> value) => GetMaxSize(value, GetSizeAndType);

    /// <summary>
    /// Gets the maximum size required to write the <see cref="Geometry.PolygonZ"/>.
    /// </summary>
    /// <param name="value">The value to get the maximum size for.</param>
    /// <returns>The number of bytes that <paramref name="value"/> will take up.</returns>
    public static int GetMaxSize(ICollection<Geometry.Polygon<Geometry.PointZ>> value) => GetMaxSize(value, GetSizeAndType);

    /// <summary>
    /// Gets the maximum size required to write the <see cref="Geometry.PolygonM"/>.
    /// </summary>
    /// <param name="value">The value to get the maximum size for.</param>
    /// <returns>The number of bytes that <paramref name="value"/> will take up.</returns>
    public static int GetMaxSize(ICollection<Geometry.Polygon<Geometry.PointM>> value) => GetMaxSize(value, GetSizeAndType);

    /// <summary>
    /// Gets the maximum size required to write the <see cref="Geometry.PolygonZM"/>.
    /// </summary>
    /// <param name="value">The value to get the maximum size for.</param>
    /// <returns>The number of bytes that <paramref name="value"/> will take up.</returns>
    public static int GetMaxSize(ICollection<Geometry.Polygon<Geometry.PointZM>> value) => GetMaxSize(value, GetSizeAndType);

    /// <summary>
    /// Reads the byte order and geometry type.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>The byte order and geometry type.</returns>
    internal static (WkbByteOrder ByteOrder, WkbGeometryType GeometryType) ReadByteOrderAndGeometryType(ref ReadOnlySpan<byte> source)
    {
        var byteOrder = (WkbByteOrder)source[0];
        source = source[1..];
        var geometryType = (WkbGeometryType)(byteOrder is WkbByteOrder.Ndr
            ? ReadUInt32LittleEndian(source)
            : ReadUInt32BigEndian(source));
        source = source[4..];
        return (byteOrder, geometryType);
    }

    /// <inheritdoc cref="ReadPoint(ReadOnlySpan{byte})"/>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    internal static Geometry.Point ReadPoint(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder, WkbGeometryType geometryType) =>
        geometryType switch
        {
            WkbGeometryType.Point => ReadPoint(ref source, byteOrder),
            var v => throw new Geometry.InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.Point)}"),
        };

    /// <inheritdoc cref="ReadPointZ(ReadOnlySpan{byte})"/>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    internal static Geometry.PointZ ReadPointZ(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder, WkbGeometryType geometryType) =>
        geometryType switch
        {
            WkbGeometryType.PointZ => ReadPointZ(ref source, byteOrder),
            var v => throw new Geometry.InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.PointZ)}"),
        };

    /// <inheritdoc cref="ReadPointM(ReadOnlySpan{byte})"/>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    internal static Geometry.PointM ReadPointM(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder, WkbGeometryType geometryType) =>
        geometryType switch
        {
            WkbGeometryType.PointM => ReadPointM(ref source, byteOrder),
            var v => throw new Geometry.InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.PointZ)}"),
        };

    /// <inheritdoc cref="ReadPointZM(ReadOnlySpan{byte})"/>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    internal static Geometry.PointZM ReadPointZM(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.PointZM => ReadPointZM(ref source, byteOrder),
        var v => throw new Geometry.InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.PointZM)}"),
    };

    /// <inheritdoc cref="ReadMultiPoint(ReadOnlySpan{byte})"/>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    internal static Geometry.IMultiGeometry<Geometry.Point> ReadMultiPoint(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.MultiPoint => ReadMulti(source, byteOrder, ReadPoint),
        var v => throw new Geometry.InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.MultiPoint)}"),
    };

    /// <inheritdoc cref="ReadMultiPointZ(ReadOnlySpan{byte})"/>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    internal static Geometry.IMultiGeometry<Geometry.PointZ> ReadMultiPointZ(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.MultiPointZ => ReadMulti(source, byteOrder, ReadPointZ),
        var v => throw new Geometry.InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.MultiPointZ)}"),
    };

    /// <inheritdoc cref="ReadMultiPointM(ReadOnlySpan{byte})"/>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    internal static Geometry.IMultiGeometry<Geometry.PointM> ReadMultiPointM(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.MultiPointM => ReadMulti(source, byteOrder, ReadPointM),
        var v => throw new Geometry.InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.MultiPointZ)}"),
    };

    /// <inheritdoc cref="ReadMultiPointZM(ReadOnlySpan{byte})"/>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    internal static Geometry.IMultiGeometry<Geometry.PointZM> ReadMultiPointZM(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.MultiPointZM => ReadMulti(source, byteOrder, ReadPointZM),
        var v => throw new Geometry.InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.MultiPointZM)}"),
    };

    /// <inheritdoc cref="ReadLineString(ReadOnlySpan{byte})"/>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    internal static Geometry.Polyline ReadLineString(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.LineString => [.. ReadPoints(ref source, byteOrder, ReadPoint)],
        var v => throw new Geometry.InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.LineString)}"),
    };

    /// <inheritdoc cref="ReadLineStringZ(ReadOnlySpan{byte})"/>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    internal static Geometry.PolylineZ ReadLineStringZ(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.LineStringZ => [.. ReadPoints(ref source, byteOrder, ReadPointZ)],
        var v => throw new Geometry.InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.LineString)}"),
    };

    /// <inheritdoc cref="ReadLineStringM(ReadOnlySpan{byte})"/>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    internal static Geometry.PolylineM ReadLineStringM(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.LineStringM => [.. ReadPoints(ref source, byteOrder, ReadPointM)],
        var v => throw new Geometry.InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.LineString)}"),
    };

    /// <inheritdoc cref="ReadLineStringZM(ReadOnlySpan{byte})"/>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    internal static Geometry.PolylineZM ReadLineStringZM(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.LineStringZM => [.. ReadPoints(ref source, byteOrder, ReadPointZM)],
        var v => throw new Geometry.InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.LineString)}"),
    };

    /// <inheritdoc cref="ReadMultiLineString(ReadOnlySpan{byte})"/>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    internal static Geometry.IMultiGeometry<Geometry.Polyline> ReadMultiLineString(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.MultiLineString => ReadMulti(source, byteOrder, ReadLineString),
        var v => throw new Geometry.InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.MultiLineString)}"),
    };

    /// <inheritdoc cref="ReadMultiLineStringZ(ReadOnlySpan{byte})"/>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    internal static Geometry.IMultiGeometry<Geometry.PolylineZ> ReadMultiLineStringZ(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.MultiLineStringZ or WkbGeometryType.MultiLineStringM => ReadMulti(source, byteOrder, ReadLineStringZ),
        var v => throw new Geometry.InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.MultiLineString)}"),
    };

    /// <inheritdoc cref="ReadMultiLineStringM(ReadOnlySpan{byte})"/>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    internal static Geometry.IMultiGeometry<Geometry.PolylineM> ReadMultiLineStringM(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.MultiLineStringM => ReadMulti(source, byteOrder, ReadLineStringM),
        var v => throw new Geometry.InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.MultiLineString)}"),
    };

    /// <inheritdoc cref="ReadMultiLineStringZM(ReadOnlySpan{byte})"/>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    internal static Geometry.IMultiGeometry<Geometry.PolylineZM> ReadMultiLineStringZM(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.MultiLineStringZM => ReadMulti(source, byteOrder, ReadLineStringZM),
        var v => throw new Geometry.InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.MultiLineString)}"),
    };

    /// <inheritdoc cref="ReadPolygon(ReadOnlySpan{byte})"/>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    internal static Geometry.Polygon ReadPolygon(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.Polygon => [.. ReadLinearRings(ref source, byteOrder, ReadPoint)],
        var v => throw new Geometry.InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.Polygon)}"),
    };

    /// <inheritdoc cref="ReadPolygonZ(ReadOnlySpan{byte})"/>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    internal static Geometry.PolygonZ ReadPolygonZ(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.PolygonZ => [.. ReadLinearRings(ref source, byteOrder, ReadPointZ)],
        var v => throw new Geometry.InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.Polygon)}"),
    };

    /// <inheritdoc cref="ReadPolygonM(ReadOnlySpan{byte})"/>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    internal static Geometry.PolygonM ReadPolygonM(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.PolygonM => [.. ReadLinearRings(ref source, byteOrder, ReadPointM)],
        var v => throw new Geometry.InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.Polygon)}"),
    };

    /// <inheritdoc cref="ReadPolygonZM(ReadOnlySpan{byte})"/>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    internal static Geometry.PolygonZM ReadPolygonZM(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.PolygonZM => [.. ReadLinearRings(ref source, byteOrder, ReadPointZM)],
        var v => throw new Geometry.InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.Polygon)}"),
    };

    /// <inheritdoc cref="ReadMultiPolygon(ReadOnlySpan{byte})"/>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    internal static Geometry.IMultiGeometry<Geometry.Polygon> ReadMultiPolygon(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.MultiPolygon => ReadMulti(source, byteOrder, ReadPolygon),
        var v => throw new Geometry.InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.MultiPolygon)}"),
    };

    /// <inheritdoc cref="ReadMultiPolygonZ(ReadOnlySpan{byte})"/>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    internal static Geometry.IMultiGeometry<Geometry.PolygonZ> ReadMultiPolygonZ(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.MultiPolygonZ => ReadMulti(source, byteOrder, ReadPolygonZ),
        var v => throw new Geometry.InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.MultiPolygon)}"),
    };

    /// <inheritdoc cref="ReadMultiPolygonM(ReadOnlySpan{byte})"/>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    internal static Geometry.IMultiGeometry<Geometry.PolygonM> ReadMultiPolygonM(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.MultiPolygonM => ReadMulti(source, byteOrder, ReadPolygonM),
        var v => throw new Geometry.InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.MultiPolygon)}"),
    };

    /// <inheritdoc cref="ReadMultiPolygonZM(ReadOnlySpan{byte})"/>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    internal static Geometry.IMultiGeometry<Geometry.PolygonZM> ReadMultiPolygonZM(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.MultiPolygonZM => ReadMulti(source, byteOrder, ReadPolygonZM),
        var v => throw new Geometry.InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.MultiPolygon)}"),
    };

    /// <inheritdoc cref="ReadGeometry(ReadOnlySpan{byte})"/>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    internal static Geometry.IGeometry ReadGeometry(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.Point => ReadPoint(ref source, byteOrder),
        WkbGeometryType.PointZ => ReadPointZ(ref source, byteOrder),
        WkbGeometryType.PointM => ReadPointM(ref source, byteOrder),
        WkbGeometryType.PointZM => ReadPointZM(ref source, byteOrder),
        WkbGeometryType.MultiPoint => ReadMulti(source, byteOrder, ReadPoint),
        WkbGeometryType.MultiPointZ => ReadMulti(source, byteOrder, ReadPointZ),
        WkbGeometryType.MultiPointM => ReadMulti(source, byteOrder, ReadPointM),
        WkbGeometryType.MultiPointZM => ReadMulti(source, byteOrder, ReadPointZM),
        WkbGeometryType.LineString => Geometry.Polyline.FromPoints(ReadPoints(ref source, byteOrder, ReadPoint)),
        WkbGeometryType.LineStringZ => Geometry.PolylineZ.FromPoints(ReadPoints(ref source, byteOrder, ReadPointZ)),
        WkbGeometryType.LineStringM => Geometry.PolylineM.FromPoints(ReadPoints(ref source, byteOrder, ReadPointM)),
        WkbGeometryType.LineStringZM => Geometry.PolylineZM.FromPoints(ReadPoints(ref source, byteOrder, ReadPointZM)),
        WkbGeometryType.MultiLineString => ReadMulti(source, byteOrder, ReadLineString),
        WkbGeometryType.MultiLineStringZ => ReadMulti(source, byteOrder, ReadLineStringZ),
        WkbGeometryType.MultiLineStringM => ReadMulti(source, byteOrder, ReadLineStringM),
        WkbGeometryType.MultiLineStringZM => ReadMulti(source, byteOrder, ReadLineStringZM),
        WkbGeometryType.Polygon => new Geometry.Polygon(ReadLinearRings(ref source, byteOrder, ReadPoint)),
        WkbGeometryType.PolygonZ => new Geometry.PolygonZ(ReadLinearRings(ref source, byteOrder, ReadPointZ)),
        WkbGeometryType.PolygonM => new Geometry.PolygonM(ReadLinearRings(ref source, byteOrder, ReadPointM)),
        WkbGeometryType.PolygonZM => new Geometry.PolygonZM(ReadLinearRings(ref source, byteOrder, ReadPointZM)),
        WkbGeometryType.MultiPolygon => ReadMulti(source, byteOrder, ReadPolygon),
        WkbGeometryType.MultiPolygonZ => ReadMulti(source, byteOrder, ReadPolygonZ),
        WkbGeometryType.MultiPolygonM => ReadMulti(source, byteOrder, ReadPolygonM),
        WkbGeometryType.MultiPolygonZM => ReadMulti(source, byteOrder, ReadPolygonZM),
        _ => throw new Geometry.InvalidGeometryTypeException(),
    };

    /// <summary>
    /// Writes a <see cref="Geometry.Point"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePointBigEndian(Span<byte> destination, Geometry.Point value, bool includeMetadata) =>
        Write(value, destination, littleEndian: false, includeMetadata, getSizeAndType: GetSizeAndType, getXY: GetXY, getZ: Throw, getM: Throw);

    /// <summary>
    /// Writes <see cref="Geometry.Point"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePointBigEndian(Span<byte> destination, IEnumerable<Geometry.Point> values, bool includeMetadata) =>
        Write([.. values], destination, littleEndian: false, WkbGeometryType.MultiPoint, includeMetadata, true, GetSizeAndType, GetXY, Throw, Throw);

    /// <summary>
    /// Writes a <see cref="Geometry.Point"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePointLittleEndian(Span<byte> destination, Geometry.Point value, bool includeMetadata) =>
        Write(value, destination, littleEndian: true, includeMetadata, getSizeAndType: GetSizeAndType, getXY: GetXY, getZ: Throw, getM: Throw);

    /// <summary>
    /// Writes <see cref="Geometry.Point"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePointLittleEndian(Span<byte> destination, IEnumerable<Geometry.Point> values, bool includeMetadata) =>
        Write([.. values], destination, littleEndian: true, WkbGeometryType.MultiPoint, includeMetadata, true, GetSizeAndType, GetXY, Throw, Throw);

    /// <summary>
    /// Writes a <see cref="Geometry.PointZ"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePointZBigEndian(Span<byte> destination, Geometry.PointZ value, bool includeMetadata) =>
        Write(value, destination, littleEndian: false, includeMetadata, getSizeAndType: GetSizeAndType, getXY: GetXY, getZ: GetZ, getM: Throw);

    /// <summary>
    /// Writes <see cref="Geometry.PointZ"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePointZBigEndian(Span<byte> destination, IEnumerable<Geometry.PointZ> values, bool includeMetadata) =>
        Write([.. values], destination, littleEndian: false, WkbGeometryType.MultiPoint, includeMetadata, true, GetSizeAndType, GetXY, GetZ, Throw);

    /// <summary>
    /// Writes a <see cref="Geometry.PointZ"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePointZLittleEndian(Span<byte> destination, Geometry.PointZ value, bool includeMetadata) =>
        Write(value, destination, littleEndian: true, includeMetadata, getSizeAndType: GetSizeAndType, getXY: GetXY, getZ: GetZ, getM: Throw);

    /// <summary>
    /// Writes <see cref="Geometry.PointZ"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePointZLittleEndian(Span<byte> destination, IEnumerable<Geometry.PointZ> values, bool includeMetadata) =>
        Write([.. values], destination, littleEndian: true, WkbGeometryType.MultiPoint, includeMetadata, true, GetSizeAndType, GetXY, GetZ, Throw);

    /// <summary>
    /// Writes a <see cref="Geometry.PointM"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePointMBigEndian(Span<byte> destination, Geometry.PointM value, bool includeMetadata) =>
        Write(value, destination, littleEndian: false, includeMetadata, getSizeAndType: GetSizeAndType, getXY: GetXY, getZ: Throw, getM: GetM);

    /// <summary>
    /// Writes <see cref="Geometry.PointM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePointMBigEndian(Span<byte> destination, IEnumerable<Geometry.PointM> values, bool includeMetadata) =>
        Write([.. values], destination, littleEndian: false, WkbGeometryType.MultiPoint, includeMetadata, true, GetSizeAndType, GetXY, Throw, GetM);

    /// <summary>
    /// Writes a <see cref="Geometry.PointM"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePointMLittleEndian(Span<byte> destination, Geometry.PointM value, bool includeMetadata) =>
        Write(value, destination, littleEndian: true, includeMetadata, getSizeAndType: GetSizeAndType, getXY: GetXY, getZ: Throw, getM: GetM);

    /// <summary>
    /// Writes <see cref="Geometry.PointM"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePointMLittleEndian(Span<byte> destination, IEnumerable<Geometry.PointM> values, bool includeMetadata) =>
        Write([.. values], destination, littleEndian: true, WkbGeometryType.MultiPoint, includeMetadata, true, GetSizeAndType, GetXY, Throw, GetM);

    /// <summary>
    /// Writes a <see cref="Geometry.PointZM"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePointZMBigEndian(Span<byte> destination, Geometry.PointZM value, bool includeMetadata) =>
        Write(value, destination, littleEndian: false, includeMetadata, getSizeAndType: GetSizeAndType, getXY: GetXY, getZ: Throw, getM: GetM);

    /// <summary>
    /// Writes <see cref="Geometry.PointZM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePointZMBigEndian(Span<byte> destination, IEnumerable<Geometry.PointZM> values, bool includeMetadata) =>
        Write([.. values], destination, littleEndian: false, WkbGeometryType.MultiPoint, includeMetadata, true, GetSizeAndType, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes a <see cref="Geometry.PointZM"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/>.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePointZMLittleEndian(Span<byte> destination, Geometry.PointZM value, bool includeMetadata) =>
        Write(value, destination, littleEndian: true, includeMetadata, getSizeAndType: GetSizeAndType, getXY: GetXY, getZ: GetZ, getM: GetM);

    /// <summary>
    /// Writes <see cref="Geometry.PointZM"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePointZMLittleEndian(Span<byte> destination, IEnumerable<Geometry.PointZM> values, bool includeMetadata) =>
        Write([.. values], destination, littleEndian: true, WkbGeometryType.MultiPoint, includeMetadata, true, GetSizeAndType, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes a <see cref="Geometry.Polyline"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Polyline"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteLineStringBigEndian(Span<byte> destination, Geometry.Polyline<Geometry.Point> value, bool includeMetadata) =>
        Write([.. value], destination, littleEndian: false, WkbGeometryType.LineString, includeMetadata, false, GetSizeAndType, GetXY, Throw, Throw);

    /// <summary>
    /// Writes <see cref="Geometry.Polyline"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Polyline"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteLineStringBigEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.Point>> values, bool includeMetadata) =>
        Write(values, destination, littleEndian: false, includeMetadata, GetSizeAndType, GetXY, Throw, Throw);

    /// <summary>
    /// Writes a <see cref="Geometry.Polyline"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Polyline"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteLineStringLittleEndian(Span<byte> destination, Geometry.Polyline<Geometry.Point> value, bool includeMetadata) =>
        Write([.. value], destination, littleEndian: true, WkbGeometryType.LineString, includeMetadata, false, GetSizeAndType, GetXY, Throw, Throw);

    /// <summary>
    /// Writes <see cref="Geometry.Point"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteLineStringLittleEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.Point>> values, bool includeMetadata) =>
        Write(values, destination, littleEndian: true, includeMetadata, GetSizeAndType, GetXY, Throw, Throw);

    /// <summary>
    /// Writes a <see cref="Geometry.PolylineZ"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteLineStringZBigEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointZ> value, bool includeMetadata) =>
        Write([.. value], destination, littleEndian: false, WkbGeometryType.LineString, includeMetadata, false, GetSizeAndType, GetXY, GetZ, Throw);

    /// <summary>
    /// Writes <see cref="Geometry.PolylineZ"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteLineStringZBigEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointZ>> values, bool includeMetadata) =>
        Write(values, destination, littleEndian: false, includeMetadata, GetSizeAndType, GetXY, GetZ, Throw);

    /// <summary>
    /// Writes a <see cref="Geometry.PolylineZ"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteLineStringZLittleEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointZ> value, bool includeMetadata) =>
        Write([.. value], destination, littleEndian: true, WkbGeometryType.LineString, includeMetadata, false, GetSizeAndType, GetXY, GetZ, Throw);

    /// <summary>
    /// Writes <see cref="Geometry.PointZ"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteLineStringZLittleEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointZ>> values, bool includeMetadata) =>
        Write(values, destination, littleEndian: true, includeMetadata, GetSizeAndType, GetXY, GetZ, Throw);

    /// <summary>
    /// Writes a <see cref="Geometry.PolylineM"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteLineStringMBigEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointM> value, bool includeMetadata) =>
        Write([.. value], destination, littleEndian: false, WkbGeometryType.LineString, includeMetadata, false, GetSizeAndType, GetXY, Throw, GetM);

    /// <summary>
    /// Writes <see cref="Geometry.PolylineM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteLineStringMBigEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointM>> values, bool includeMetadata) =>
        Write(values, destination, littleEndian: false, includeMetadata, GetSizeAndType, GetXY, Throw, GetM);

    /// <summary>
    /// Writes a <see cref="Geometry.PolylineM"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteLineStringMLittleEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointM> value, bool includeMetadata) =>
        Write([.. value], destination, littleEndian: true, WkbGeometryType.LineString, includeMetadata, false, GetSizeAndType, GetXY, Throw, GetM);

    /// <summary>
    /// Writes <see cref="Geometry.PointM"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteLineStringMLittleEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointM>> values, bool includeMetadata) =>
        Write(values, destination, littleEndian: true, includeMetadata, GetSizeAndType, GetXY, Throw, GetM);

    /// <summary>
    /// Writes a <see cref="Geometry.PolylineZM"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteLineStringZMBigEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointZM> value, bool includeMetadata) =>
        Write([.. value], destination, littleEndian: false, WkbGeometryType.LineString, includeMetadata, false, GetSizeAndType, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes <see cref="Geometry.PolylineZM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteLineStringZMBigEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointZM>> values, bool includeMetadata) =>
        Write(values, destination, littleEndian: false, includeMetadata, GetSizeAndType, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes a <see cref="Geometry.PolylineZM"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolylineZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteLineStringZMLittleEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointZM> value, bool includeMetadata) =>
        Write([.. value], destination, littleEndian: true, WkbGeometryType.LineString, includeMetadata, false, GetSizeAndType, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes <see cref="Geometry.PointZM"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteLineStringZMLittleEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointZM>> values, bool includeMetadata) =>
        Write(values, destination, littleEndian: true, includeMetadata, GetSizeAndType, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes a <see cref="Geometry.Polygon"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Polygon"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePolygonBigEndian(Span<byte> destination, Geometry.Polygon<Geometry.Point> value, bool includeMetadata) => Write(value, destination, littleEndian: false, includeMetadata, GetSizeAndType, GetXY, Throw, Throw);

    /// <summary>
    /// Writes <see cref="Geometry.Polygon"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Polygon"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePolygonBigEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.Point>> values, bool includeMetadata) =>
        Write([.. values], destination, littleEndian: false, includeMetadata, GetSizeAndType, GetXY, Throw, Throw);

    /// <summary>
    /// Writes a <see cref="Geometry.Polygon"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Polygon"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePolygonLittleEndian(Span<byte> destination, Geometry.Polygon<Geometry.Point> value, bool includeMetadata) => Write(value, destination, littleEndian: true, includeMetadata, GetSizeAndType, GetXY, Throw, Throw);

    /// <summary>
    /// Writes <see cref="Geometry.Point"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.Point"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePolygonLittleEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.Point>> values, bool includeMetadata) =>
        Write([.. values], destination, littleEndian: true, includeMetadata, GetSizeAndType, GetXY, Throw, Throw);

    /// <summary>
    /// Writes a <see cref="Geometry.PolygonZ"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePolygonZBigEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointZ> value, bool includeMetadata) => Write(value, destination, littleEndian: false, includeMetadata, GetSizeAndType, GetXY, GetZ, Throw);

    /// <summary>
    /// Writes <see cref="Geometry.PolygonZ"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePolygonZBigEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointZ>> values, bool includeMetadata) =>
        Write([.. values], destination, littleEndian: false, includeMetadata, GetSizeAndType, GetXY, GetZ, Throw);

    /// <summary>
    /// Writes a <see cref="Geometry.PolygonZ"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePolygonZLittleEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointZ> value, bool includeMetadata) => Write(value, destination, littleEndian: true, includeMetadata, GetSizeAndType, GetXY, GetZ, Throw);

    /// <summary>
    /// Writes <see cref="Geometry.PointZ"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZ"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePolygonZLittleEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointZ>> values, bool includeMetadata) =>
        Write([.. values], destination, littleEndian: true, includeMetadata, GetSizeAndType, GetXY, GetZ, Throw);

    /// <summary>
    /// Writes a <see cref="Geometry.PolygonM"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePolygonMBigEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointM> value, bool includeMetadata) => Write(value, destination, littleEndian: false, includeMetadata, GetSizeAndType, GetXY, Throw, GetM);

    /// <summary>
    /// Writes <see cref="Geometry.PolygonM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePolygonMBigEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointM>> values, bool includeMetadata) =>
        Write([.. values], destination, littleEndian: false, includeMetadata, GetSizeAndType, GetXY, Throw, GetM);

    /// <summary>
    /// Writes a <see cref="Geometry.PolygonM"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePolygonMLittleEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointM> value, bool includeMetadata) => Write(value, destination, littleEndian: true, includeMetadata, GetSizeAndType, GetXY, Throw, GetM);

    /// <summary>
    /// Writes <see cref="Geometry.PointM"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePolygonMLittleEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointM>> values, bool includeMetadata) =>
        Write([.. values], destination, littleEndian: true, includeMetadata, GetSizeAndType, GetXY, Throw, GetM);

    /// <summary>
    /// Writes a <see cref="Geometry.PolygonZM"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePolygonZMBigEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointZM> value, bool includeMetadata) => Write(value, destination, littleEndian: false, includeMetadata, GetSizeAndType, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes <see cref="Geometry.PolygonZM"/> instances into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePolygonZMBigEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointZM>> values, bool includeMetadata) =>
        Write([.. values], destination, littleEndian: false, includeMetadata, GetSizeAndType, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes a <see cref="Geometry.PolygonZM"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PolygonZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePolygonZMLittleEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointZM> value, bool includeMetadata) => Write(value, destination, littleEndian: true, includeMetadata, GetSizeAndType, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes <see cref="Geometry.PointZM"/> instances into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="values">The values to write into the span of bytes.</param>
    /// <param name="includeMetadata">Set to <see langword="true"/> to include the metadata.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain the <see cref="Geometry.PointZM"/> instances.</exception>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WritePolygonZMLittleEndian(Span<byte> destination, IEnumerable<Geometry.Polygon<Geometry.PointZM>> values, bool includeMetadata) =>
        Write([.. values], destination, littleEndian: true, includeMetadata, GetSizeAndType, GetXY, GetZ, GetM);

    /// <summary>
    /// Writes the header as big endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="value">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderBigEndian(Span<byte> destination, Geometry.Point value, Func<WkbGeometryType, uint> getType) => WriteHeader(value, destination, littleEndian: false, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as big endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="value">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderBigEndian(Span<byte> destination, Geometry.PointZ value, Func<WkbGeometryType, uint> getType) => WriteHeader(value, destination, littleEndian: false, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as big endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="value">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderBigEndian(Span<byte> destination, Geometry.PointM value, Func<WkbGeometryType, uint> getType) => WriteHeader(value, destination, littleEndian: false, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as big endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="value">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderBigEndian(Span<byte> destination, Geometry.PointZM value, Func<WkbGeometryType, uint> getType) => WriteHeader(value, destination, littleEndian: false, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as big endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="values">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderBigEndian(Span<byte> destination, ICollection<Geometry.Point> values, Func<WkbGeometryType, uint> getType) => WriteHeader(values, destination, littleEndian: false, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as big endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="values">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderBigEndian(Span<byte> destination, ICollection<Geometry.PointZ> values, Func<WkbGeometryType, uint> getType) => WriteHeader(values, destination, littleEndian: false, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as big endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="values">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderBigEndian(Span<byte> destination, ICollection<Geometry.PointM> values, Func<WkbGeometryType, uint> getType) => WriteHeader(values, destination, littleEndian: false, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as big endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="values">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderBigEndian(Span<byte> destination, ICollection<Geometry.PointZM> values, Func<WkbGeometryType, uint> getType) => WriteHeader(values, destination, littleEndian: false, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as big endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="value">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderBigEndian(Span<byte> destination, Geometry.Polyline<Geometry.Point> value, Func<WkbGeometryType, uint> getType) => WriteHeader(value, destination, littleEndian: false, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as big endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="value">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderBigEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointZ> value, Func<WkbGeometryType, uint> getType) => WriteHeader(value, destination, littleEndian: false, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as big endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="value">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderBigEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointM> value, Func<WkbGeometryType, uint> getType) => WriteHeader(value, destination, littleEndian: false, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as big endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="value">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderBigEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointZM> value, Func<WkbGeometryType, uint> getType) => WriteHeader(value, destination, littleEndian: false, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as big endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="values">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderBigEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.Point>> values, Func<WkbGeometryType, uint> getType) => WriteHeader(values, destination, littleEndian: false, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as big endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="values">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderBigEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointZ>> values, Func<WkbGeometryType, uint> getType) => WriteHeader(values, destination, littleEndian: false, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as big endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="values">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderBigEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointM>> values, Func<WkbGeometryType, uint> getType) => WriteHeader(values, destination, littleEndian: false, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as big endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="values">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderBigEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointZM>> values, Func<WkbGeometryType, uint> getType) => WriteHeader(values, destination, littleEndian: false, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as big endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="value">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderBigEndian(Span<byte> destination, Geometry.Polygon<Geometry.Point> value, Func<WkbGeometryType, uint> getType) => WriteHeader(value, destination, littleEndian: false, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as big endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="value">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderBigEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointZ> value, Func<WkbGeometryType, uint> getType) => WriteHeader(value, destination, littleEndian: false, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as big endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="value">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderBigEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointM> value, Func<WkbGeometryType, uint> getType) => WriteHeader(value, destination, littleEndian: false, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as big endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="value">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderBigEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointZM> value, Func<WkbGeometryType, uint> getType) => WriteHeader(value, destination, littleEndian: false, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as big endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="values">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderBigEndian(Span<byte> destination, ICollection<Geometry.Polygon<Geometry.Point>> values, Func<WkbGeometryType, uint> getType) => WriteHeader(values, destination, littleEndian: false, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as big endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="values">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderBigEndian(Span<byte> destination, ICollection<Geometry.Polygon<Geometry.PointZ>> values, Func<WkbGeometryType, uint> getType) => WriteHeader(values, destination, littleEndian: false, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as big endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="values">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderBigEndian(Span<byte> destination, ICollection<Geometry.Polygon<Geometry.PointM>> values, Func<WkbGeometryType, uint> getType) => WriteHeader(values, destination, littleEndian: false, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as big endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="values">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderBigEndian(Span<byte> destination, ICollection<Geometry.Polygon<Geometry.PointZM>> values, Func<WkbGeometryType, uint> getType) => WriteHeader(values, destination, littleEndian: false, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as little endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="value">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderLittleEndian(Span<byte> destination, Geometry.Point value, Func<WkbGeometryType, uint> getType) => WriteHeader(value, destination, littleEndian: true, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as little endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="value">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderLittleEndian(Span<byte> destination, Geometry.PointZ value, Func<WkbGeometryType, uint> getType) => WriteHeader(value, destination, littleEndian: true, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as little endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="value">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderLittleEndian(Span<byte> destination, Geometry.PointM value, Func<WkbGeometryType, uint> getType) => WriteHeader(value, destination, littleEndian: true, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as little endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="value">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderLittleEndian(Span<byte> destination, Geometry.PointZM value, Func<WkbGeometryType, uint> getType) => WriteHeader(value, destination, littleEndian: true, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as little endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="values">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderLittleEndian(Span<byte> destination, ICollection<Geometry.Point> values, Func<WkbGeometryType, uint> getType) => WriteHeader(values, destination, littleEndian: true, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as little endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="values">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderLittleEndian(Span<byte> destination, ICollection<Geometry.PointZ> values, Func<WkbGeometryType, uint> getType) => WriteHeader(values, destination, littleEndian: true, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as little endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="values">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderLittleEndian(Span<byte> destination, ICollection<Geometry.PointM> values, Func<WkbGeometryType, uint> getType) => WriteHeader(values, destination, littleEndian: true, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as little endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="values">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderLittleEndian(Span<byte> destination, ICollection<Geometry.PointZM> values, Func<WkbGeometryType, uint> getType) => WriteHeader(values, destination, littleEndian: true, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as little endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="value">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderLittleEndian(Span<byte> destination, Geometry.Polyline<Geometry.Point> value, Func<WkbGeometryType, uint> getType) => WriteHeader(value, destination, littleEndian: true, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as little endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="value">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderLittleEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointZ> value, Func<WkbGeometryType, uint> getType) => WriteHeader(value, destination, littleEndian: true, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as little endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="value">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderLittleEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointM> value, Func<WkbGeometryType, uint> getType) => WriteHeader(value, destination, littleEndian: true, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as little endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="value">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderLittleEndian(Span<byte> destination, Geometry.Polyline<Geometry.PointZM> value, Func<WkbGeometryType, uint> getType) => WriteHeader(value, destination, littleEndian: true, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as little endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="values">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderLittleEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.Point>> values, Func<WkbGeometryType, uint> getType) => WriteHeader(values, destination, littleEndian: true, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as little endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="values">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderLittleEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointZ>> values, Func<WkbGeometryType, uint> getType) => WriteHeader(values, destination, littleEndian: true, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as little endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="values">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderLittleEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointM>> values, Func<WkbGeometryType, uint> getType) => WriteHeader(values, destination, littleEndian: true, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as little endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="values">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderLittleEndian(Span<byte> destination, ICollection<Geometry.Polyline<Geometry.PointZM>> values, Func<WkbGeometryType, uint> getType) => WriteHeader(values, destination, littleEndian: true, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as little endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="value">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderLittleEndian(Span<byte> destination, Geometry.Polygon<Geometry.Point> value, Func<WkbGeometryType, uint> getType) => WriteHeader(value, destination, littleEndian: true, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as little endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="value">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderLittleEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointZ> value, Func<WkbGeometryType, uint> getType) => WriteHeader(value, destination, littleEndian: true, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as little endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="value">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderLittleEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointM> value, Func<WkbGeometryType, uint> getType) => WriteHeader(value, destination, littleEndian: true, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as little endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="value">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderLittleEndian(Span<byte> destination, Geometry.Polygon<Geometry.PointZM> value, Func<WkbGeometryType, uint> getType) => WriteHeader(value, destination, littleEndian: true, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as little endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="values">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderLittleEndian(Span<byte> destination, ICollection<Geometry.Polygon<Geometry.Point>> values, Func<WkbGeometryType, uint> getType) => WriteHeader(values, destination, littleEndian: true, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as little endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="values">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderLittleEndian(Span<byte> destination, ICollection<Geometry.Polygon<Geometry.PointZ>> values, Func<WkbGeometryType, uint> getType) => WriteHeader(values, destination, littleEndian: true, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as little endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="values">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderLittleEndian(Span<byte> destination, ICollection<Geometry.Polygon<Geometry.PointM>> values, Func<WkbGeometryType, uint> getType) => WriteHeader(values, destination, littleEndian: true, getType, GetSizeAndType);

    /// <summary>
    /// Writes the header as little endian.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="values">The value.</param>
    /// <param name="getType">The function to get the type value.</param>
    /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
    internal static int WriteHeaderLittleEndian(Span<byte> destination, ICollection<Geometry.Polygon<Geometry.PointZM>> values, Func<WkbGeometryType, uint> getType) => WriteHeader(values, destination, littleEndian: true, getType, GetSizeAndType);

    private static T ReadGeometry<T>(ref ReadOnlySpan<byte> source, CreateFunction<T> func)
    {
        var (byteOrder, geometryType) = ReadByteOrderAndGeometryType(ref source);
        return func(ref source, byteOrder, geometryType);
    }

    private static Geometry.Point ReadPoint(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder)
    {
        var x = ReadCoordinate(ref source, byteOrder);
        var y = ReadCoordinate(ref source, byteOrder);
        return new(x, y);
    }

    private static Geometry.PointZ ReadPointZ(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder)
    {
        var x = ReadCoordinate(ref source, byteOrder);
        var y = ReadCoordinate(ref source, byteOrder);
        var z = ReadCoordinate(ref source, byteOrder);
        return new(x, y, z);
    }

    private static Geometry.PointM ReadPointM(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder)
    {
        var x = ReadCoordinate(ref source, byteOrder);
        var y = ReadCoordinate(ref source, byteOrder);
        var measurement = ReadCoordinate(ref source, byteOrder);
        return new(x, y, measurement);
    }

    private static Geometry.PointZM ReadPointZM(ref ReadOnlySpan<byte> source, WkbByteOrder byteOrder)
    {
        var x = ReadCoordinate(ref source, byteOrder);
        var y = ReadCoordinate(ref source, byteOrder);
        var z = ReadCoordinate(ref source, byteOrder);
        var m = ReadCoordinate(ref source, byteOrder);
        return new(x, y, z, m);
    }

    private static T[] ReadPoints<T>(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, CreatePointFunction<T> createPointFunction)
    {
        var numPoints = byteOrder is WkbByteOrder.Ndr
            ? ReadUInt32LittleEndian(span)
            : ReadUInt32BigEndian(span);
        span = span[4..];

        var points = new T[numPoints];
        for (var i = 0; i < numPoints; i++)
        {
            points[i] = createPointFunction(ref span, byteOrder);
        }

        return points;
    }

    private static Geometry.LinearRing<T>[] ReadLinearRings<T>(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, CreatePointFunction<T> createPointFunction)
        where T : struct
    {
        var count = byteOrder is WkbByteOrder.Ndr
            ? ReadUInt32LittleEndian(span)
            : ReadUInt32BigEndian(span);
        span = span[4..];

        var values = new Geometry.LinearRing<T>[count];
        for (var i = 0; i < count; i++)
        {
            values[i] = [.. ReadPoints(ref span, byteOrder, createPointFunction)];
        }

        return values;
    }

    private static Geometry.MultiGeometry<T> ReadMulti<T>(ReadOnlySpan<byte> span, WkbByteOrder byteOrder, CreateFunction<T> func)
        where T : Geometry.IGeometry
    {
        var count = byteOrder is WkbByteOrder.Ndr
            ? ReadUInt32LittleEndian(span)
            : ReadUInt32BigEndian(span);
        span = span[4..];

        var values = new T[count];
        for (var i = 0; i < count; i++)
        {
            values[i] = ReadGeometry(ref span, func);
        }

        return Geometry.MultiGeometry.Create<T>(values);
    }

    private static double ReadCoordinate(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder)
    {
        var value = BitConverter.Int64BitsToDouble(byteOrder is WkbByteOrder.Ndr
            ? ReadInt64LittleEndian(span)
            : ReadInt64BigEndian(span));
        span = span[8..];
        return value;
    }

    private static SizeAndType GetSizeAndType(Geometry.Point point) => (16, false, false, (uint)WkbGeometryType.Point);

    private static SizeAndType GetSizeAndType(Geometry.PointZ point) => (24, true, false, (uint)WkbGeometryType.PointZ);

    private static SizeAndType GetSizeAndType(Geometry.PointM point) => (24, false, true, (uint)WkbGeometryType.PointM);

    private static SizeAndType GetSizeAndType(Geometry.PointZM point) => (32, true, true, (uint)WkbGeometryType.PointZM);

    private static XY GetXY(Geometry.Point point) => (point.X, point.Y);

    private static XY GetXY(Geometry.PointZ point) => (point.X, point.Y);

    private static XY GetXY(Geometry.PointM point) => (point.X, point.Y);

    private static XY GetXY(Geometry.PointZM point) => (point.X, point.Y);

    private static double GetZ(Geometry.PointZ point) => point.Z;

    private static double GetZ(Geometry.PointZM point) => point.Z;

    private static double GetM(Geometry.PointM point) => point.Measurement;

    private static double GetM(Geometry.PointZM point) => point.Measurement;

    private static double Throw<T>(T pt) => throw new Geometry.InvalidGeometryTypeException();

    private static int GetMaxSize<T>(T value, bool includeMetadata, Func<T, SizeAndType> getSizeAndType)
        where T : struct
    {
        var (size, _, _, _) = getSizeAndType(value);
        if (includeMetadata)
        {
            size += 5;
        }

        return size;
    }

    private static int GetMaxSize<T>(ICollection<T> points, bool includeMetadata, Func<T, SizeAndType> getSizeAndType)
        where T : struct
    {
        var totalSize = 0;
        if (includeMetadata)
        {
            totalSize += 9;
        }

        using var enumerator = points.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            return totalSize;
        }

        var (size, _, _, _) = getSizeAndType(enumerator.Current);
        if (includeMetadata)
        {
            size += 5;
        }

        return totalSize + (size * points.Count);
    }

    private static int GetMaxSize<T>(Geometry.Polyline<T> line, Func<T, SizeAndType> getSizeAndType)
        where T : struct => 9 + GetMaxSize(line, includeMetadata: false, getSizeAndType);

    private static int GetMaxSize<T>(ICollection<Geometry.Polyline<T>> lines, Func<T, SizeAndType> getSizeAndType)
        where T : struct
    {
        var totalSize = 9;

        foreach (var line in lines)
        {
            totalSize += GetMaxSize(line, getSizeAndType);
        }

        return totalSize;
    }

    private static int GetMaxSize<T>(Geometry.Polygon<T> polygon, Func<T, SizeAndType> getSizeAndType)
        where T : struct
    {
        var totalSize = 9;

        foreach (var linearRing in polygon)
        {
            totalSize += sizeof(uint);
            totalSize += GetMaxSize(linearRing, includeMetadata: false, getSizeAndType);
        }

        return totalSize;
    }

    private static int GetMaxSize<T>(ICollection<Geometry.Polygon<T>> polygons, Func<T, SizeAndType> getSizeAndType)
        where T : struct
    {
        var totalSize = 9;

        foreach (var polygon in polygons)
        {
            totalSize += GetMaxSize(polygon, getSizeAndType);
        }

        return totalSize;
    }

    private static int WriteHeader<T>(T value, Span<byte> span, bool littleEndian, Func<WkbGeometryType, uint> getType, Func<T, SizeAndType> getSizeAndType)
    {
        var (_, _, _, geometryType) = getSizeAndType(value);
        return WriteHeader(span, (WkbGeometryType)geometryType, getType, littleEndian);
    }

    private static int WriteHeader<T>(ICollection<T> values, Span<byte> span, bool littleEndian, Func<WkbGeometryType, uint> getType, Func<T, SizeAndType> getSizeAndType)
    {
        var geometryType = WkbGeometryType.MultiPoint;
        if (values.Count is not 0)
        {
            var (_, _, _, type) = getSizeAndType(values.First());
            geometryType = (geometryType - WkbGeometryType.Point) + (WkbGeometryType)type;
        }

        return WriteHeader(span, geometryType, getType, littleEndian);
    }

    private static int WriteHeader<T>(Geometry.Polyline<T> values, Span<byte> span, bool littleEndian, Func<WkbGeometryType, uint> getType, Func<T, SizeAndType> getSizeAndType)
    {
        var geometryType = WkbGeometryType.LineString;
        if (values.Count is not 0)
        {
            var (_, _, _, type) = getSizeAndType(values[0]);
            geometryType = (geometryType - WkbGeometryType.Point) + (WkbGeometryType)type;
        }

        return WriteHeader(span, geometryType, getType, littleEndian);
    }

    private static int WriteHeader<T>(ICollection<Geometry.Polyline<T>> values, Span<byte> span, bool littleEndian, Func<WkbGeometryType, uint> getType, Func<T, SizeAndType> getSizeAndType)
    {
        var geometryType = WkbGeometryType.MultiLineString;
        if (values.Count is not 0)
        {
            using var enumerator = values.First().GetEnumerator();

            if (enumerator.MoveNext())
            {
                var (_, _, _, type) = getSizeAndType(enumerator.Current);
                geometryType = (geometryType - WkbGeometryType.Point) + (WkbGeometryType)type;
            }
        }

        return WriteHeader(span, geometryType, getType, littleEndian);
    }

    private static int WriteHeader<T>(Geometry.Polygon<T> polygon, Span<byte> span, bool littleEndian, Func<WkbGeometryType, uint> getType, Func<T, SizeAndType> getSizeAndType)
        where T : struct
    {
        // write the type
        var geometryType = WkbGeometryType.Polygon;
        if (polygon.Count is not 0)
        {
            var firstLinearRing = polygon[0];

            if (firstLinearRing.Count is not 0)
            {
                var (_, _, _, type) = getSizeAndType(firstLinearRing[0]);
                geometryType = (geometryType - WkbGeometryType.Point) + (WkbGeometryType)type;
            }
        }

        return WriteHeader(span, geometryType, getType, littleEndian);
    }

    private static int WriteHeader<T>(ICollection<Geometry.Polygon<T>> values, Span<byte> span, bool littleEndian, Func<WkbGeometryType, uint> getType, Func<T, SizeAndType> getSizeAndType)
    {
        var geometryType = WkbGeometryType.MultiPolygon;
        if (values.Count is not 0)
        {
            var firstPolygon = values.First();

            if (firstPolygon.Count is not 0)
            {
                var firstLinearRing = firstPolygon[0];

                if (firstLinearRing.Count is not 0)
                {
                    var (_, _, _, type) = getSizeAndType(firstLinearRing[0]);
                    geometryType = (geometryType - WkbGeometryType.Point) + (WkbGeometryType)type;
                }
            }
        }

        return WriteHeader(span, geometryType, getType, littleEndian);
    }

    private static int WriteHeader(Span<byte> destination, WkbGeometryType geometryType, Func<WkbGeometryType, uint> getType, bool littleEndian)
    {
        // write the byte order
        destination[0] = littleEndian ? (byte)1 : (byte)0;
        return 1 + WriteValue(destination[1..], getType(geometryType), littleEndian);
    }

    private static int Write<T>(
        ICollection<T> points,
        Span<byte> span,
        bool littleEndian,
        WkbGeometryType geometryType,
        bool includeMetadata,
        bool includeItemMetadata,
        Func<T, SizeAndType> getSizeAndType,
        Func<T, XY> getXY,
        Func<T, double> getZ,
        Func<T, double> getM)
        where T : struct
    {
        var count = points.Count;

        if (count is not 0)
        {
            var (_, _, _, type) = getSizeAndType(points.First());
            geometryType = (geometryType - WkbGeometryType.Point) + (WkbGeometryType)type;
        }

        var s = span;
        var written = 0;
        if (includeMetadata)
        {
            written = WriteHeader(s, geometryType, PassThrough, littleEndian);
            s = span[written..];
            written += WriteValue(s, (uint)count, littleEndian);
            s = span[written..];
        }

        foreach (var point in points)
        {
            written += Write(point, s, littleEndian, includeItemMetadata, getSizeAndType, getXY, getZ, getM);
            s = span[written..];
        }

        return written;
    }

    private static int Write<T>(ICollection<Geometry.Polyline<T>> lines, Span<byte> span, bool littleEndian, bool includeMetadata, Func<T, SizeAndType> getSizeAndType, Func<T, XY> getXY, Func<T, double> getZ, Func<T, double> getM)
        where T : struct
    {
        var geometryType = WkbGeometryType.MultiLineString;
        var count = lines.Count;

        if (count is not 0)
        {
            using var enumerator = lines.First().GetEnumerator();

            if (enumerator.MoveNext())
            {
                var (_, _, _, type) = getSizeAndType(enumerator.Current);
                geometryType = (geometryType - WkbGeometryType.Point) + (WkbGeometryType)type;
            }
        }

        var s = span;
        var written = 0;
        if (includeMetadata)
        {
            written = WriteHeader(s, geometryType, PassThrough, littleEndian);
            s = span[written..];
            written += WriteValue(s, (uint)count, littleEndian);
            s = span[written..];
        }

        foreach (var line in lines)
        {
            written += Write([.. line], s, littleEndian, WkbGeometryType.LineString, includeMetadata: true, includeItemMetadata: false, getSizeAndType, getXY, getZ, getM);
            s = span[written..];
        }

        return written;
    }

    private static int Write<T>(Geometry.Polygon<T> polygon, Span<byte> span, bool littleEndian, bool includeMetadata, Func<T, SizeAndType> getSizeAndType, Func<T, XY> getXY, Func<T, double> getZ, Func<T, double> getM)
        where T : struct
    {
        // write the type
        var geometryType = WkbGeometryType.Polygon;
        var count = polygon.Count;

        if (count is not 0)
        {
            var firstLinearRing = polygon[0];

            if (firstLinearRing.Count is not 0)
            {
                var (_, _, _, type) = getSizeAndType(firstLinearRing[0]);
                geometryType = (geometryType - WkbGeometryType.Point) + (WkbGeometryType)type;
            }
        }

        var s = span;
        var written = 0;
        if (includeMetadata)
        {
            written = WriteHeader(s, geometryType, PassThrough, littleEndian);
            s = span[written..];
            written += WriteValue(s, (uint)count, littleEndian);
            s = span[written..];
        }

        foreach (var linearRing in polygon)
        {
            if (littleEndian)
            {
                WriteUInt32LittleEndian(s, (uint)linearRing.Count);
            }
            else
            {
                WriteUInt32BigEndian(s, (uint)linearRing.Count);
            }

            written += sizeof(uint);
            s = span[written..];

            foreach (var point in linearRing)
            {
                written += Write(point, s, littleEndian, includeMetadata: false, getSizeAndType: getSizeAndType, getXY: getXY, getZ: getZ, getM: getM);
                s = span[written..];
            }
        }

        return written;
    }

    private static int Write<T>(ICollection<Geometry.Polygon<T>> polygons, Span<byte> span, bool littleEndian, bool includeMetadata, Func<T, SizeAndType> getSizeAndType, Func<T, XY> getXY, Func<T, double> getZ, Func<T, double> getM)
        where T : struct
    {
        // write the type
        var geometryType = WkbGeometryType.MultiPolygon;
        var count = polygons.Count;

        if (count is not 0)
        {
            var firstPolygon = polygons.First();

            if (firstPolygon.Count is not 0)
            {
                var firstLinearRing = firstPolygon[0];

                if (firstLinearRing.Count is not 0)
                {
                    var (_, _, _, type) = getSizeAndType(firstLinearRing[0]);
                    geometryType = (geometryType - WkbGeometryType.Point) + (WkbGeometryType)type;
                }
            }
        }

        var s = span;
        var written = 0;
        if (includeMetadata)
        {
            written = WriteHeader(s, geometryType, PassThrough, littleEndian);
            s = span[written..];
            written += WriteValue(s, (uint)count, littleEndian);
            s = span[written..];
        }

        foreach (var polygon in polygons)
        {
            written += Write(polygon, s, littleEndian, includeMetadata: true, getSizeAndType, getXY, getZ, getM);
            s = span[written..];
        }

        return written;
    }

    private static int Write<T>(T point, Span<byte> span, bool littleEndian, bool includeMetadata, Func<T, SizeAndType> getSizeAndType, Func<T, XY> getXY, Func<T, double> getZ, Func<T, double> getM)
        where T : struct
    {
        // write the type
        var (_, hasZ, hasM, type) = getSizeAndType(point);

        var written = 0;
        if (includeMetadata)
        {
            written = WriteHeader(span, (WkbGeometryType)type, PassThrough, littleEndian);
            span = span[written..];
        }

        var idx = 0;
        var (x, y) = getXY(point);
        written += WriteValue(span[idx..], x, littleEndian);
        idx += 8;
        written += WriteValue(span[idx..], y, littleEndian);

        if (hasZ)
        {
            idx += 8;
            written += WriteValue(span[idx..], getZ(point), littleEndian);
        }

        if (hasM)
        {
            idx += 8;
            written += WriteValue(span[idx..], getM(point), littleEndian);
        }

        return written;
    }

    private static int WriteValue(Span<byte> span, double value, bool isLittleEndian)
    {
        if (isLittleEndian)
        {
            WriteInt64LittleEndian(span, BitConverter.DoubleToInt64Bits(value));
        }
        else
        {
            WriteInt64BigEndian(span, BitConverter.DoubleToInt64Bits(value));
        }

        return sizeof(double);
    }

    private static int WriteValue(Span<byte> span, uint value, bool isLittleEndian)
    {
        if (isLittleEndian)
        {
            WriteUInt32LittleEndian(span, value);
        }
        else
        {
            WriteUInt32BigEndian(span, value);
        }

        return sizeof(uint);
    }

    private static uint PassThrough(WkbGeometryType geometryType) => (uint)geometryType;
}