// -----------------------------------------------------------------------
// <copyright file="WkbRecord.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

using static System.Buffers.Binary.BinaryPrimitives;

/// <summary>
/// Represents an implementation of <see cref="Data.IGeometryRecord"/> that reads Well-Known Binary.
/// </summary>
public class WkbRecord : Data.Common.BinaryGeometryRecord
{
    /// <summary>
    /// Initialises a new instance of the <see cref="WkbRecord"/> class.
    /// </summary>
    /// <param name="bytes">The byte array to read from.</param>
    public WkbRecord(byte[] bytes)
        : base(bytes)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="WkbRecord"/> class.
    /// </summary>
    /// <param name="bytes">The byte array to read from.</param>
    /// <param name="index">The index in <paramref name="bytes"/> at which the stream begins.</param>
    /// <param name="count">The length of the stream in bytes.</param>
    public WkbRecord(byte[] bytes, int index, int count)
        : base(bytes, index, count)
    {
    }

    /// <summary>
    /// Creates a geometry instance.
    /// </summary>
    /// <typeparam name="T">The type of geometry.</typeparam>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    /// <returns>The created geometry.</returns>
    protected delegate T CreateFunction<out T>(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType);

    private delegate T CreatePointFunction<out T>(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder);

    /// <summary>
    /// The Well-Known Binary byte order.
    /// </summary>
    protected enum WkbByteOrder : byte
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
    protected enum WkbGeometryType : uint
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

    /// <inheritdoc />
    public override Point GetPoint() => this.GetGeometry(GetPoint);

    /// <inheritdoc />
    public override PointZ GetPointZ() => this.GetGeometry(GetPointZ);

    /// <inheritdoc />
    public override PointM GetPointM() => this.GetGeometry(GetPointM);

    /// <inheritdoc />
    public override PointZM GetPointZM() => this.GetGeometry(GetPointZM);

    /// <inheritdoc/>
    public override IReadOnlyCollection<Point> GetMultiPoint() => this.GetGeometry(GetMultiPoint);

    /// <inheritdoc/>
    public override IReadOnlyCollection<PointZ> GetMultiPointZ() => this.GetGeometry(GetMultiPointZ);

    /// <inheritdoc/>
    public override IReadOnlyCollection<PointM> GetMultiPointM() => this.GetGeometry(GetMultiPointM);

    /// <inheritdoc/>
    public override IReadOnlyCollection<PointZM> GetMultiPointZM() => this.GetGeometry(GetMultiPointZM);

    /// <inheritdoc />
    public override Polyline GetLineString() => this.GetGeometry(GetLineString);

    /// <inheritdoc />
    public override PolylineZ GetLineStringZ() => this.GetGeometry(GetLineStringZ);

    /// <inheritdoc />
    public override PolylineM GetLineStringM() => this.GetGeometry(GetLineStringM);

    /// <inheritdoc />
    public override PolylineZM GetLineStringZM() => this.GetGeometry(GetLineStringZM);

    /// <inheritdoc />
    public override IReadOnlyCollection<Polyline> GetMultiLineString() => this.GetGeometry(GetMultiLineString);

    /// <inheritdoc />
    public override IReadOnlyCollection<PolylineZ> GetMultiLineStringZ() => this.GetGeometry(GetMultiLineStringZ);

    /// <inheritdoc />
    public override IReadOnlyCollection<PolylineM> GetMultiLineStringM() => this.GetGeometry(GetMultiLineStringM);

    /// <inheritdoc />
    public override IReadOnlyCollection<PolylineZM> GetMultiLineStringZM() => this.GetGeometry(GetMultiLineStringZM);

    /// <inheritdoc />
    public override Polygon GetPolygon() => this.GetGeometry(GetPolygon);

    /// <inheritdoc />
    public override PolygonZ GetPolygonZ() => this.GetGeometry(GetPolygonZ);

    /// <inheritdoc />
    public override PolygonM GetPolygonM() => this.GetGeometry(GetPolygonM);

