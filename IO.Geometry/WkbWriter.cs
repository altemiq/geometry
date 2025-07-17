// -----------------------------------------------------------------------
// <copyright file="WkbWriter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

using static System.Buffers.Binary.BinaryPrimitives;

using SizeAndType = System.ValueTuple<int, bool, bool, uint>;
using XY = System.ValueTuple<double, double>;

/// <summary>
/// Represents an implementation of <see cref="Data.IGeometryWriter"/> that writes Well-Known Binary.
/// </summary>
public class WkbWriter : Data.Common.BinaryGeometryWriter
{
    /// <summary>
    /// Initialises a new instance of the <see cref="WkbWriter"/> class.
    /// </summary>
    /// <param name="bytes">The byte array to write to.</param>
    public WkbWriter(byte[] bytes)
        : base(bytes)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="WkbWriter"/> class.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    public WkbWriter(Stream stream)
        : base(stream)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="WkbWriter"/> class.
    /// </summary>
    /// <param name="bytes">The byte array to write to.</param>
    /// <param name="littleEndian">Set to <see langword="true"/> to treat the data as little endian.</param>
    public WkbWriter(byte[] bytes, bool littleEndian)
        : base(bytes, littleEndian)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="WkbWriter"/> class.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="littleEndian">Set to <see langword="true"/> to treat the data as little endian.</param>
    public WkbWriter(Stream stream, bool littleEndian)
        : base(stream, littleEndian)
    {
    }

    /// <summary>
    /// The WKB integer codes.
    /// </summary>
    protected enum WkbGeometryType : uint
    {
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
        /// Point with Z value.
        /// </summary>
        PointZ = 1001,

        /// <summary>
        /// Point with M value.
        /// </summary>
        PointM = 2001,

        /// <summary>
        /// Point with Z and M values.
        /// </summary>
        PointZM = 3001,
    }

