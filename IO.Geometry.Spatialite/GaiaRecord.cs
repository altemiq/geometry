// -----------------------------------------------------------------------
// <copyright file="GaiaRecord.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Spatialite;

using Altemiq.Geometry;

/// <summary>
/// The <c>GAIA</c> <see cref="Data.IGeometryRecord"/>.
/// </summary>
public class GaiaRecord : Data.Common.BinaryGeometryRecord, Data.ISridGeometryRecord
{
    /// <summary>
    /// Initialises a new instance of the <see cref="GaiaRecord"/> class.
    /// </summary>
    /// <param name="bytes">The byte array to read from.</param>
    public GaiaRecord(byte[] bytes)
        : base(bytes, 0, bytes.Length)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="GaiaRecord"/> class.
    /// </summary>
    /// <param name="bytes">The byte array to read from.</param>
    /// <param name="start">The index in <paramref name="bytes"/> at which the stream begins.</param>
    /// <param name="length">The length of the stream in bytes.</param>
    public GaiaRecord(byte[] bytes, int start, int length)
        : base(bytes, start, length)
    {
    }

    private delegate T ReadPointDelegate<out T>(ref ReadOnlySpan<byte> span, bool littleEndian);

    private delegate T CreateFunction<out T>(ref ReadOnlySpan<byte> span, GaiaGeometryType type, bool littleEndian);

    /// <summary>
    /// Tries to create a <see cref="GaiaRecord"/>.
    /// </summary>
    /// <param name="bytes">The bytes.</param>
    /// <param name="record">The created record.</param>
    /// <returns><see langword="true"/> if successful; otherwise <see langword="false"/>.</returns>
    public static bool TryCreate(byte[] bytes, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out GaiaRecord? record)
    {
        var tempRecord = new GaiaRecord(bytes);
        var (successful, _, _, _, _) = tempRecord.ReadHeader();
        record = successful ? tempRecord : default;
        return successful;
    }

    /// <inheritdoc/>
    public int GetSrid()
    {
        var (successful, srid, _, _, _) = this.ReadHeader();
        if (!successful)
        {
            throw new InvalidDataException();
        }

        return srid;
    }

    /// <inheritdoc/>
    public override object GetGeometry()
    {
        var (successful, _, littleEndian, _, type) = this.ReadHeader();
        if (!successful)
        {
            throw new InvalidDataException();
        }

        var span = this.AsSpanWithoutHeader();
        object returnValue = ToBase(type) switch
        {
            GaiaGeometryType.Point => ReadPoint(ref span, type, littleEndian),
            GaiaGeometryType.PointZ => ReadPointZ(ref span, type, littleEndian),
            GaiaGeometryType.PointM => ReadPointM(ref span, type, littleEndian),
            GaiaGeometryType.PointZM => ReadPointZM(ref span, type, littleEndian),
            GaiaGeometryType.MultiPoint => ReadMultiImpl(ref span, littleEndian, ReadPoint),
            GaiaGeometryType.MultiPointZ => ReadMultiImpl(ref span, littleEndian, ReadPointZ),
            GaiaGeometryType.MultiPointM => ReadMultiImpl(ref span, littleEndian, ReadPointM),
            GaiaGeometryType.MultiPointZM => ReadMultiImpl(ref span, littleEndian, ReadPointZM),
            GaiaGeometryType.LineString => ReadPolyline(ref span, type, littleEndian),
            GaiaGeometryType.LineStringZ => ReadPolylineZ(ref span, type, littleEndian),
            GaiaGeometryType.LineStringM => ReadPolylineM(ref span, type, littleEndian),
            GaiaGeometryType.LineStringZM => ReadPolylineZM(ref span, type, littleEndian),
            GaiaGeometryType.MultiLineString => ReadMultiImpl(ref span, littleEndian, ReadPolyline),
            GaiaGeometryType.MultiLineStringZ => ReadMultiImpl(ref span, littleEndian, ReadPolylineZ),
            GaiaGeometryType.MultiLineStringM => ReadMultiImpl(ref span, littleEndian, ReadPolylineM),
            GaiaGeometryType.MultiLineStringZM => ReadMultiImpl(ref span, littleEndian, ReadPolylineZM),
            GaiaGeometryType.Polygon => ReadPolygon(ref span, type, littleEndian),
            GaiaGeometryType.PolygonZ => ReadPolygonZ(ref span, type, littleEndian),
            GaiaGeometryType.PolygonM => ReadPolygonM(ref span, type, littleEndian),
            GaiaGeometryType.PolygonZM => ReadPolygonZM(ref span, type, littleEndian),
            GaiaGeometryType.MultiPolygon => ReadMultiImpl(ref span, littleEndian, ReadPolygon),
            GaiaGeometryType.MultiPolygonZ => ReadMultiImpl(ref span, littleEndian, ReadPolygonZ),
            GaiaGeometryType.MultiPolygonM => ReadMultiImpl(ref span, littleEndian, ReadPolygonM),
            GaiaGeometryType.MultiPolygonZM => ReadMultiImpl(ref span, littleEndian, ReadPolygonZM),
            _ => throw new Altemiq.Geometry.InvalidGeometryTypeException(),
        };

        return CheckEnd(span, returnValue);
    }

