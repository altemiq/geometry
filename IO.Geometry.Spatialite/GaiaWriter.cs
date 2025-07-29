// -----------------------------------------------------------------------
// <copyright file="GaiaWriter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Spatialite;

using Altemiq.Geometry;

/// <summary>
/// Represents an implementation of <see cref="Data.IGeometryWriter"/> the writes <c>GAIA</c> geometries.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Public API")]
public class GaiaWriter : Data.Common.BinaryGeometryWriter
{
    /// <summary>
    /// Initialises a new instance of the <see cref="GaiaWriter"/> class.
    /// </summary>
    /// <param name="bytes">The byte array to write to.</param>
    public GaiaWriter(byte[] bytes)
        : base(bytes)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="GaiaWriter"/> class.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    public GaiaWriter(Stream stream)
        : base(stream)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="GaiaWriter"/> class.
    /// </summary>
    /// <param name="bytes">The byte array to write to.</param>
    /// <param name="littleEndian">Set to <see langword="true"/> to treat the data as little endian.</param>
    public GaiaWriter(byte[] bytes, bool littleEndian)
        : base(bytes, littleEndian)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="GaiaWriter"/> class.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="littleEndian">Set to <see langword="true"/> to treat the data as little endian.</param>
    public GaiaWriter(Stream stream, bool littleEndian)
        : base(stream, littleEndian)
    {
    }

    private delegate (double MinX, double MinY, double MaxX, double MaxY) Writer<in T>(T value, bool includeMetadata);

    private delegate (double X, double T) GetXYFunc<in T>(T point);

    private delegate bool TryGetValue<in T>(T point, out double z);

    /// <inheritdoc/>
    public override void Write(Point point) => this.Write(point, 0);

    /// <inheritdoc cref="Write(Altemiq.Geometry.Point)" />
    public void Write(Point point, int srid) => this.Write(point, GetXY, TryGetZ, TryGetM, srid);

    /// <inheritdoc/>
    public override void Write(PointZ point) => this.Write(point, 0);

    /// <inheritdoc cref="Write(PointZ)" />
    public void Write(PointZ point, int srid) => this.Write(point, GetXY, TryGetZ, TryGetM, srid);

    /// <inheritdoc/>
    public override void Write(PointM point) => this.Write(point, 0);

    /// <inheritdoc cref="Write(PointM)" />
    public void Write(PointM point, int srid) => this.Write(point, GetXY, TryGetZ, TryGetM, srid);

    /// <inheritdoc/>
    public override void Write(PointZM point) => this.Write(point, 0);

    /// <inheritdoc cref="Write(PointZM)" />
    public void Write(PointZM point, int srid) => this.Write(point, GetXY, TryGetZ, TryGetM, srid);

    /// <inheritdoc/>
    public override void Write(IEnumerable<Point> points) => this.Write(points, 0);

    /// <inheritdoc cref="Write(IEnumerable{Point})" />
    public void Write(IEnumerable<Point> points, int srid) => this.Write(srid, () => this.Write(points, GaiaGeometryType.MultiPoint, GetPointType, this.Write));

    /// <inheritdoc/>
    public override void Write(IEnumerable<PointZ> points) => this.Write(points, 0);

    /// <inheritdoc cref="Write(IEnumerable{PointZ})" />
    public void Write(IEnumerable<PointZ> points, int srid) => this.Write(srid, () => this.Write(points, GaiaGeometryType.MultiPoint, GetPointType, this.Write));

    /// <inheritdoc/>
    public override void Write(IEnumerable<PointM> points) => this.Write(points, 0);

    /// <inheritdoc cref="Write(IEnumerable{PointM})" />
    public void Write(IEnumerable<PointM> points, int srid) => this.Write(srid, () => this.Write(points, GaiaGeometryType.MultiPoint, GetPointType, this.Write));

    /// <inheritdoc/>
    public override void Write(IEnumerable<PointZM> points) => this.Write(points, 0);

    /// <inheritdoc cref="Write(IEnumerable{PointZM})" />
    public void Write(IEnumerable<PointZM> points, int srid) => this.Write(srid, () => this.Write(points, GaiaGeometryType.MultiPoint, GetPointType, this.Write));

    /// <inheritdoc/>
    public override void Write(Polyline<Point> polyline) => this.Write(polyline, 0);