    /// <inheritdoc />
    public override PolygonZM GetPolygonZM() => this.GetGeometry(GetPolygonZM);

    /// <inheritdoc />
    public override IReadOnlyCollection<Polygon> GetMultiPolygon() => this.GetGeometry(GetMultiPolygon);

    /// <inheritdoc />
    public override IReadOnlyCollection<PolygonZ> GetMultiPolygonZ() => this.GetGeometry(GetMultiPolygonZ);

    /// <inheritdoc />
    public override IReadOnlyCollection<PolygonM> GetMultiPolygonM() => this.GetGeometry(GetMultiPolygonM);

    /// <inheritdoc />
    public override IReadOnlyCollection<PolygonZM> GetMultiPolygonZM() => this.GetGeometry(GetMultiPolygonZM);

    /// <inheritdoc/>
    public override object? GetGeometry() => this.GetGeometry(GetGeometry);

    /// <inheritdoc />
    public override bool IsNull()
    {
        var span = this.AsSpan();
        return span.Length <= 5;
    }

    /// <summary>
    /// Gets the byte order and geometry type.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <returns>The byte order and geometry type.</returns>
    protected static (WkbByteOrder ByteOrder, WkbGeometryType GeometryType) GetByteOrderAndGeometryType(ref ReadOnlySpan<byte> span)
    {
        var byteOrder = (WkbByteOrder)span[0];
        span = span[1..];
        var geometryType = (WkbGeometryType)(byteOrder is WkbByteOrder.Ndr ? ReadUInt32LittleEndian(span) : ReadUInt32BigEndian(span));
        span = span[4..];
        return (byteOrder, geometryType);
    }