    /// <inheritdoc/>
    public override Point GetPoint() => this.ReadPoint(ReadPoint);

    /// <inheritdoc/>
    public override PointZ GetPointZ() => this.ReadPoint(ReadPointZ);

    /// <inheritdoc/>
    public override PointM GetPointM() => this.ReadPoint(ReadPointM);

    /// <inheritdoc/>
    public override PointZM GetPointZM() => this.ReadPoint(ReadPointZM);

    /// <inheritdoc/>
    public override IReadOnlyCollection<Point> GetMultiPoint() => this.ReadMulti(ReadPoint);

    /// <inheritdoc/>
    public override IReadOnlyCollection<PointZ> GetMultiPointZ() => this.ReadMulti(ReadPointZ);

    /// <inheritdoc/>
    public override IReadOnlyCollection<PointM> GetMultiPointM() => this.ReadMulti(ReadPointM);

    /// <inheritdoc/>
    public override IReadOnlyCollection<PointZM> GetMultiPointZM() => this.ReadMulti(ReadPointZM);

    /// <inheritdoc/>
    public override Polyline GetLineString() => this.ReadLineString(ReadPolyline);

    /// <inheritdoc/>
    public override PolylineZ GetLineStringZ() => this.ReadLineString(ReadPolylineZ);

    /// <inheritdoc/>
    public override PolylineM GetLineStringM() => this.ReadLineString(ReadPolylineM);

    /// <inheritdoc/>
    public override PolylineZM GetLineStringZM() => this.ReadLineString(ReadPolylineZM);

    /// <inheritdoc/>
    public override IReadOnlyCollection<Polyline> GetMultiLineString() => this.ReadMulti(ReadPolyline);

    /// <inheritdoc/>
    public override IReadOnlyCollection<PolylineZ> GetMultiLineStringZ() => this.ReadMulti(ReadPolylineZ);

    /// <inheritdoc/>
    public override IReadOnlyCollection<PolylineM> GetMultiLineStringM() => this.ReadMulti(ReadPolylineM);

    /// <inheritdoc/>
    public override IReadOnlyCollection<PolylineZM> GetMultiLineStringZM() => this.ReadMulti(ReadPolylineZM);

    /// <inheritdoc/>
    public override Polygon GetPolygon() => this.ReadPolygon(ReadPolygon);

    /// <inheritdoc/>
    public override PolygonZ GetPolygonZ() => this.ReadPolygon(ReadPolygonZ);

    /// <inheritdoc/>
    public override PolygonM GetPolygonM() => this.ReadPolygon(ReadPolygonM);

    /// <inheritdoc/>
    public override PolygonZM GetPolygonZM() => this.ReadPolygon(ReadPolygonZM);

    /// <inheritdoc/>
    public override IReadOnlyCollection<Polygon> GetMultiPolygon() => this.ReadMulti(ReadPolygon);

    /// <inheritdoc/>
    public override IReadOnlyCollection<PolygonZ> GetMultiPolygonZ() => this.ReadMulti(ReadPolygonZ);

    /// <inheritdoc/>
    public override IReadOnlyCollection<PolygonM> GetMultiPolygonM() => this.ReadMulti(ReadPolygonM);

    /// <inheritdoc/>
    public override IReadOnlyCollection<PolygonZM> GetMultiPolygonZM() => this.ReadMulti(ReadPolygonZM);

    /// <inheritdoc/>
    public override bool IsNull() => this.AsSpan().Length is 0;

    private static Point ReadPoint(ref ReadOnlySpan<byte> span, GaiaGeometryType type, bool littleEndian)
    {
        CheckType(type, GaiaGeometryType.Point);
        return ReadPoint(ref span, littleEndian);
    }