    /// <inheritdoc cref="Write(Polyline{Point})" />
    public void Write(Polyline<Point> polyline, int srid) => this.Write(srid, () => this.Write(polyline, includeMetadata: true, this.Write));

    /// <inheritdoc/>
    public override void Write(Polyline<PointZ> polyline) => this.Write(polyline, 0);

    /// <inheritdoc cref="Write(Polyline{PointZ})" />
    public void Write(Polyline<PointZ> polyline, int srid) => this.Write(srid, () => this.Write(polyline, includeMetadata: true, this.Write));

    /// <inheritdoc/>
    public override void Write(Polyline<PointM> polyline) => this.Write(polyline, 0);

    /// <inheritdoc cref="Write(Polyline{PointM})" />
    public void Write(Polyline<PointM> polyline, int srid) => this.Write(srid, () => this.Write(polyline, includeMetadata: true, this.Write));

    /// <inheritdoc/>
    public override void Write(Polyline<PointZM> polyline) => this.Write(polyline, 0);

    /// <inheritdoc cref="Write(Polyline{PointZM})" />
    public void Write(Polyline<PointZM> polyline, int srid) => this.Write(srid, () => this.Write(polyline, includeMetadata: true, this.Write));

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polyline<Point>> polylines) => this.Write(polylines, 0);

    /// <inheritdoc cref="Write(IEnumerable{Polyline{Point}})"/>
    public void Write(IEnumerable<Polyline<Point>> polylines, int srid) => this.Write(srid, () => this.Write(polylines, GaiaGeometryType.MultiLineString, GetPolylineType, this.Write));

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polyline<PointZ>> polylines) => this.Write(polylines, 0);

    /// <inheritdoc cref="Write(IEnumerable{Polyline{PointZ}})"/>
    public void Write(IEnumerable<Polyline<PointZ>> polylines, int srid) => this.Write(srid, () => this.Write(polylines, GaiaGeometryType.MultiLineString, GetPolylineType, this.Write));

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polyline<PointM>> polylines) => this.Write(polylines, 0);

    /// <inheritdoc cref="Write(IEnumerable{Polyline{PointM}})"/>
    public void Write(IEnumerable<Polyline<PointM>> polylines, int srid) => this.Write(srid, () => this.Write(polylines, GaiaGeometryType.MultiLineString, GetPolylineType, this.Write));

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polyline<PointZM>> polylines) => this.Write(polylines, 0);

    /// <inheritdoc cref="Write(IEnumerable{Polyline{PointZM}})"/>
    public void Write(IEnumerable<Polyline<PointZM>> polylines, int srid) => this.Write(srid, () => this.Write(polylines, GaiaGeometryType.MultiLineString, GetPolylineType, this.Write));

    /// <inheritdoc/>
    public override void Write(Polygon<Point> polygon) => this.Write(polygon, 0);

    /// <inheritdoc cref="Write(Polygon{Point})" />
    public void Write(Polygon<Point> polygon, int srid) => this.Write(srid, () => this.Write(polygon, includeMetadata: true, this.Write));

    /// <inheritdoc/>
    public override void Write(Polygon<PointZ> polygon) => this.Write(polygon, 0);

    /// <inheritdoc cref="Write(Polygon{PointZ})" />
    public void Write(Polygon<PointZ> polygon, int srid) => this.Write(srid, () => this.Write(polygon, includeMetadata: true, this.Write));

    /// <inheritdoc/>
    public override void Write(Polygon<PointM> polygon) => this.Write(polygon, 0);

    /// <inheritdoc cref="Write(Polygon{PointM})" />
    public void Write(Polygon<PointM> polygon, int srid) => this.Write(srid, () => this.Write(polygon, includeMetadata: true, this.Write));

    /// <inheritdoc/>
    public override void Write(Polygon<PointZM> polygon) => this.Write(polygon, 0);

    /// <inheritdoc cref="Write(Polygon{PointZM})" />
    public void Write(Polygon<PointZM> polygon, int srid) => this.Write(srid, () => this.Write(polygon, includeMetadata: true, this.Write));

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polygon<Point>> polygons) => this.Write(polygons, 0);

    /// <inheritdoc cref="Write(IEnumerable{Polygon{Point}})"/>
    public void Write(IEnumerable<Polygon<Point>> polygons, int srid) => this.Write(srid, () => this.Write(polygons, GaiaGeometryType.MultiPolygon, GetPolygonType, this.Write));

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polygon<PointZ>> polygons) => this.Write(polygons, 0);

    /// <inheritdoc cref="Write(IEnumerable{Polygon{PointZ}})"/>
    public void Write(IEnumerable<Polygon<PointZ>> polygons, int srid) => this.Write(srid, () => this.Write(polygons, GaiaGeometryType.MultiPolygon, GetPolygonType, this.Write));

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polygon<PointM>> polygons) => this.Write(polygons, 0);

    /// <inheritdoc cref="Write(IEnumerable{Polygon{PointM}})"/>
    public void Write(IEnumerable<Polygon<PointM>> polygons, int srid) => this.Write(srid, () => this.Write(polygons, GaiaGeometryType.MultiPolygon, GetPolygonType, this.Write));

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polygon<PointZM>> polygons) => this.Write(polygons, 0);

    /// <inheritdoc cref="Write(IEnumerable{Polygon{PointZM}})"/>
    public void Write(IEnumerable<Polygon<PointZM>> polygons, int srid) => this.Write(srid, () => this.Write(polygons, GaiaGeometryType.MultiPolygon, GetPolygonType, this.Write));

    private static GaiaGeometryType GetPointType<T>(T point) => point switch
    {
        PointZM => GaiaGeometryType.PointZM,
        PointM => GaiaGeometryType.PointM,
        PointZ => GaiaGeometryType.PointZ,
        Point => GaiaGeometryType.Point,
        _ => throw new InvalidGeometryTypeException(),
    };

    private static GaiaGeometryType GetPointType<T>(T point, GaiaGeometryType geometryType) => GetPointType(point) + (geometryType - GaiaGeometryType.Point);

    private static GaiaGeometryType GetPolylineType<T>(Polyline<T> line, GaiaGeometryType geometryType) => GetPointType(line[0], geometryType);

    private static GaiaGeometryType GetLinearRingType<T>(LinearRing<T> line, GaiaGeometryType geometryType)
        where T : struct => GetPointType(line[0], geometryType);

    private static GaiaGeometryType GetPolygonType<T>(Polygon<T> polygon, GaiaGeometryType geometryType)
        where T : struct => GetLinearRingType(polygon[0], geometryType);

    private static (double X, double Y) GetXY(Point point) => (point.X, point.Y);

    private static (double X, double Y) GetXY(PointZ point) => (point.X, point.Y);

    private static (double X, double Y) GetXY(PointM point) => (point.X, point.Y);

    private static (double X, double Y) GetXY(PointZM point) => (point.X, point.Y);

    private static bool TryGetZ(Point point, out double z)
    {
        z = default;
        return false;
    }

    private static bool TryGetZ(PointZ point, out double z)
    {
        z = point.Z;
        return true;
    }

    private static bool TryGetZ(PointM point, out double z)
    {
        z = default;
        return false;
    }

    private static bool TryGetZ(PointZM point, out double z)
    {
        z = point.Z;
        return true;
    }

    private static bool TryGetM(Point point, out double m)
    {
        m = default;
        return false;
    }

    private static bool TryGetM(PointZ point, out double m)
    {
        m = default;
        return false;
    }

    private static bool TryGetM(PointM point, out double m)
    {
        m = point.Measurement;
        return true;
    }

    private static bool TryGetM(PointZM point, out double m)
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

    private (double MinX, double MinY, double MaxX, double MaxY) Write(Point point, bool includeMetadata) => this.Write(point, includeMetadata, GetXY, TryGetZ, TryGetM);

    private (double MinX, double MinY, double MaxX, double MaxY) Write(PointZ point, bool includeMetadata) => this.Write(point, includeMetadata, GetXY, TryGetZ, TryGetM);

    private (double MinX, double MinY, double MaxX, double MaxY) Write(PointM point, bool includeMetadata) => this.Write(point, includeMetadata, GetXY, TryGetZ, TryGetM);

    private (double MinX, double MinY, double MaxX, double MaxY) Write(PointZM point, bool includeMetadata) => this.Write(point, includeMetadata, GetXY, TryGetZ, TryGetM);

    private (double MinX, double MinY, double MaxX, double MaxY) Write(Polyline<Point> ring, bool includeMetadata) => this.Write(ring, GaiaGeometryType.LineString, includeMetadata, GetPointType, this.Write);

    private (double MinX, double MinY, double MaxX, double MaxY) Write(Polyline<PointZ> ring, bool includeMetadata) => this.Write(ring, GaiaGeometryType.LineString, includeMetadata, GetPointType, this.Write);

    private (double MinX, double MinY, double MaxX, double MaxY) Write(Polyline<PointM> ring, bool includeMetadata) => this.Write(ring, GaiaGeometryType.LineString, includeMetadata, GetPointType, this.Write);

    private (double MinX, double MinY, double MaxX, double MaxY) Write(Polyline<PointZM> ring, bool includeMetadata) => this.Write(ring, GaiaGeometryType.LineString, includeMetadata, GetPointType, this.Write);

    private (double MinX, double MinY, double MaxX, double MaxY) Write(Polygon<Point> polygon, bool includeMetadata) => this.Write(polygon, GaiaGeometryType.Polygon, includeMetadata, GetLinearRingType, this.Write);

    private (double MinX, double MinY, double MaxX, double MaxY) Write(Polygon<PointZ> polygon, bool includeMetadata) => this.Write(polygon, GaiaGeometryType.Polygon, includeMetadata, GetLinearRingType, this.Write);

    private (double MinX, double MinY, double MaxX, double MaxY) Write(Polygon<PointM> polygon, bool includeMetadata) => this.Write(polygon, GaiaGeometryType.Polygon, includeMetadata, GetLinearRingType, this.Write);

    private (double MinX, double MinY, double MaxX, double MaxY) Write(Polygon<PointZM> ring, bool includeMetadata) => this.Write(ring, GaiaGeometryType.LineString, includeMetadata, GetLinearRingType, this.Write);

    private (double MinX, double MinY, double MaxX, double MaxY) Write(LinearRing<Point> ring, bool includeMetadata) => this.Write(ring, GaiaGeometryType.LineString, includeMetadata, GetPointType, this.Write);

    private (double MinX, double MinY, double MaxX, double MaxY) Write(LinearRing<PointZ> ring, bool includeMetadata) => this.Write(ring, GaiaGeometryType.LineString, includeMetadata, GetPointType, this.Write);

    private (double MinX, double MinY, double MaxX, double MaxY) Write(LinearRing<PointM> ring, bool includeMetadata) => this.Write(ring, GaiaGeometryType.LineString, includeMetadata, GetPointType, this.Write);

    private (double MinX, double MinY, double MaxX, double MaxY) Write(LinearRing<PointZM> ring, bool includeMetadata) => this.Write(ring, GaiaGeometryType.LineString, includeMetadata, GetPointType, this.Write);

    private void Write(int srid, Func<(double MinX, double MinY, double MaxX, double MaxY)> function)
    {
        this.WriteHeader(srid);
        WriteEnvelope(function());
        this.WriteFooter();

        void WriteEnvelope((double MinX, double MinY, double MaxX, double MaxY) envelope)
        {
            if (envelope.MinX is double.MaxValue)
            {
                return;
            }

            var position = this.BaseStream.Position;
            if (this.BaseStream.Position is not 6 && !this.BaseStream.CanSeek)
            {
                throw new InvalidOperationException();
            }

            this.BaseStream.Position = 6;
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
            Span<byte> span = stackalloc byte[32];
#else
            var a = new byte[32];
            var span = a.AsSpan();
#endif

            WriteDouble(span[0..8], this.IsLittleEndian, envelope.MinX);
            WriteDouble(span[8..16], this.IsLittleEndian, envelope.MinY);
            WriteDouble(span[16..24], this.IsLittleEndian, envelope.MaxX);
            WriteDouble(span[24..32], this.IsLittleEndian, envelope.MaxY);
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
            this.BaseStream.Write(span);
#else
            this.BaseStream.Write(a, 0, a.Length);
#endif

            this.BaseStream.Position = position;
        }
    }

    private void Write<T>(T point, GetXYFunc<T> getXY, TryGetValue<T> getZ, TryGetValue<T> getM, int srid)
    {
        this.WriteHeader(srid, point, point, getXY);
        _ = this.Write(point, includeMetadata: true, getXY, getZ, getM);
        this.WriteFooter();
    }

    private (double MinX, double MinY, double MaxX, double MaxY) Write<T>(T point, bool includeMetadata, GetXYFunc<T> getXYFunc, TryGetValue<T> getZ, TryGetValue<T> getM)
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

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
        Span<byte> span = stackalloc byte[sizeof(double) * components.Count];