    /// <inheritdoc/>
    public override void Write(Point point) => this.Write(point, includeMetadata: true, GetSizeAndType, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public override void Write(PointZ point) => this.Write(point, includeMetadata: true, GetSizeAndType, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public override void Write(PointM point) => this.Write(point, includeMetadata: true, GetSizeAndType, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public override void Write(PointZM point) => this.Write(point, includeMetadata: true, GetSizeAndType, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public override void Write(IEnumerable<Point> points) => this.Write([..points], WkbGeometryType.MultiPoint, includeMetadata: true, GetSizeAndType, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public override void Write(IEnumerable<PointZ> points) => this.Write([..points], WkbGeometryType.MultiPoint, includeMetadata: true, GetSizeAndType, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public override void Write(IEnumerable<PointM> points) => this.Write([..points], WkbGeometryType.MultiPoint, includeMetadata: true, GetSizeAndType, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public override void Write(IEnumerable<PointZM> points) => this.Write([..points], WkbGeometryType.MultiPoint, includeMetadata: true, GetSizeAndType, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public override void Write(Polyline<Point> polyline) => this.Write([..polyline], WkbGeometryType.LineString, includeMetadata: false, GetSizeAndType, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public override void Write(Polyline<PointZ> polyline) => this.Write([..polyline], WkbGeometryType.LineString, includeMetadata: false, GetSizeAndType, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public override void Write(Polyline<PointM> polyline) => this.Write([..polyline], WkbGeometryType.LineString, includeMetadata: false, GetSizeAndType, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public override void Write(Polyline<PointZM> polyline) => this.Write([..polyline], WkbGeometryType.LineString, includeMetadata: false, GetSizeAndType, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polyline<Point>> polylines) => this.Write([..polylines], GetSizeAndType, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polyline<PointZ>> polylines) => this.Write([..polylines], GetSizeAndType, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polyline<PointM>> polylines) => this.Write([..polylines], GetSizeAndType, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polyline<PointZM>> polylines) => this.Write([..polylines], GetSizeAndType, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public override void Write(Polygon<Point> polygon) => this.Write(polygon, GetSizeAndType, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public override void Write(Polygon<PointZ> polygon) => this.Write(polygon, GetSizeAndType, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public override void Write(Polygon<PointM> polygon) => this.Write(polygon, GetSizeAndType, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public override void Write(Polygon<PointZM> polygon) => this.Write(polygon, GetSizeAndType, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polygon<Point>> polygons) => this.Write([..polygons], GetSizeAndType, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polygon<PointZ>> polygons) => this.Write([..polygons], GetSizeAndType, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polygon<PointM>> polygons) => this.Write([..polygons], GetSizeAndType, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polygon<PointZM>> polygons) => this.Write([..polygons], GetSizeAndType, GetXY, GetZ, GetM);

    private static SizeAndType GetSizeAndType(Point point) => (16, false, false, (uint)WkbGeometryType.Point);

    private static SizeAndType GetSizeAndType(PointZ point) => (24, true, false, (uint)WkbGeometryType.PointZ);

    private static SizeAndType GetSizeAndType(PointM point) => (24, false, true, (uint)WkbGeometryType.PointM);

    private static SizeAndType GetSizeAndType(PointZM point) => (32, true, true, (uint)WkbGeometryType.PointZM);

    private static XY GetXY(Point point) => (point.X, point.Y);

    private static XY GetXY(PointZ point) => (point.X, point.Y);

    private static XY GetXY(PointM point) => (point.X, point.Y);

    private static XY GetXY(PointZM point) => (point.X, point.Y);

    private static double GetZ(Point point) => throw new InvalidGeometryTypeException();

    private static double GetZ(PointZ point) => point.Z;

    private static double GetZ(PointM point) => throw new InvalidGeometryTypeException();

    private static double GetZ(PointZM point) => point.Z;

    private static double GetM(Point point) => throw new InvalidGeometryTypeException();

    private static double GetM(PointZ point) => throw new InvalidGeometryTypeException();

    private static double GetM(PointM point) => point.Measurement;

    private static double GetM(PointZM point) => point.Measurement;

    private void Write<T>(ICollection<T> points, WkbGeometryType geometryType, bool includeMetadata, Func<T, SizeAndType> getSizeAndType, Func<T, XY> getXY, Func<T, double> getZ, Func<T, double> getM)
        where T : struct
    {
        var count = points.Count;

        if (count is not 0)
        {
            var (_, _, _, type) = getSizeAndType(points.First());
            geometryType = (geometryType - WkbGeometryType.Point) + (WkbGeometryType)type;
        }

        this.WriteHeader(count, geometryType);

        foreach (var point in points)
        {
            this.Write(point, includeMetadata, getSizeAndType, getXY, getZ, getM);
        }
    }

    private void Write<T>(ICollection<Polyline<T>> lines, Func<T, SizeAndType> getSizeAndType, Func<T, XY> getXY, Func<T, double> getZ, Func<T, double> getM)
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

        this.WriteHeader(count, geometryType);

        foreach (var line in lines)
        {
            this.Write([..line], WkbGeometryType.LineString, includeMetadata: false, getSizeAndType, getXY, getZ, getM);
        }
    }

    private void Write<T>(Polygon<T> polygon, Func<T, SizeAndType> getSizeAndType, Func<T, XY> getXY, Func<T, double> getZ, Func<T, double> getM)
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

        this.WriteHeader(count, geometryType);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        Span<byte> span = stackalloc byte[4];
#else
        var a = new byte[4];
        var span = a.AsSpan();
#endif
        foreach (var linearRing in polygon)
        {
            if (this.IsLittleEndian)
            {
                WriteUInt32LittleEndian(span, (uint)linearRing.Count);
            }
            else
            {
                WriteUInt32BigEndian(span, (uint)linearRing.Count);
            }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
            this.BaseStream.Write(span);
#else
            this.BaseStream.Write(a, 0, a.Length);
#endif

            foreach (var point in linearRing)
            {
                this.Write(point, includeMetadata: false, getSizeAndType, getXY, getZ, getM);
            }
        }
    }

    private void Write<T>(ICollection<Polygon<T>> polygons, Func<T, SizeAndType> getSizeAndType, Func<T, XY> getXY, Func<T, double> getZ, Func<T, double> getM)
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

        this.WriteHeader(count, geometryType);

        foreach (var polygon in polygons)
        {
            this.Write(polygon, getSizeAndType, getXY, getZ, getM);
        }
    }

    private void Write<T>(T point, bool includeMetadata, Func<T, SizeAndType> getSizeAndType, Func<T, XY> getXY, Func<T, double> getZ, Func<T, double> getM)
        where T : struct
    {
        // write the type
        var (size, hasZ, hasM, type) = getSizeAndType(point);
        var geometryType = (WkbGeometryType)type;

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        Span<byte> span = stackalloc byte[size];
#else
        var a = new byte[size];
        var span = a.AsSpan();
#endif

        if (includeMetadata)
        {
            // write the byte order
            span[0] = this.IsLittleEndian ? (byte)1 : (byte)0;
            if (this.IsLittleEndian)
            {
                WriteUInt32LittleEndian(span[1..5], (uint)geometryType);
            }
            else
            {
                WriteUInt32BigEndian(span[1..5], (uint)geometryType);
            }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
            this.BaseStream.Write(span[0..5]);
#else
            this.BaseStream.Write(a, 0, 5);
#endif
        }

        var idx = 0;
        var (x, y) = getXY(point);
        WriteValue(span[idx..], x, this.IsLittleEndian);
        idx += 8;
        WriteValue(span[idx..], y, this.IsLittleEndian);

        if (hasZ)
        {
            idx += 8;
            WriteValue(span[idx..], getZ(point), this.IsLittleEndian);
        }

        if (hasM)
        {
            idx += 8;
            WriteValue(span[idx..], getM(point), this.IsLittleEndian);
        }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        this.BaseStream.Write(span);
#else
        this.BaseStream.Write(a, 0, a.Length);
#endif

        static void WriteValue(Span<byte> span, double value, bool isLittleEndian)
        {
            if (isLittleEndian)
            {
                WriteInt64LittleEndian(span, BitConverter.DoubleToInt64Bits(value));
            }
            else
            {
                WriteInt64BigEndian(span, BitConverter.DoubleToInt64Bits(value));
            }
        }
    }

    private void WriteHeader(int count, WkbGeometryType geometryType)
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        Span<byte> span = stackalloc byte[9];
#else
        var a = new byte[9];
        var span = a.AsSpan();
#endif

        // write the byte order
        span[0] = this.IsLittleEndian ? (byte)1 : (byte)0;

        if (this.IsLittleEndian)
        {
            WriteUInt32LittleEndian(span[1..5], (uint)geometryType);
            WriteUInt32LittleEndian(span[5..9], (uint)count);
        }
        else
        {
            WriteUInt32BigEndian(span[1..5], (uint)geometryType);
            WriteUInt32BigEndian(span[5..9], (uint)count);
        }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        this.BaseStream.Write(span);
#else
        this.BaseStream.Write(a, 0, a.Length);
#endif
    }
}