    private static Point ReadPoint(ref ReadOnlySpan<byte> span, bool littleEndian)
    {
        var point = new Point(ReadDouble(span, littleEndian), ReadDouble(span[8..], littleEndian));
        span = span[16..];
        return point;
    }

    private static PointZ ReadPointZ(ref ReadOnlySpan<byte> span, GaiaGeometryType type, bool littleEndian)
    {
        CheckType(type, GaiaGeometryType.PointZ);
        return ReadPointZ(ref span, littleEndian);
    }

    private static PointZ ReadPointZ(ref ReadOnlySpan<byte> span, bool littleEndian)
    {
        var point = new PointZ(ReadDouble(span, littleEndian), ReadDouble(span[8..], littleEndian), ReadDouble(span[16..], littleEndian));
        span = span[24..];
        return point;
    }

    private static PointM ReadPointM(ref ReadOnlySpan<byte> span, GaiaGeometryType type, bool littleEndian)
    {
        CheckType(type, GaiaGeometryType.PointM);
        return ReadPointM(ref span, littleEndian);
    }

    private static PointM ReadPointM(ref ReadOnlySpan<byte> span, bool littleEndian)
    {
        var point = new PointM(ReadDouble(span, littleEndian), ReadDouble(span[8..], littleEndian), ReadDouble(span[16..], littleEndian));
        span = span[24..];
        return point;
    }

    private static PointZM ReadPointZM(ref ReadOnlySpan<byte> span, GaiaGeometryType type, bool littleEndian)
    {
        CheckType(type, GaiaGeometryType.PointZM);
        return ReadPointZM(ref span, littleEndian);
    }

    private static PointZM ReadPointZM(ref ReadOnlySpan<byte> span, bool littleEndian)
    {
        var point = new PointZM(ReadDouble(span, littleEndian), ReadDouble(span[8..], littleEndian), ReadDouble(span[16..], littleEndian), ReadDouble(span[24..], littleEndian));
        span = span[32..];
        return point;
    }

    private static T[] ReadLineString<T>(ref ReadOnlySpan<byte> span, GaiaGeometryType type, ReadPointDelegate<T> func, bool littleEndian)
    {
        CheckType(ToBase(type), GaiaGeometryType.LineString);
        return ReadPointsImpl(ref span, func, littleEndian);
    }

    private static Polyline ReadPolyline(ref ReadOnlySpan<byte> span, GaiaGeometryType type, bool littleEndian) => [.. ReadLineString(ref span, type, ReadPoint, littleEndian)];

    private static PolylineZ ReadPolylineZ(ref ReadOnlySpan<byte> span, GaiaGeometryType type, bool littleEndian) => [.. ReadLineString(ref span, type, ReadPointZ, littleEndian)];

    private static PolylineM ReadPolylineM(ref ReadOnlySpan<byte> span, GaiaGeometryType type, bool littleEndian) => [.. ReadLineString(ref span, type, ReadPointM, littleEndian)];

    private static PolylineZM ReadPolylineZM(ref ReadOnlySpan<byte> span, GaiaGeometryType type, bool littleEndian) => [.. ReadLineString(ref span, type, ReadPointZM, littleEndian)];

    private static LinearRing<T>[] ReadPolygon<T>(ref ReadOnlySpan<byte> span, GaiaGeometryType type, ReadPointDelegate<T> func, bool littleEndian)
        where T : struct
    {
        CheckType(ToBase(type), GaiaGeometryType.Polygon);

        var number = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span);
        span = span[4..];

        var rings = new LinearRing<T>[number];
        for (var i = 0; i < number; i++)
        {
            rings[i] = [.. ReadPointsImpl(ref span, func, littleEndian)];
        }

