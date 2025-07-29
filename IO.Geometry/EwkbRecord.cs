// -----------------------------------------------------------------------
// <copyright file="EwkbRecord.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

/// <summary>
/// Represents an extended <see cref="WkbRecord"/>.
/// </summary>
public class EwkbRecord : WkbRecord, Data.ISridGeometryRecord
{
    /// <summary>
    /// Initialises a new instance of the <see cref="EwkbRecord"/> class.
    /// </summary>
    /// <param name="bytes">The byte array to read from.</param>
    public EwkbRecord(byte[] bytes)
        : base(bytes)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="EwkbRecord"/> class.
    /// </summary>
    /// <param name="bytes">The byte array to read from.</param>
    /// <param name="index">The index in <paramref name="bytes"/> at which the stream begins.</param>
    /// <param name="count">The length of the stream in bytes.</param>
    public EwkbRecord(byte[] bytes, int index, int count)
        : base(bytes, index, count)
    {
    }

    /// <inheritdoc/>
    public int GetSrid()
    {
        var span = this.AsSpan();
        var (byteOrder, geometryType) = GetByteOrderAndGeometryType(ref span);
        return GetSrid(span, (int)geometryType, byteOrder);
    }

    /// <inheritdoc />
    public override bool IsNull()
    {
        var span = this.AsSpan();
        return span.Length <= 9;
    }

    /// <inheritdoc/>
    protected override T GetGeometry<T>(CreateFunction<T> func)
    {
        var span = this.AsSpan();
        var (byteOrder, geometryType) = GetByteOrderAndGeometryType(ref span);
        return GetGeometry(span[4..], byteOrder, geometryType, func);
    }

    /// <inheritdoc cref="GetSrid()"/>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected virtual int GetSrid(WkbByteOrder byteOrder, WkbGeometryType geometryType) => GetSrid(this.AsSpan(5), (int)geometryType, byteOrder);

    private static int GetSrid(ReadOnlySpan<byte> span, int type, WkbByteOrder byteOrder)
    {
        if ((type & 0x20000000) is not 0)
        {
            // read the SRID
            return byteOrder switch
            {
                WkbByteOrder.Ndr => System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span),
                WkbByteOrder.Xdr => System.Buffers.Binary.BinaryPrimitives.ReadInt32BigEndian(span),
                _ => throw new InvalidOperationException(),
            };
        }

        return 0;
    }

    private static T GetGeometry<T>(ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType, CreateFunction<T> func)
    {
        return func(ref span, byteOrder, GetGeometryType((int)geometryType));

        static WkbGeometryType GetGeometryType(int type)
        {
            var geometryType = (WkbGeometryType)(type & 0x1FFFFFFF);
            var hasZ = (type & 0x80000000) is not 0;
            var hasM = (type & 0x40000000) is not 0;

            return (geometryType, hasZ, hasM) switch
            {
                (WkbGeometryType.Point, false, false) => WkbGeometryType.Point,
                (WkbGeometryType.Point, true, false) => WkbGeometryType.PointZ,
                (WkbGeometryType.Point, false, true) => WkbGeometryType.PointM,
                (WkbGeometryType.Point, true, true) => WkbGeometryType.PointZM,
                (WkbGeometryType.MultiPoint, false, false) => WkbGeometryType.MultiPoint,
                (WkbGeometryType.MultiPoint, true, false) => WkbGeometryType.MultiPointZ,
                (WkbGeometryType.MultiPoint, false, true) => WkbGeometryType.MultiPointM,
                (WkbGeometryType.MultiPoint, true, true) => WkbGeometryType.MultiPointZM,
                (WkbGeometryType.LineString, false, false) => WkbGeometryType.LineString,
                (WkbGeometryType.LineString, true, false) => WkbGeometryType.LineStringZ,
                (WkbGeometryType.LineString, false, true) => WkbGeometryType.LineStringM,
                (WkbGeometryType.LineString, true, true) => WkbGeometryType.LineStringZM,
                (WkbGeometryType.MultiLineString, false, false) => WkbGeometryType.MultiLineString,
                (WkbGeometryType.MultiLineString, true, false) => WkbGeometryType.MultiLineStringZ,
                (WkbGeometryType.MultiLineString, false, true) => WkbGeometryType.MultiLineStringM,
                (WkbGeometryType.MultiLineString, true, true) => WkbGeometryType.MultiLineStringZM,
                (WkbGeometryType.Polygon, false, false) => WkbGeometryType.Polygon,
                (WkbGeometryType.Polygon, true, false) => WkbGeometryType.PolygonZ,
                (WkbGeometryType.Polygon, false, true) => WkbGeometryType.PolygonM,
                (WkbGeometryType.Polygon, true, true) => WkbGeometryType.PolygonZM,
                (WkbGeometryType.MultiPolygon, false, false) => WkbGeometryType.MultiPolygon,
                (WkbGeometryType.MultiPolygon, true, false) => WkbGeometryType.MultiPolygonZ,
                (WkbGeometryType.MultiPolygon, false, true) => WkbGeometryType.MultiPolygonM,
                (WkbGeometryType.MultiPolygon, true, true) => WkbGeometryType.MultiPolygonZM,
                (WkbGeometryType.Geometry, false, false) => WkbGeometryType.Geometry,
                (WkbGeometryType.Geometry, true, false) => WkbGeometryType.GeometryZ,
                (WkbGeometryType.Geometry, false, true) => WkbGeometryType.GeometryM,
                (WkbGeometryType.Geometry, true, true) => WkbGeometryType.GeometryZM,
                (WkbGeometryType.GeometryCollection, false, false) => WkbGeometryType.GeometryCollection,
                (WkbGeometryType.GeometryCollection, true, false) => WkbGeometryType.GeometryCollectionZ,
                (WkbGeometryType.GeometryCollection, false, true) => WkbGeometryType.GeometryCollectionM,
                (WkbGeometryType.GeometryCollection, true, true) => WkbGeometryType.GeometryCollectionZM,
                _ => throw new NotSupportedException(),
            };
        }
    }
}