#else
        var a = new byte[sizeof(double) * components.Count];
        var span = a.AsSpan();
#endif

        if (includeMetadata)
        {
            WriteInt32(span, this.IsLittleEndian, (int)geometryType);
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
            this.BaseStream.Write(span[0..4]);
#else
            this.BaseStream.Write(a, 0, 4);
#endif
        }

        for (var i = 0; i < components.Count; i++)
        {
            var idx = i * sizeof(double);
            WriteDouble(span[idx..], this.IsLittleEndian, components[i]);
        }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
        this.BaseStream.Write(span);
#else
        this.BaseStream.Write(a, 0, a.Length);
#endif

        return (x, y, x, y);
    }

    private (double MinX, double MinY, double MaxX, double MaxY) Write<T>(Polyline<T> points, bool includeMetadata, Writer<T> writer) => this.Write(points, GaiaGeometryType.LineString, includeMetadata, GetPointType, writer);

    private (double MinX, double MinY, double MaxX, double MaxY) Write<T>(IEnumerable<T> points, GaiaGeometryType geometryType, bool includeMetadata, Func<T, GaiaGeometryType, GaiaGeometryType> getGeometryType, Writer<T> writer)
    {
        var typePosition = includeMetadata ? this.BaseStream.Position : -1L;
        var haveType = false;
        var countPosition = includeMetadata ? typePosition + sizeof(GaiaGeometryType) : this.BaseStream.Position;
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
        Span<byte> span = stackalloc byte[4];
#else
        var a = new byte[4];
        var span = a.AsSpan();
#endif
        if (points is IList<T> list)
        {
            if (includeMetadata)
            {
                // write the type
                geometryType = getGeometryType(list[0], geometryType);
                WriteInt32(span, this.IsLittleEndian, (int)geometryType);
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
                this.BaseStream.Write(span);
#else
                this.BaseStream.Write(a, 0, a.Length);
#endif
                haveType = true;
                typePosition = -1;
            }

            WriteInt32(span, this.IsLittleEndian, list.Count);
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
            this.BaseStream.Write(span);
#else
            this.BaseStream.Write(a, 0, a.Length);
#endif
            countPosition = -1;
        }

        var count = 0;
        var min = (X: double.MaxValue, Y: double.MaxValue);
        var max = (X: double.MinValue, Y: double.MinValue);
        foreach (var point in points)
        {
            if (includeMetadata && !haveType)
            {
                geometryType = GetPointType(point) + (geometryType - GaiaGeometryType.Point);
                haveType = true;
            }

            var (minX, minY, maxX, maxY) = writer(point, false);
            min = Min(min, (minX, minY));
            max = Max(max, (maxX, maxY));
            count++;
        }

        var currentPosition = this.BaseStream.Position;
        if (typePosition >= 0)
        {
            if (!this.BaseStream.CanSeek)
            {
                throw new InvalidOperationException();
            }

            this.BaseStream.Position = typePosition;

            WriteInt32(span, this.IsLittleEndian, (int)geometryType);
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
            this.BaseStream.Write(span);
#else
            this.BaseStream.Write(a, 0, a.Length);
#endif
        }

        if (countPosition >= 0)
        {
            if (!this.BaseStream.CanSeek)
            {
                throw new InvalidOperationException();
            }

            this.BaseStream.Position = countPosition;
            WriteInt32(span, this.IsLittleEndian, count);
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
            this.BaseStream.Write(span);
#else
            this.BaseStream.Write(a, 0, a.Length);
#endif
        }

        if (this.BaseStream.CanSeek)
        {
            this.BaseStream.Position = currentPosition;
        }

        return min.X is double.MaxValue ? default : (MinX: min.X, MinY: min.Y, MaxX: max.X, MaxY: max.Y);
    }

    private (double MinX, double MinY, double MaxX, double MaxY) Write<T>(Polygon<T> polygon, bool includeMetadata, Writer<T> writer)
        where T : struct
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
        Span<byte> span = stackalloc byte[4];
