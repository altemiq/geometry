// -----------------------------------------------------------------------
// <copyright file="ShpWriter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

using DimensionsAndType = System.ValueTuple<int, bool, bool, Altemiq.IO.Geometry.Shapefile.ShpType>;
using XY = System.ValueTuple<double, double>;

/// <summary>
/// The SHP writer.
/// </summary>
public class ShpWriter : Data.IGeometryWriter, IDisposable
{
    private readonly bool leaveOpen;

    private bool disposedValue;

    private int recordNumber;

    private long start;

    private ShpType shpType;

    /// <summary>
    /// Initialises a new instance of the <see cref="ShpWriter"/> class.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="ShpWriter"/> object is disposed; otherwise, <see langword="false"/>.</param>
    public ShpWriter(Stream stream, bool leaveOpen = false)
    {
        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        System.Diagnostics.Contracts.Contract.EndContractBlock();
        (this.BaseStream, this.leaveOpen) = (stream, leaveOpen);
    }

    /// <summary>
    /// Gets the underlying stream of the <see cref="ShpWriter"/>.
    /// </summary>
    internal Stream BaseStream { get; }

    /// <summary>
    /// Updates the header.
    /// </summary>
    /// <param name="extents">The extents.</param>
    public void Update(EnvelopeZM extents) => Update(this.BaseStream, this.start, extents);

    /// <summary>
    /// Writes the header.
    /// </summary>
    /// <param name="header">The header.</param>
    public void Write(Header header)
    {
        this.start = this.BaseStream.Position;
        header.CopyTo(this.BaseStream);
        this.shpType = header.ShpType;
        this.recordNumber = 0;
    }

    /// <summary>
    /// Writes the geometry.
    /// </summary>
    /// <param name="geometry">The geometry.</param>
    public void Write(object? geometry)
    {
        switch (geometry)
        {
            case null:
                this.Write<object?>(value: default, static (stream, _) =>
                {
                    WriteNull(stream);
                    return 0;
                });

                break;
            case Point point:
                this.Write(point);
                break;
            case PointZ point:
                this.Write(point);
                break;
            case PointM point:
                this.Write(point);
                break;
            case PointZM point:
                this.Write(point);
                break;
            case Polyline polyline:
                this.WritePolyLines([polyline]);
                break;
            case PolylineZ polyline:
                this.WritePolyLines([polyline]);
                break;
            case PolylineM polyline:
                this.WritePolyLines([polyline]);
                break;
            case PolylineZM polyline:
                this.WritePolyLines([polyline]);
                break;
            case IEnumerable<Polyline> polylines:
                this.WritePolyLines(polylines);
                break;
            case IEnumerable<PolylineZ> polylines:
                this.WritePolyLines(polylines);
                break;
            case IEnumerable<PolylineM> polylines:
                this.WritePolyLines(polylines);
                break;
            case IEnumerable<PolylineZM> polylines:
                this.WritePolyLines(polylines);
                break;
            case Polygon polygon:
                this.Write(polygon);
                break;
            case PolygonZ polygon when this.shpType == ShpType.MultiPatch:
                this.WriteMultiPatch([polygon]);
                break;
            case PolygonZ polygon:
                this.WritePolygons([polygon]);
                break;
            case PolygonM polygon:
                this.WritePolygons([polygon]);
                break;
            case PolygonZM polygon:
                this.WritePolygons([polygon]);
                break;
            case IEnumerable<Polygon> polygons:
                this.WritePolygons(polygons);
                break;
            case IEnumerable<PolygonZ> polygons when this.shpType is ShpType.MultiPatch:
                this.WriteMultiPatch(polygons);
                break;
            case IEnumerable<PolygonZ> polygons:
                this.WritePolygons(polygons);
                break;
            case IEnumerable<PolygonM> polygons:
                this.WritePolygons(polygons);
                break;
            case IEnumerable<PolygonZM> polygons:
                this.WritePolygons(polygons);
                break;
            case IEnumerable<Point> points:
                this.Write(points);
                break;
            case IEnumerable<PointZ> points:
                this.Write(points);
                break;
            case IEnumerable<PointM> points:
                this.Write(points);
                break;
            case IEnumerable<PointZM> points:
                this.Write(points);
                break;
        }
    }