        return rings;
    }

    private static Polygon ReadPolygon(ref ReadOnlySpan<byte> span, GaiaGeometryType type, bool littleEndian) => [.. ReadPolygon(ref span, type, ReadPoint, littleEndian)];

    private static PolygonZ ReadPolygonZ(ref ReadOnlySpan<byte> span, GaiaGeometryType type, bool littleEndian) => [.. ReadPolygon(ref span, type, ReadPointZ, littleEndian)];

    private static PolygonM ReadPolygonM(ref ReadOnlySpan<byte> span, GaiaGeometryType type, bool littleEndian) => [.. ReadPolygon(ref span, type, ReadPointM, littleEndian)];

    private static PolygonZM ReadPolygonZM(ref ReadOnlySpan<byte> span, GaiaGeometryType type, bool littleEndian) => [.. ReadPolygon(ref span, type, ReadPointZM, littleEndian)];

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

    private static T[] ReadMultiImpl<T>(ref ReadOnlySpan<byte> span, bool littleEndian, CreateFunction<T> creationFunction)
    {
        var count = ReadInt32(span, littleEndian);
        span = span[4..];
        return ReadMultiImpl(span, littleEndian, creationFunction, count);
    }

    private static T[] ReadMultiImpl<T>(ReadOnlySpan<byte> span, bool littleEndian, CreateFunction<T> creationFunction, int number)
    {
        var items = new T[number];
        for (var i = 0; i < number; i++)
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

        return CheckEnd(span, items);
    }

    [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(value))]
    private static T CheckEnd<T>(ReadOnlySpan<byte> span, [System.Diagnostics.CodeAnalysis.MaybeNull] T value) => span[0] is GaiaConstants.BlobMark.End ? value! : throw new InvalidOperationException(Properties.Resources.LastByteWasNotTheEndMarker);

    private static GaiaGeometryType ToBase(GaiaGeometryType type) => (GaiaGeometryType)((int)type % 1000);

    private static double ReadDouble(ReadOnlySpan<byte> span, bool littleEndian) =>
        BitConverter.Int64BitsToDouble(littleEndian ? System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(span) : System.Buffers.Binary.BinaryPrimitives.ReadInt64BigEndian(span));

    private static int ReadInt32(ReadOnlySpan<byte> span, bool littleEndian) => littleEndian ? System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span) : System.Buffers.Binary.BinaryPrimitives.ReadInt32BigEndian(span);

    private static void CheckType(GaiaGeometryType actual, GaiaGeometryType expected)
    {
        if (actual != expected)
        {
            throw new Altemiq.Geometry.InvalidGeometryTypeException();
        }
    }

    private T ReadPoint<T>(CreateFunction<T> func)
    {
        var (successful, _, byteOrder, _, type) = this.ReadHeader();
        if (!successful)
        {
            throw new InvalidDataException();
        }

        var span = this.AsSpanWithoutHeader();
        var point = func(ref span, type, byteOrder);
        return CheckEnd(span, point);
    }

    private TPolyline ReadLineString<TPolyline>(CreateFunction<TPolyline> func)
    {
        var (successful, _, byteOrder, _, type) = this.ReadHeader();
        if (!successful)
        {
            throw new InvalidDataException();
        }

        var span = this.AsSpanWithoutHeader();
        var lineString = func(ref span, type, byteOrder);
        return CheckEnd(span, lineString);
    }

    private TPolygon ReadPolygon<TPolygon>(CreateFunction<TPolygon> func)
    {
        var (successful, _, byteOrder, _, type) = this.ReadHeader();
        if (!successful)
        {
            throw new InvalidDataException();
        }

        var span = this.AsSpanWithoutHeader();
        var polygon = func(ref span, type, byteOrder);
        return CheckEnd(span, polygon);
    }

    private T[] ReadMulti<T>(CreateFunction<T> creationFunction)
    {
        var (successful, _, littleEndian, _, type) = this.ReadHeader();
        if (!successful)
        {
            throw new InvalidGeometryTypeException();
        }

        // see if this is one of the multi-geometries
        if (ToBase(type) <= GaiaGeometryType.Polygon)
        {
            throw new InvalidGeometryTypeException();
        }

        var span = this.AsSpanWithoutHeader();
        return ReadMultiImpl(ref span, littleEndian, creationFunction);
    }

    private (bool Successful, int Srid, bool LittleEndian, double[] Envelope, GaiaGeometryType Type) ReadHeader()
    {
        var span = this.AsSpan();
        if (span.Length < 45)
        {
            // cannot be an internal BLOB WKB geometry
            return default;
        }

        if (span[0] is not GaiaConstants.BlobMark.Start || span[38] is not GaiaConstants.BlobMark.Mbr)
        {
            // failed to recognize START signature or MBR
            return default;
        }

        var littleEndian = span[1] is 1;
        var srid = ReadInt32(span[2..], littleEndian);

        var envelope = new[] { ReadDouble(span[6..14], littleEndian), ReadDouble(span[14..22], littleEndian), ReadDouble(span[22..30], littleEndian), ReadDouble(span[30..38], littleEndian), };

        return (Successful: true, srid, littleEndian, envelope, (GaiaGeometryType)ReadInt32(span[39..], littleEndian));
    }

    private ReadOnlySpan<byte> AsSpanWithoutHeader() => this.AsSpan(43);
}