    /// <inheritdoc cref="GetPoint()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected static Point GetPoint(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType) =>
        geometryType switch
        {
            WkbGeometryType.Point => GetPoint(ref span, byteOrder),
            var v => throw new InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.Point)}"),
        };

    /// <inheritdoc cref="GetPoint()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    protected static Point GetPoint(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder)
    {
        var x = GetCoordinate(ref span, byteOrder);
        var y = GetCoordinate(ref span, byteOrder);
        return new(x, y);
    }

    /// <inheritdoc cref="GetPointZ()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected static PointZ GetPointZ(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType) =>
        geometryType switch
        {
            WkbGeometryType.PointZ => GetPointZ(ref span, byteOrder),
            var v => throw new InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.PointZ)}"),
        };

    /// <inheritdoc cref="GetPointZ()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    protected static PointZ GetPointZ(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder)
    {
        var x = GetCoordinate(ref span, byteOrder);
        var y = GetCoordinate(ref span, byteOrder);
        var z = GetCoordinate(ref span, byteOrder);
        return new(x, y, z);
    }

    /// <inheritdoc cref="GetPointM()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected static PointM GetPointM(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType) =>
        geometryType switch
        {
            WkbGeometryType.PointM => GetPointM(ref span, byteOrder),
            var v => throw new InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.PointZ)}"),
        };

    /// <inheritdoc cref="GetPointM()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    protected static PointM GetPointM(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder)
    {
        var x = GetCoordinate(ref span, byteOrder);
        var y = GetCoordinate(ref span, byteOrder);
        var measurement = GetCoordinate(ref span, byteOrder);
        return new(x, y, measurement);
    }

    /// <inheritdoc cref="GetPointZM()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected static PointZM GetPointZM(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.PointZM => GetPointZM(ref span, byteOrder),
        var v => throw new InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.PointZM)}"),
    };

    /// <inheritdoc cref="GetPointZM()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    protected static PointZM GetPointZM(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder)
    {
        var x = GetCoordinate(ref span, byteOrder);
        var y = GetCoordinate(ref span, byteOrder);
        var z = GetCoordinate(ref span, byteOrder);
        var m = GetCoordinate(ref span, byteOrder);
        return new(x, y, z, m);
    }

    /// <inheritdoc cref="GetMultiPoint()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected static IReadOnlyCollection<Point> GetMultiPoint(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.MultiPoint => GetMulti(span, byteOrder, GetPoint),
        var v => throw new InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.MultiPoint)}"),
    };

    /// <inheritdoc cref="GetMultiPointZ()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected static IReadOnlyCollection<PointZ> GetMultiPointZ(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.MultiPointZ => GetMulti(span, byteOrder, GetPointZ),
        var v => throw new InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.MultiPointZ)}"),
    };

    /// <inheritdoc cref="GetMultiPointM()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected static IReadOnlyCollection<PointM> GetMultiPointM(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.MultiPointM => GetMulti(span, byteOrder, GetPointM),
        var v => throw new InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.MultiPointZ)}"),
    };

    /// <inheritdoc cref="GetMultiPointZM()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected static IReadOnlyCollection<PointZM> GetMultiPointZM(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.MultiPointZM => GetMulti(span, byteOrder, GetPointZM),
        var v => throw new InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.MultiPointZM)}"),
    };

    /// <inheritdoc cref="GetLineString()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected static Polyline GetLineString(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.LineString => [..GetPoints(ref span, byteOrder, GetPoint)],
        var v => throw new InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.LineString)}"),
    };

    /// <inheritdoc cref="GetLineStringZ()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected static PolylineZ GetLineStringZ(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.LineStringZ => [..GetPoints(ref span, byteOrder, GetPointZ)],
        var v => throw new InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.LineString)}"),
    };

    /// <inheritdoc cref="GetLineStringM()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected static PolylineM GetLineStringM(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.LineStringM => [..GetPoints(ref span, byteOrder, GetPointM)],
        var v => throw new InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.LineString)}"),
    };

    /// <inheritdoc cref="GetLineStringZM()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected static PolylineZM GetLineStringZM(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.LineStringZM => [..GetPoints(ref span, byteOrder, GetPointZM)],
        var v => throw new InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.LineString)}"),
    };

    /// <inheritdoc cref="GetMultiLineString()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected static IReadOnlyCollection<Polyline> GetMultiLineString(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.MultiLineString => GetMulti(span, byteOrder, GetLineString),
        var v => throw new InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.MultiLineString)}"),
    };

    /// <inheritdoc cref="GetMultiLineStringZ()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected static IReadOnlyCollection<PolylineZ> GetMultiLineStringZ(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.MultiLineStringZ or WkbGeometryType.MultiLineStringM => GetMulti(span, byteOrder, GetLineStringZ),
        var v => throw new InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.MultiLineString)}"),
    };

    /// <inheritdoc cref="GetMultiLineStringM()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected static IReadOnlyCollection<PolylineM> GetMultiLineStringM(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.MultiLineStringM => GetMulti(span, byteOrder, GetLineStringM),
        var v => throw new InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.MultiLineString)}"),
    };

    /// <inheritdoc cref="GetMultiLineStringZM()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected static IReadOnlyCollection<PolylineZM> GetMultiLineStringZM(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.MultiLineStringZM => GetMulti(span, byteOrder, GetLineStringZM),
        var v => throw new InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.MultiLineString)}"),
    };

    /// <inheritdoc cref="GetPolygon()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected static Polygon GetPolygon(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.Polygon => [.. GetLinearRings(ref span, byteOrder, GetPoint)],
        var v => throw new InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.Polygon)}"),
    };

    /// <inheritdoc cref="GetPolygonZ()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected static PolygonZ GetPolygonZ(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.PolygonZ => [.. GetLinearRings(ref span, byteOrder, GetPointZ)],
        var v => throw new InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.Polygon)}"),
    };

    /// <inheritdoc cref="GetPolygonM()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected static PolygonM GetPolygonM(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.PolygonM => [.. GetLinearRings(ref span, byteOrder, GetPointM)],
        var v => throw new InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.Polygon)}"),
    };

    /// <inheritdoc cref="GetPolygonZM()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected static PolygonZM GetPolygonZM(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.PolygonZM => [.. GetLinearRings(ref span, byteOrder, GetPointZM)],
        var v => throw new InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.Polygon)}"),
    };

    /// <inheritdoc cref="GetMultiPolygon()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected static IReadOnlyCollection<Polygon> GetMultiPolygon(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.MultiPolygon => GetMulti(span, byteOrder, GetPolygon),
        var v => throw new InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.MultiPolygon)}"),
    };

    /// <inheritdoc cref="GetMultiPolygonZ()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected static IReadOnlyCollection<PolygonZ> GetMultiPolygonZ(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.MultiPolygonZ => GetMulti(span, byteOrder, GetPolygonZ),
        var v => throw new InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.MultiPolygon)}"),
    };

    /// <inheritdoc cref="GetMultiPolygonM()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected static IReadOnlyCollection<PolygonM> GetMultiPolygonM(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.MultiPolygonM => GetMulti(span, byteOrder, GetPolygonM),
        var v => throw new InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.MultiPolygon)}"),
    };

    /// <inheritdoc cref="GetMultiPolygonZM()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected static IReadOnlyCollection<PolygonZM> GetMultiPolygonZM(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.MultiPolygonZM => GetMulti(span, byteOrder, GetPolygonZM),
        var v => throw new InvalidGeometryTypeException($"Geometry in stream ({(uint)v}) was not a {nameof(WkbGeometryType.MultiPolygon)}"),
    };

    /// <inheritdoc cref="GetGeometry()"/>
    /// <param name="span">The span to read from.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected static object? GetGeometry(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, WkbGeometryType geometryType) => geometryType switch
    {
        WkbGeometryType.Point => GetPoint(ref span, byteOrder),
        WkbGeometryType.PointZ => GetPointZ(ref span, byteOrder),
        WkbGeometryType.PointM => GetPointM(ref span, byteOrder),
        WkbGeometryType.PointZM => GetPointZM(ref span, byteOrder),
        WkbGeometryType.MultiPoint => GetMulti(span, byteOrder, GetPoint),
        WkbGeometryType.MultiPointZ => GetMulti(span, byteOrder, GetPointZ),
        WkbGeometryType.MultiPointM => GetMulti(span, byteOrder, GetPointM),
        WkbGeometryType.MultiPointZM => GetMulti(span, byteOrder, GetPointZM),
        WkbGeometryType.LineString => Polyline.FromPoints(GetPoints(ref span, byteOrder, GetPoint)),
        WkbGeometryType.LineStringZ => PolylineZ.FromPoints(GetPoints(ref span, byteOrder, GetPointZ)),
        WkbGeometryType.LineStringM => PolylineM.FromPoints(GetPoints(ref span, byteOrder, GetPointM)),
        WkbGeometryType.LineStringZM => PolylineZM.FromPoints(GetPoints(ref span, byteOrder, GetPointZM)),
        WkbGeometryType.MultiLineString => GetMulti(span, byteOrder, GetLineString),
        WkbGeometryType.MultiLineStringZ => GetMulti(span, byteOrder, GetLineStringZ),
        WkbGeometryType.MultiLineStringM => GetMulti(span, byteOrder, GetLineStringM),
        WkbGeometryType.MultiLineStringZM => GetMulti(span, byteOrder, GetLineStringZM),
        WkbGeometryType.Polygon => new Polygon(GetLinearRings(ref span, byteOrder, GetPoint)),
        WkbGeometryType.PolygonZ => new PolygonZ(GetLinearRings(ref span, byteOrder, GetPointZ)),
        WkbGeometryType.PolygonM => new PolygonM(GetLinearRings(ref span, byteOrder, GetPointM)),
        WkbGeometryType.PolygonZM => new PolygonZM(GetLinearRings(ref span, byteOrder, GetPointZM)),
        WkbGeometryType.MultiPolygon => GetMulti(span, byteOrder, GetPolygon),
        WkbGeometryType.MultiPolygonZ => GetMulti(span, byteOrder, GetPolygonZ),
        WkbGeometryType.MultiPolygonM => GetMulti(span, byteOrder, GetPolygonM),
        WkbGeometryType.MultiPolygonZM => GetMulti(span, byteOrder, GetPolygonZM),
        _ => null,
    };

    /// <inheritdoc cref="GetPoint()"/>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected virtual Point GetPoint(WkbByteOrder byteOrder, WkbGeometryType geometryType)
    {
        var span = this.AsSpan();
        return GetPoint(ref span, byteOrder, geometryType);
    }

    /// <inheritdoc cref="GetPointZ()"/>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected virtual PointZ GetPointZ(WkbByteOrder byteOrder, WkbGeometryType geometryType)
    {
        var span = this.AsSpan();
        return GetPointZ(ref span, byteOrder, geometryType);
    }

    /// <inheritdoc cref="GetPointZM()"/>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected virtual PointZM GetPointZM(WkbByteOrder byteOrder, WkbGeometryType geometryType)
    {
        var span = this.AsSpan();
        return GetPointZM(ref span, byteOrder, geometryType);
    }

    /// <inheritdoc cref="GetMultiPoint()"/>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected virtual IEnumerable<Point> GetMultiPoint(WkbByteOrder byteOrder, WkbGeometryType geometryType)
    {
        var span = this.AsSpan();
        return GetMultiPoint(ref span, byteOrder, geometryType);
    }

    /// <inheritdoc cref="GetMultiPointZ()"/>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected virtual IEnumerable<PointZ> GetMultiPointZ(WkbByteOrder byteOrder, WkbGeometryType geometryType)
    {
        var span = this.AsSpan();
        return GetMultiPointZ(ref span, byteOrder, geometryType);
    }

    /// <inheritdoc cref="GetMultiPointZM()"/>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected virtual IEnumerable<PointZM> GetMultiPointZM(WkbByteOrder byteOrder, WkbGeometryType geometryType)
    {
        var span = this.AsSpan();
        return GetMultiPointZM(ref span, byteOrder, geometryType);
    }

    /// <inheritdoc cref="GetLineString()"/>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected virtual Polyline GetLineString(WkbByteOrder byteOrder, WkbGeometryType geometryType)
    {
        var span = this.AsSpan();
        return GetLineString(ref span, byteOrder, geometryType);
    }

    /// <inheritdoc cref="GetLineStringZ()"/>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected virtual PolylineZ GetLineStringZ(WkbByteOrder byteOrder, WkbGeometryType geometryType)
    {
        var span = this.AsSpan();
        return GetLineStringZ(ref span, byteOrder, geometryType);
    }

    /// <inheritdoc cref="GetLineStringZM()"/>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected virtual PolylineZM GetLineStringZM(WkbByteOrder byteOrder, WkbGeometryType geometryType)
    {
        var span = this.AsSpan();
        return GetLineStringZM(ref span, byteOrder, geometryType);
    }

    /// <inheritdoc cref="GetMultiLineString()"/>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected virtual IEnumerable<Polyline> GetMultiLineString(WkbByteOrder byteOrder, WkbGeometryType geometryType)
    {
        var span = this.AsSpan();
        return GetMultiLineString(ref span, byteOrder, geometryType);
    }

    /// <inheritdoc cref="GetMultiLineStringZ()"/>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected virtual IEnumerable<PolylineZ> GetMultiLineStringZ(WkbByteOrder byteOrder, WkbGeometryType geometryType)
    {
        var span = this.AsSpan();
        return GetMultiLineStringZ(ref span, byteOrder, geometryType);
    }

    /// <inheritdoc cref="GetMultiLineStringZM()"/>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected virtual IEnumerable<PolylineZM> GetMultiLineStringZM(WkbByteOrder byteOrder, WkbGeometryType geometryType)
    {
        var span = this.AsSpan();
        return GetMultiLineStringZM(ref span, byteOrder, geometryType);
    }

    /// <inheritdoc cref="GetPolygon()"/>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected virtual Polygon GetPolygon(WkbByteOrder byteOrder, WkbGeometryType geometryType)
    {
        var span = this.AsSpan();
        return GetPolygon(ref span, byteOrder, geometryType);
    }

    /// <inheritdoc cref="GetPolygonZ()"/>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected virtual PolygonZ GetPolygonZ(WkbByteOrder byteOrder, WkbGeometryType geometryType)
    {
        var span = this.AsSpan();
        return GetPolygonZ(ref span, byteOrder, geometryType);
    }

    /// <inheritdoc cref="GetPolygonZM()"/>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected virtual PolygonZM GetPolygonZM(WkbByteOrder byteOrder, WkbGeometryType geometryType)
    {
        var span = this.AsSpan();
        return GetPolygonZM(ref span, byteOrder, geometryType);
    }

    /// <inheritdoc cref="GetMultiPolygon()"/>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected virtual IReadOnlyCollection<Polygon> GetMultiPolygon(WkbByteOrder byteOrder, WkbGeometryType geometryType)
    {
        var span = this.AsSpan();
        return GetMultiPolygon(ref span, byteOrder, geometryType);
    }

    /// <inheritdoc cref="GetMultiPolygonZ()"/>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected virtual IReadOnlyCollection<PolygonZ> GetMultiPolygonZ(WkbByteOrder byteOrder, WkbGeometryType geometryType)
    {
        var span = this.AsSpan();
        return GetMultiPolygonZ(ref span, byteOrder, geometryType);
    }

    /// <inheritdoc cref="GetMultiPolygonZM()"/>
    /// <param name="byteOrder">The byte order.</param>
    /// <param name="geometryType">The geometry type.</param>
    protected virtual IReadOnlyCollection<PolygonZM> GetMultiPolygonZM(WkbByteOrder byteOrder, WkbGeometryType geometryType)
    {
        var span = this.AsSpan();
        return GetMultiPolygonZM(ref span, byteOrder, geometryType);
    }

    /// <inheritdoc cref="GetGeometry()"/>
    /// <param name="func">The create geometry function.</param>
    protected virtual T GetGeometry<T>(CreateFunction<T> func)
    {
        var span = this.AsSpan();
        return GetGeometry(ref span, func);
    }

    private static T GetGeometry<T>(ref ReadOnlySpan<byte> span, CreateFunction<T> func)
    {
        var (byteOrder, geometryType) = GetByteOrderAndGeometryType(ref span);
        return func(ref span, byteOrder, geometryType);
    }

    private static T[] GetPoints<T>(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, CreatePointFunction<T> createPointFunction)
    {
        var numPoints = byteOrder is WkbByteOrder.Ndr ? ReadUInt32LittleEndian(span) : ReadUInt32BigEndian(span);
        span = span[4..];

        var points = new T[numPoints];
        for (var i = 0; i < numPoints; i++)
        {
            points[i] = createPointFunction(ref span, byteOrder);
        }

        return points;
    }

    private static LinearRing<T>[] GetLinearRings<T>(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder, CreatePointFunction<T> createPointFunction)
        where T : struct
    {
        var count = byteOrder is WkbByteOrder.Ndr ? ReadUInt32LittleEndian(span) : ReadUInt32BigEndian(span);
        span = span[4..];

        var values = new LinearRing<T>[count];
        for (var i = 0; i < count; i++)
        {
            values[i] = [.. GetPoints(ref span, byteOrder, createPointFunction)];
        }

        return values;
    }

    private static T[] GetMulti<T>(ReadOnlySpan<byte> span, WkbByteOrder byteOrder, CreateFunction<T> func)
    {
        var count = byteOrder is WkbByteOrder.Ndr ? ReadUInt32LittleEndian(span) : ReadUInt32BigEndian(span);
        span = span[4..];

        var values = new T[count];
        for (var i = 0; i < count; i++)
        {
            values[i] = GetGeometry(ref span, func);
        }

        return values;
    }

    private static double GetCoordinate(ref ReadOnlySpan<byte> span, WkbByteOrder byteOrder)
    {
        var value = BitConverter.Int64BitsToDouble(byteOrder is WkbByteOrder.Ndr ? ReadInt64LittleEndian(span) : ReadInt64BigEndian(span));
        span = span[8..];
        return value;
    }
}