    /// <inheritdoc/>
    public void Write(Point point) =>
        this.Write(point, static (stream, point) =>
        {
            Span<byte> span = stackalloc byte[20];
            System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(span, (int)ShpType.Point);
            System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[4..], BitConverter.DoubleToInt64Bits(point.X));
            System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[12..], BitConverter.DoubleToInt64Bits(point.Y));
            stream.Write(span);
            return 1;
        });

    /// <inheritdoc/>
    public void Write(PointZ point) =>
        this.Write(point, static (stream, point) =>
        {
            Span<byte> span = stackalloc byte[20];
            System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(span, (int)ShpType.Point);
            System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[4..], BitConverter.DoubleToInt64Bits(point.X));
            System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[12..], BitConverter.DoubleToInt64Bits(point.Y));
            stream.Write(span);
            return 1;
        });

    /// <inheritdoc/>
    public void Write(PointM point) =>
        this.Write(point, static (stream, point) =>
        {
            Span<byte> span = stackalloc byte[36];
            System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(span, (int)ShpType.PointM);
            System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[4..], BitConverter.DoubleToInt64Bits(point.X));
            System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[12..], BitConverter.DoubleToInt64Bits(point.Y));
            BinaryPrimitives.WriteDoubleLittleEndianIfNotNan(span[20..], Constants.NoData);
            BinaryPrimitives.WriteDoubleLittleEndianIfNotNan(span[28..], BitConverter.DoubleToInt64Bits(point.Measurement));
            stream.Write(span);
            return 1;
        });

    /// <inheritdoc/>
    public void Write(PointZM point) =>
        this.Write(point, static (stream, point) =>
        {
            Span<byte> span = stackalloc byte[36];
            System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(span, (int)ShpType.PointM);
            System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[4..], BitConverter.DoubleToInt64Bits(point.X));
            System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[12..], BitConverter.DoubleToInt64Bits(point.Y));
            BinaryPrimitives.WriteDoubleLittleEndianIfNotNan(span[20..], BitConverter.DoubleToInt64Bits(point.Z));
            BinaryPrimitives.WriteDoubleLittleEndianIfNotNan(span[28..], BitConverter.DoubleToInt64Bits(point.Measurement));
            stream.Write(span);
            return 1;
        });

    /// <inheritdoc/>
    public void Write(IEnumerable<Point> points) => this.Write(points, (s, p) => Write(s, p, GetDimensionsAndType, GetXY, GetZ, GetM));

    /// <inheritdoc/>
    public void Write(IEnumerable<PointZ> points) => this.Write(points, (s, p) => Write(s, p, GetDimensionsAndType, GetXY, GetZ, GetM));

    /// <inheritdoc/>
    public void Write(IEnumerable<PointM> points) => this.Write(points, (s, p) => Write(s, p, GetDimensionsAndType, GetXY, GetZ, GetM));

    /// <inheritdoc/>
    public void Write(IEnumerable<PointZM> points) => this.Write(points, (s, p) => Write(s, p, GetDimensionsAndType, GetXY, GetZ, GetM));

    /// <inheritdoc/>
    public void Write(Polyline<Point> polyline) => this.WritePolyLines([polyline]);

    /// <inheritdoc/>
    public void Write(Polyline<PointZ> polyline) => this.WritePolyLines([polyline]);

    /// <inheritdoc/>
    public void Write(Polyline<PointM> polyline) => this.WritePolyLines([polyline]);

    /// <inheritdoc/>
    public void Write(Polyline<PointZM> polyline) => this.WritePolyLines([polyline]);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polyline<Point>> polylines) => this.WritePolyLines(polylines);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polyline<PointZ>> polylines) => this.WritePolyLines(polylines);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polyline<PointM>> polylines) => this.WritePolyLines(polylines);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polyline<PointZM>> polylines) => this.WritePolyLines(polylines);

    /// <inheritdoc/>
    public void Write(Polygon<Point> polygon) => this.WritePolygons([polygon]);

    /// <inheritdoc/>
    public void Write(Polygon<PointZ> polygon) => this.WritePolygons([polygon]);

    /// <inheritdoc/>
    public void Write(Polygon<PointM> polygon) => this.WritePolygons([polygon]);

    /// <inheritdoc/>
    public void Write(Polygon<PointZM> polygon) => this.WritePolygons([polygon]);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polygon<Point>> polygons) => this.WritePolygons(polygons);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polygon<PointZ>> polygons) => this.WritePolygons(polygons);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polygon<PointM>> polygons) => this.WritePolygons(polygons);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polygon<PointZM>> polygons) => this.WritePolygons(polygons);

    /// <inheritdoc/>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Updates the stream with the extents.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="start">The start of the stream.</param>
    /// <param name="extents">The extents to write.</param>
    internal static void Update(Stream stream, long start, EnvelopeZM extents)
    {
        var current = stream.Position;
        var length = (uint)(current - start);
        Span<byte> span = stackalloc byte[64];

        stream.Position = start + 24;

        System.Buffers.Binary.BinaryPrimitives.WriteUInt32BigEndian(span, length / 2U);
        stream.Write(span[..4]);

        stream.Position += 2 * sizeof(int);
        System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[..8], BitConverter.DoubleToInt64Bits(extents.Left));
        System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[8..16], BitConverter.DoubleToInt64Bits(extents.Bottom));
        System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[16..24], BitConverter.DoubleToInt64Bits(extents.Right));
        System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[24..32], BitConverter.DoubleToInt64Bits(extents.Top));
        BinaryPrimitives.WriteDoubleLittleEndianIfNotNan(span[32..40], extents.Front, 0D);
        BinaryPrimitives.WriteDoubleLittleEndianIfNotNan(span[40..48], extents.Back, 0D);
        BinaryPrimitives.WriteDoubleLittleEndianIfNotNan(span[48..56], extents.Measurement, 0D);
        BinaryPrimitives.WriteDoubleLittleEndianIfNotNan(span[56..64], extents.Measurement + extents.Length, 0D);

        stream.Write(span);
        stream.Position = current;
    }

    /// <summary>
    /// Disposes the resources for this instance.
    /// </summary>
    /// <param name="disposing">Set to <see langword="true"/> to dispose of managed resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing && !this.leaveOpen)
            {
                this.BaseStream.Dispose();
            }

            this.disposedValue = true;
        }
    }

    private static int Write<T>(Stream stream, IEnumerable<T>? points, Func<T, DimensionsAndType> func, Func<T, XY> getXy, Func<T, double> getZ, Func<T, double> getM)
    {
        if (points is null)
        {
            WriteNull(stream);
            return 0;
        }

        var (count, bounds, boundsOffset, type) = WritePoints(stream, [.. points], func, getXy, getZ, getM);

        if (boundsOffset != default)
        {
            Span<byte> span = stackalloc byte[36];
            var currentPosition = stream.Position;
            stream.Position = boundsOffset;
            System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[..8], BitConverter.DoubleToInt64Bits(bounds.Left));
            System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[8..16], BitConverter.DoubleToInt64Bits(bounds.Bottom));
            System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[16..24], BitConverter.DoubleToInt64Bits(bounds.Right));
            System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[24..32], BitConverter.DoubleToInt64Bits(bounds.Top));
            System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(span[32..36], count);
            stream.Write(span);
            stream.Position = currentPosition;
        }

        if (type is ShpType.MultiPointZ)
        {
            Span<byte> span = stackalloc byte[16];
            System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[..8], BitConverter.DoubleToInt64Bits(bounds.Front));
            System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[8..16], BitConverter.DoubleToInt64Bits(bounds.Back));
            stream.Write(span);
        }

        if (type is ShpType.MultiPointM or ShpType.MultiPointZ)
        {
            Span<byte> span = stackalloc byte[16];
            System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[..8], BitConverter.DoubleToInt64Bits(bounds.Measurement));
            System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[8..16], BitConverter.DoubleToInt64Bits(bounds.Measurement + bounds.Length));
            stream.Write(span);
        }

        return count;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2589:Boolean expressions should not be gratuitous", Justification = "False positive")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Bug", "S2583:Conditionally executed code should be reachable", Justification = "False positive")]
        static (int Count, EnvelopeZM Bounds, long Offset, ShpType Type) WritePoints(
            Stream stream,
            ICollection<T> points,
            Func<T, DimensionsAndType> getPoint,
            Func<T, XY> getXY,
            Func<T, double> getZ,
            Func<T, double> getM)
        {
            var hasZ = false;
            var hasM = false;
            var type = ShpType.NullShape;
            var dimensions = 0;
            var count = 0;
            var minimumX = double.MaxValue;
            var minimumY = double.MaxValue;
            var minimumZ = double.MaxValue;
            var minimumM = double.MaxValue;
            var maximumX = double.MinValue;
            var maximumY = double.MinValue;
            var maximumZ = double.MinValue;
            var maximumM = double.MinValue;
            var boundsOffset = 0L;
            Span<byte> span = stackalloc byte[16];
            foreach (var point in points)
            {
                if (dimensions is 0)
                {
                    (dimensions, hasZ, hasM, type) = getPoint(point);

                    type = TypeCallback(type);
                }

                var (x, y) = getXY(point);
                System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span, BitConverter.DoubleToInt64Bits(x));
                System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[8..], BitConverter.DoubleToInt64Bits(y));
                stream.Write(span);

                minimumX = Math.Min(minimumX, x);
                minimumY = Math.Min(minimumY, y);
                maximumX = Math.Max(maximumX, x);
                maximumY = Math.Max(maximumY, y);

                count++;
            }

            // no points, so set the callback to be null shape.
            if (count is 0)
            {
                type = TypeCallback(type);
                return (0, EnvelopeZM.Empty, boundsOffset, type);
            }

            if (hasZ)
            {
                System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span, 0);
                System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[8..], 0);
                stream.Write(span);

                foreach (var z in points
                             .Select(getZ))
                {
                    System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span, BitConverter.DoubleToInt64Bits(z));
                    stream.Write(span[..4]);

                    if (double.IsNaN(z) || z < Constants.NoDataLimit)
                    {
                        continue;
                    }

                    minimumZ = Math.Min(minimumZ, z);
                    maximumZ = Math.Max(maximumZ, z);
                }
            }

            if (hasM)
            {
                System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span, 0);
                System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[8..], 0);
                stream.Write(span);

                foreach (var m in points
                             .Select(getM))
                {
                    System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span, BitConverter.DoubleToInt64Bits(m));
                    stream.Write(span[..4]);

                    if (double.IsNaN(m) || m < Constants.NoDataLimit)
                    {
                        continue;
                    }

                    minimumM = Math.Min(minimumM, m);
                    maximumM = Math.Max(maximumM, m);
                }
            }

            if (minimumZ is double.MaxValue)
            {
                minimumZ = 0D;
            }

            if (minimumM is double.MaxValue)
            {
                minimumM = 0D;
            }

            if (maximumZ is double.MinValue)
            {
                maximumZ = 0D;
            }

            if (maximumM is double.MinValue)
            {
                maximumM = 0D;
            }

            return (
                Count: count,
                Bounds: new(minimumX, minimumY, minimumZ, minimumM, maximumX, maximumY, maximumZ, maximumM),
                Offset: boundsOffset,
                Type: type);

            ShpType TypeCallback(ShpType type)
            {
                if (type is ShpType.NullShape)
                {
                    WriteNull(stream);
                    return type;
                }

                type += 7;

                Span<byte> span = stackalloc byte[36];
                System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(span, (int)type);
                stream.Write(span[..4]);

                boundsOffset = stream.Position;
                System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span, BitConverter.DoubleToInt64Bits(Constants.NoData));
                System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[8..], BitConverter.DoubleToInt64Bits(Constants.NoData));
                System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[16..], BitConverter.DoubleToInt64Bits(Constants.NoData));
                System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[24..], BitConverter.DoubleToInt64Bits(Constants.NoData));
                System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(span[32..], 0);
                stream.Write(span);
                return type;
            }
        }
    }

    private static int WriteImpl<TPoint>(
        Stream stream,
        IEnumerable<IEnumerable<TPoint>>? multiPart,
        Func<ShpType, ShpType> typeAdjustment,
        Func<TPoint, DimensionsAndType> getPoint,
        Func<TPoint, XY> getXY,
        Func<TPoint, double> getZ,
        Func<TPoint, double> getM)
        where TPoint : struct
    {
        if (multiPart is null)
        {
            WriteNull(stream);
            return 0;
        }

        const ShpPartType NullPartType = (ShpPartType)(-1);
        var hasZ = false;
        var hasM = false;
        var type = ShpType.NullShape;
        var dimensions = 0;
        var count = 0;
        var minimumX = double.MaxValue;
        var minimumY = double.MaxValue;
        var minimumZ = double.MaxValue;
        var minimumM = double.MaxValue;
        var maximumX = double.MinValue;
        var maximumY = double.MinValue;
        var maximumZ = double.MinValue;
        var maximumM = double.MinValue;

        using var pointStream = new MemoryStream();
        using var heightStream = new MemoryStream();
        using var measurementStream = new MemoryStream();
        var indexes = new List<int>();
        var parts = new List<ShpPartType>();

        Span<byte> span = stackalloc byte[16];
        foreach (var part in multiPart)
        {
            indexes.Add(count);
            if (part is ShpLinearRing<TPoint> linearRing)
            {
                // increase this to make sure we're up to the name number
                while (parts.Count < indexes.Count)
                {
                    parts.Add(NullPartType);
                }

                parts[indexes.Count - 1] = linearRing.PartType;
            }

            foreach (var point in part)
            {
                if (dimensions is 0)
                {
                    (dimensions, hasZ, hasM, type) = getPoint(point);

                    type = typeAdjustment(type);
                }

                var (x, y) = getXY(point);
                System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span, BitConverter.DoubleToInt64Bits(x));
                System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[8..], BitConverter.DoubleToInt64Bits(y));
                pointStream.Write(span);

                minimumX = Math.Min(minimumX, x);
                minimumY = Math.Min(minimumY, y);
                maximumX = Math.Max(maximumX, x);
                maximumY = Math.Max(maximumY, y);

                if (hasZ)
                {
                    var z = getZ(point);
                    BinaryPrimitives.WriteDoubleLittleEndianIfNotNan(span, z);
                    heightStream.Write(span[..4]);

                    if (double.IsNaN(z) || z < Constants.NoDataLimit)
                    {
                        continue;
                    }

                    minimumZ = Math.Min(minimumZ, z);
                    maximumZ = Math.Max(maximumZ, z);
                }

                if (hasM)
                {
                    var m = getM(point);
                    BinaryPrimitives.WriteDoubleLittleEndianIfNotNan(span, m);
                    measurementStream.Write(span[..4]);

                    if (double.IsNaN(m) || m < Constants.NoDataLimit)
                    {
                        continue;
                    }

                    minimumM = Math.Min(minimumM, m);
                    maximumM = Math.Max(maximumM, m);
                }

                count++;
            }
        }

        if (minimumZ is double.MaxValue)
        {
            minimumZ = 0D;
        }

        if (minimumM is double.MaxValue)
        {
            minimumM = 0D;
        }

        if (maximumZ is double.MinValue)
        {
            maximumZ = 0D;
        }

        if (maximumM is double.MinValue)
        {
            maximumM = 0D;
        }

        var bounds = new EnvelopeZM(minimumX, minimumY, minimumZ, minimumM, maximumX, maximumY, maximumZ, maximumM);
        WriteHeader(stream, type, bounds, indexes, count);
        WriteIndexes(stream, indexes);

        if (type is ShpType.MultiPatch && (parts.Count is 0 || parts.Exists(static partType => partType is NullPartType)))
        {
            throw new InvalidGeometryTypeException();
        }

        WritePartTypes(stream, parts);

        pointStream.Position = 0;
        pointStream.CopyTo(stream);

        if (hasZ)
        {
            System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[..8], BitConverter.DoubleToInt64Bits(bounds.Front));
            System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[8..16], BitConverter.DoubleToInt64Bits(bounds.Back));
            stream.Write(span[..16]);
            heightStream.Position = 0;
            heightStream.CopyTo(stream);
        }

        if (hasM)
        {
            System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[..8], BitConverter.DoubleToInt64Bits(bounds.Measurement));
            System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[8..16], BitConverter.DoubleToInt64Bits(bounds.Measurement + bounds.Length));
            stream.Write(span[..16]);
            measurementStream.Position = 0;
            measurementStream.CopyTo(stream);
        }

        return count;

        static void WriteHeader(Stream stream, ShpType type, EnvelopeZM bounds, List<int> indexes, int count)
        {
            Span<byte> span = stackalloc byte[44];
            System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(span, (int)type);
            System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[4..12], BitConverter.DoubleToInt64Bits(bounds.Left));
            System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[12..20], BitConverter.DoubleToInt64Bits(bounds.Bottom));
            System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[20..28], BitConverter.DoubleToInt64Bits(bounds.Right));
            System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[28..36], BitConverter.DoubleToInt64Bits(bounds.Top));
            System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(span[36..40], indexes.Count);
            System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(span[40..44], count);

            stream.Write(span);
        }

        static void WriteIndexes(Stream stream, List<int> indexes)
        {
            var size = indexes.Count * sizeof(int);
            Span<byte> span = stackalloc byte[size];
            for (var i = 0; i < indexes.Count; i++)
            {
                var idx = 44 + (i * sizeof(int));
                System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(span[idx..], indexes[i]);
            }

            stream.Write(span);
        }

        static void WritePartTypes(Stream stream, List<ShpPartType> parts)
        {
            var size = parts.Count * sizeof(int);
            Span<byte> span = stackalloc byte[size];
            for (var i = 0; i < parts.Count; i++)
            {
                var idx = i * sizeof(int);
                System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(span[idx..], (int)parts[i]);
            }

            stream.Write(span);
        }
    }

    private static void WriteNull(Stream stream)
    {
        Span<byte> span = stackalloc byte[4];
        System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(span, (int)ShpType.NullShape);
        stream.Write(span);
    }

    private static DimensionsAndType GetDimensionsAndType(Point point) => (2, false, false, ShpType.Point);

    private static DimensionsAndType GetDimensionsAndType(PointZ point) => (3, true, false, ShpType.PointZ);

    private static DimensionsAndType GetDimensionsAndType(PointM point) => (3, false, true, ShpType.PointM);

    private static DimensionsAndType GetDimensionsAndType(PointZM point) => (4, true, true, ShpType.PointZ);

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

    private static ShpType PointToPolyline(ShpType type) => type + 2;

    private static ShpType PointToPolygon(ShpType type) => type + 4;

    private static ShpType PointToMultiPatch(ShpType type) => ShpType.MultiPatch;

    private void WritePolyLines(IEnumerable<Polyline<Point>> polylines) => this.Write(polylines, static (writer, lineStrings) => WriteImpl(writer, lineStrings, PointToPolyline, GetDimensionsAndType, GetXY, GetZ, GetM));

    private void WritePolyLines(IEnumerable<Polyline<PointZ>> polylines) => this.Write(polylines, static (writer, lineStrings) => WriteImpl(writer, lineStrings, PointToPolyline, GetDimensionsAndType, GetXY, GetZ, GetM));

    private void WritePolyLines(IEnumerable<Polyline<PointM>> polylines) => this.Write(polylines, static (writer, lineStrings) => WriteImpl(writer, lineStrings, PointToPolyline, GetDimensionsAndType, GetXY, GetZ, GetM));

    private void WritePolyLines(IEnumerable<Polyline<PointZM>> polylines) => this.Write(polylines, static (writer, lineStrings) => WriteImpl(writer, lineStrings, PointToPolyline, GetDimensionsAndType, GetXY, GetZ, GetM));

    private void WritePolygons(IEnumerable<Polygon<Point>> polygons) => this.Write(polygons, static (writer, polygons) => WriteImpl(writer, polygons.SelectMany(static polygon => polygon), PointToPolygon, GetDimensionsAndType, GetXY, GetZ, GetM));

    private void WritePolygons(IEnumerable<Polygon<PointZ>> polygons) => this.Write(polygons, static (writer, polygons) => WriteImpl(writer, polygons.SelectMany(static polygon => polygon), PointToPolygon, GetDimensionsAndType, GetXY, GetZ, GetM));

    private void WritePolygons(IEnumerable<Polygon<PointM>> polygons) => this.Write(polygons, static (writer, polygons) => WriteImpl(writer, polygons.SelectMany(static polygon => polygon), PointToPolygon, GetDimensionsAndType, GetXY, GetZ, GetM));

    private void WritePolygons(IEnumerable<Polygon<PointZM>> polygons) => this.Write(polygons, static (writer, polygons) => WriteImpl(writer, polygons.SelectMany(static polygon => polygon), PointToPolygon, GetDimensionsAndType, GetXY, GetZ, GetM));

    private void WriteMultiPatch(IEnumerable<Polygon<PointZ>> patches) => this.Write(patches, static (writer, patches) => WriteImpl(writer, patches.SelectMany(static patch => patch), PointToMultiPatch, GetDimensionsAndType, GetXY, GetZ, GetM));

    private void Write<T>([System.Diagnostics.CodeAnalysis.MaybeNull] T value, Func<Stream, T, int> write)
    {
        Span<byte> span = stackalloc byte[8];

        System.Buffers.Binary.BinaryPrimitives.WriteInt32BigEndian(span, ++this.recordNumber);
        System.Buffers.Binary.BinaryPrimitives.WriteUInt32BigEndian(span[4..8], 0U);
        this.BaseStream.Write(span);

        var startContent = this.BaseStream.Position;
        _ = write(this.BaseStream, value);

        // write the content length
        var endContent = this.BaseStream.Position;
        var length = (int)(endContent - startContent);
        this.BaseStream.Position = startContent - sizeof(int);

        System.Buffers.Binary.BinaryPrimitives.WriteInt32BigEndian(span, length / 2);
        this.BaseStream.Write(span[..4]);
        this.BaseStream.Position = endContent;
    }
}