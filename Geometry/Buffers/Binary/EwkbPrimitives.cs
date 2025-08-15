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
}