#else
        var a = new byte[4];
        var span = a.AsSpan();
#endif
        if (includeMetadata)
        {
            var point = polygon.Points.Count is 0
                ? default
                : polygon.Points[0];
            var geometryType = GetPointType(point) + (GaiaGeometryType.Polygon - GaiaGeometryType.Point);
            WriteInt32(span, this.IsLittleEndian, (int)geometryType);
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
            this.BaseStream.Write(span);
#else
            this.BaseStream.Write(a, 0, a.Length);
#endif
        }

        WriteInt32(span, this.IsLittleEndian, polygon.Count);
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
        this.BaseStream.Write(span);
#else
        this.BaseStream.Write(a, 0, a.Length);
#endif
        var envelope = (MinX: double.MaxValue, MinY: double.MaxValue, MaxX: double.MinValue, MaxY: double.MinValue);
        foreach (var ring in polygon)
        {
            envelope = Envelope(envelope, this.Write(ring, GaiaGeometryType.LineString,  includeMetadata: false, GetPointType, writer));
        }

        return envelope;
    }

    private (double MinX, double MinY, double MaxX, double MaxY) Write<T>(IEnumerable<T> values, GaiaGeometryType geometryType, Func<T, GaiaGeometryType, GaiaGeometryType> getType, Writer<T> writer)
    {
        var typePosition = this.BaseStream.Position;
        var haveType = false;
        var countPosition = typePosition + sizeof(GaiaGeometryType);
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
        Span<byte> span = stackalloc byte[8];
#else
        var a = new byte[8];
        var span = a.AsSpan();
#endif
        if (values is IList<T> list)
        {
            // write the type
            geometryType = getType(list[0], geometryType);

            WriteInt32(span, this.IsLittleEndian, (int)geometryType);
            typePosition = -1;
            haveType = true;

            WriteInt32(span[4..], this.IsLittleEndian, list.Count);
            countPosition = -1;

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
            this.BaseStream.Write(span);
#else
            this.BaseStream.Write(a, 0, a.Length);
#endif
        }

        // write the element out
        var count = 0;
        var envelope = (MinX: double.MaxValue, MinY: double.MaxValue, MaxX: double.MinValue, MaxY: double.MinValue);
        foreach (var value in values)
        {
            if (!haveType)
            {
                geometryType = getType(value, geometryType);
                haveType = true;
            }

            this.BaseStream.WriteByte(GaiaConstants.BlobMark.Entity);
            envelope = Envelope(envelope, writer(value, includeMetadata: true));
            count++;
        }

        var currentPosition = this.BaseStream.Position;
        if (typePosition >= 0)
        {
            if (this.BaseStream.CanSeek)
            {
                throw new InvalidOperationException();
            }

            this.BaseStream.Position = typePosition;
            WriteInt32(span, this.IsLittleEndian, (int)geometryType);
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
            this.BaseStream.Write(span[0..4]);
#else
            this.BaseStream.Write(a, 0, 4);
#endif
        }

        if (countPosition >= 0)
        {
            if (this.BaseStream.CanSeek)
            {
                throw new InvalidOperationException();
            }

            this.BaseStream.Position = countPosition;
            WriteInt32(span, this.IsLittleEndian, count);
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
            this.BaseStream.Write(span[0..4]);
#else
            this.BaseStream.Write(a, 0, 4);
#endif
        }

        if (this.BaseStream.CanSeek)
        {
            this.BaseStream.Position = currentPosition;
        }

        return envelope;
    }

    private void WriteHeader<T>(int srid, T min, T max, GetXYFunc<T> getXY)
    {
        var (minX, minY) = getXY(min);
        var (maxX, maxY) = getXY(max);
        this.WriteHeader(srid, minX, minY, maxX, maxY);
    }

    private void WriteHeader(int srid) => this.WriteHeader(srid, 0D, 0D, 0D, 0D);

    private void WriteHeader(int srid, double minX, double minY, double maxX, double maxY)
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
        Span<byte> span = stackalloc byte[39];
#else
        var a = new byte[39];
        var span = a.AsSpan();
#endif

        span[0] = GaiaConstants.BlobMark.Start;
        span[1] = this.IsLittleEndian ? (byte)1 : (byte)0;

        // SRID
        WriteInt32(span[2..6], this.IsLittleEndian, srid);

        // write the envelope
        WriteDouble(span[6..14], this.IsLittleEndian, minX);
        WriteDouble(span[14..22], this.IsLittleEndian, minY);
        WriteDouble(span[22..30], this.IsLittleEndian, maxX);
        WriteDouble(span[30..38], this.IsLittleEndian, maxY);

        span[38] = GaiaConstants.BlobMark.Mbr;

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
        this.BaseStream.Write(span);
#else
        this.BaseStream.Write(a, 0, a.Length);
#endif
    }

    private void WriteFooter() => this.BaseStream.WriteByte(GaiaConstants.BlobMark.End);
}