// -----------------------------------------------------------------------
// <copyright file="ShpRecord.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

/// <summary>
/// The SHP record.
/// </summary>
/// <remarks>
/// Initialises a new instance of the <see cref="ShpRecord"/> class.
/// </remarks>
/// <param name="recordHeader">The SHP record header.</param>
/// <param name="data">The record data.</param>
public class ShpRecord(ShpRecordHeader recordHeader, byte[] data) : Data.IGeometryRecord
{
    private delegate IEnumerable<T> ReadMultiPointDelegate<out T>(ref ReadOnlySpan<byte> span, int firstPosition, int secondPosition, int thirdPosition, int count);

    /// <summary>
    /// Gets the record header.
    /// </summary>
    public ShpRecordHeader RecordHeader { get; } = recordHeader;

    /// <summary>
    /// Gets the extents of the item.
    /// </summary>
    /// <returns>The extents of the item.</returns>
    /// <exception cref="InvalidGeometryTypeException">The <see cref="ShpType"/> is invalid.</exception>
    public Envelope GetExtents()
    {
        ReadOnlySpan<byte> span = data;
        return (ShpType)System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span) switch
        {
            ShpType.Point or ShpType.PointZ or ShpType.PointM => new(GetPoint(span[4..]), Size.Empty),
            ShpType.MultiPoint or ShpType.MultiPointM or ShpType.MultiPointZ or ShpType.PolyLine or ShpType.PolyLineZ or ShpType.PolyLineM or ShpType.Polygon or ShpType.PolygonZ or ShpType.PolygonM or ShpType.MultiPatch => ReadEnvelop(span[4..]),
            ShpType.NullShape => default,
            _ => throw new InvalidGeometryTypeException(),
        };
    }

    /// <inheritdoc/>
    public Point GetPoint()
    {
        this.CheckType(ShpType.Point);
        return GetPoint(data.AsSpan(4));
    }

    /// <inheritdoc/>
    public PointZ GetPointZ()
    {
        this.CheckType(ShpType.PointZ);
        return GetPointZ(data.AsSpan(4));
    }

    /// <inheritdoc/>
    public PointM GetPointM()
    {
        this.CheckType(ShpType.PointM);
        return GetPointM(data.AsSpan(4));
    }

    /// <inheritdoc/>
    public PointZM GetPointZM()
    {
        this.CheckType(ShpType.PointZ);
        if (data.Length > 28)
        {
            return GetPointZM(data.AsSpan(4));
        }

        throw new Data.InsufficientDataException();
    }

    /// <inheritdoc/>
    public IMultiGeometry<Point> GetMultiPoint()
    {
        this.CheckType(ShpType.MultiPoint);
        return [.. this.GetMultiPoint(ReadMultiPoint)];
    }

    /// <inheritdoc/>
    public IMultiGeometry<PointZ> GetMultiPointZ()
    {
        this.CheckType(ShpType.MultiPointZ);
        return [.. this.GetMultiPoint(ReadMultiPointZ)];
    }

    /// <inheritdoc/>
    public IMultiGeometry<PointM> GetMultiPointM()
    {
        this.CheckType(ShpType.MultiPointM);
        return [.. this.GetMultiPoint(ReadMultiPointM)];
    }

    /// <inheritdoc/>
    public IMultiGeometry<PointZM> GetMultiPointZM()
    {
        this.CheckType(ShpType.MultiPoint);
        return [.. this.GetMultiPoint(ReadMultiPointZM)];
    }

    /// <inheritdoc/>
    public Polyline GetLineString()
    {
        this.CheckType(ShpType.PolyLine);
        return this.GetMultiLineString().Single();
    }

    /// <inheritdoc/>
    public PolylineZ GetLineStringZ()
    {
        this.CheckType(ShpType.PolyLineZ);
        return this.GetMultiLineStringZ().Single();
    }

    /// <inheritdoc/>
    public PolylineM GetLineStringM()
    {
        this.CheckType(ShpType.PolyLineM);
        return this.GetMultiLineStringM().Single();
    }

    /// <inheritdoc/>
    public PolylineZM GetLineStringZM()
    {
        this.CheckType(ShpType.PolyLineZ);
        return this.GetMultiLineStringZM().Single();
    }

    /// <inheritdoc/>
    public IMultiGeometry<Polyline> GetMultiLineString() => [.. this.GetMultiLineString(ReadMultiPoint, points => new Polyline(points))];

    /// <inheritdoc/>
    public IMultiGeometry<PolylineZ> GetMultiLineStringZ() => [.. this.GetMultiLineString(ReadMultiPointZ, points => new PolylineZ(points))];

    /// <inheritdoc/>
    public IMultiGeometry<PolylineM> GetMultiLineStringM() => [.. this.GetMultiLineString(ReadMultiPointM, points => new PolylineM(points))];

    /// <inheritdoc/>
    public IMultiGeometry<PolylineZM> GetMultiLineStringZM() => [.. this.GetMultiLineString(ReadMultiPointZM, points => new PolylineZM(points))];

    /// <inheritdoc/>
    public Polygon GetPolygon()
    {
        this.CheckType(ShpType.Polygon);
        return this.GetMultiPolygon().Single();
    }

    /// <inheritdoc/>
    public PolygonZ GetPolygonZ()
    {
        this.CheckType(ShpType.PolygonZ);
        return this.GetMultiPolygonZ().Single();
    }

    /// <inheritdoc/>
    public PolygonM GetPolygonM()
    {
        this.CheckType(ShpType.PolygonM);
        return this.GetMultiPolygonM().Single();
    }

    /// <inheritdoc/>
    public PolygonZM GetPolygonZM()
    {
        this.CheckType(ShpType.PolygonZ);
        return this.GetMultiPolygonZM().Single();
    }

    /// <inheritdoc/>
    public IMultiGeometry<Polygon> GetMultiPolygon() => [.. this.GetMultiPolygon(ReadMultiPoint, points => new Polygon(points))];

    /// <inheritdoc/>
    public IMultiGeometry<PolygonZ> GetMultiPolygonZ() => [.. this.GetMultiPolygon(ReadMultiPointZ, points => new PolygonZ(points))];

    /// <inheritdoc/>
    public IMultiGeometry<PolygonM> GetMultiPolygonM() => [.. this.GetMultiPolygon(ReadMultiPointM, points => new PolygonM(points))];

    /// <inheritdoc/>
    public IMultiGeometry<PolygonZM> GetMultiPolygonZM() => [.. this.GetMultiPolygon(ReadMultiPointZM, points => new PolygonZM(points))];

    /// <inheritdoc/>
    public IGeometry GetGeometry()
    {
        var shapeType = this.ReadShpType();
        return shapeType switch
        {
            ShpType.Point => GetPoint(data.AsSpan(4)),
            ShpType.PointM => GetPointM(data.AsSpan(4)),
            ShpType.PointZ when data.Length > 28 => GetPointZM(data.AsSpan(4)),
            ShpType.PointZ => GetPointZ(data.AsSpan(4)),
            ShpType.MultiPoint => this.GetMultiPoint(),
            ShpType.MultiPointZ => this.GetMultiPointZ(),
            ShpType.MultiPointM => this.GetMultiPointM(),
            ShpType.PolyLine => this.GetMultiLineString(),
            ShpType.PolyLineZ => this.GetMultiLineStringZ(),
            ShpType.PolyLineM => this.GetMultiLineStringM(),
            ShpType.Polygon => this.GetMultiPolygon(),
            ShpType.PolygonZ => this.GetMultiPolygonZ(),
            ShpType.PolygonM => this.GetMultiPolygonM(),
            ShpType.MultiPatch => GetMultiPatch(),
            _ => throw new InvalidGeometryTypeException(),
        };

        IMultiGeometry<PolygonZ> GetMultiPatch()
        {
            return [.. GetMultiPatchCore()];

            IEnumerable<PolygonZ> GetMultiPatchCore()
            {
                var span = new ReadOnlySpan<byte>(data, 36, data.Length - 36);
                var partCount = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[..4]);
                var pointCount = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[4..8]);
                span = span[8..];
                var parts = ReadPartIndexes(ref span, partCount);
                var partTypes = ReadPartTypes(ref span, partCount);

                List<LinearRing<PointZ>>? linearRings = null;
                ShpPartType previousPartType = default;
                foreach (var (part, partType) in ReadParts(span, parts, pointCount, ReadMultiPointZ).Zip(partTypes, (part, partType) => (part, partType)))
                {
                    switch (partType, previousPartType)
                    {
                        case (ShpPartType.TriangleStrip, _):
                        case (ShpPartType.TriangleFan, _):
                            if (linearRings is not null)
                            {
                                yield return new(linearRings);
                                linearRings = null;
                            }

                            yield return new(new ShpLinearRing<PointZ>(partType, part));
                            break;
                        case (ShpPartType.OuterRing, _):
                        case (ShpPartType.FirstRing, _):
                            if (linearRings is not null)
                            {
                                yield return new(linearRings);
                            }

                            linearRings = [new ShpLinearRing<PointZ>(partType, part)];
                            break;
                        case (ShpPartType.InnerRing, ShpPartType.OuterRing or ShpPartType.InnerRing):
                        case (ShpPartType.Ring, ShpPartType.FirstRing or ShpPartType.Ring):
                            linearRings ??= [];
                            linearRings.Add(new ShpLinearRing<PointZ>(partType, part));
                            break;
                        default:
                            throw new InvalidGeometryTypeException();
                    }

                    previousPartType = partType;
                }

                if (linearRings is not null)
                {
                    yield return new(linearRings);
                }

                static ShpPartType[] ReadPartTypes(ref ReadOnlySpan<byte> span, int partCount)
                {
                    var types = new ShpPartType[partCount];
                    for (var i = 0; i < partCount; i++)
                    {
                        types[i] = (ShpPartType)System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span);
                        span = span[4..];
                    }

                    return types;
                }
            }
        }
    }

    /// <inheritdoc/>
    public bool IsNull() => this.ReadShpType() is ShpType.NullShape;

    /// <inheritdoc/>
    public override string ToString()
    {
        FormattableString formattableString = $"{nameof(ShpRecordHeader.RecordNumber)}: {this.RecordHeader.RecordNumber}, Geometry: {(this.GetGeometry() is { } geometry ? geometry : "null")}";
        return formattableString.ToString(System.Globalization.CultureInfo.CurrentCulture);
    }

    private static Point[] ReadMultiPoint(ref ReadOnlySpan<byte> span, int xyPosition, int zPosition, int mPosition, int count) => ReadMultiPoint(ref span, count);

    private static Point[] ReadMultiPoint(ref ReadOnlySpan<byte> span, int count)
    {
        var points = new Point[count];
        for (var i = 0; i < count; i++)
        {
            var x = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(span));
            var y = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(span[sizeof(double)..]));
            points[i] = new(x, y);
            span = span[16..];
        }

        return points;
    }

    private static PointM[] ReadMultiPointM(ref ReadOnlySpan<byte> span, int xyPosition, int zPosition, int mPosition, int count) => ReadMultiPointM(ref span, xyPosition, mPosition, count);

    private static PointM[] ReadMultiPointM(ref ReadOnlySpan<byte> span, int xyPosition, int mPosition, int count)
    {
        var points = new PointM[count];
        for (var i = 0; i < count; i++)
        {
            var currentXYPosition = xyPosition + (i * 2 * sizeof(double));
            var x = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(span[currentXYPosition..]));
            var y = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(span[(currentXYPosition + sizeof(double))..]));
            var currentMPosition = mPosition + (i * sizeof(double));
            var m = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(span[currentMPosition..]));
            points[i] = new(x, y, m);
        }

        return points;
    }

    private static PointZ[] ReadMultiPointZ(ref ReadOnlySpan<byte> span, int xyPosition, int zPosition, int mPosition, int count) => ReadMultiPointZ(ref span, xyPosition, zPosition, count);

    private static PointZ[] ReadMultiPointZ(ref ReadOnlySpan<byte> span, int xyPosition, int zPosition, int count)
    {
        var points = new PointZ[count];
        for (var i = 0; i < count; i++)
        {
            var currentXYPosition = xyPosition + (i * 2 * sizeof(double));
            var x = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(span[currentXYPosition..]));
            var y = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(span[(currentXYPosition + sizeof(double))..]));
            var currentZPosition = zPosition + (i * sizeof(double));
            var z = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(span[currentZPosition..]));
            points[i] = new(x, y, z);
        }

        return points;
    }

    private static PointZM[] ReadMultiPointZM(ref ReadOnlySpan<byte> span, int xyPosition, int zPosition, int mPosition, int count)
    {
        var points = new PointZM[count];
        for (var i = 0; i < count; i++)
        {
            var currentXYPosition = xyPosition + (i * 2 * sizeof(double));
            var x = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(span[currentXYPosition..]));
            var y = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(span[(currentXYPosition + sizeof(double))..]));
            var currentZPosition = zPosition + (i * sizeof(double));
            var z = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(span[currentZPosition..]));

            var currentMPosition = mPosition + (i * sizeof(double));
            var m = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(span[currentMPosition..]));
            points[i] = new(x, y, z, m);
        }

        return points;
    }

    private static Envelope ReadEnvelop(ReadOnlySpan<byte> source)
    {
        var minX = ReadDouble(source[..8]);
        var minY = ReadDouble(source[8..16]);
        var maxX = ReadDouble(source[16..24]);
        var maxY = ReadDouble(source[24..32]);

        return new(minX, minY, maxX, maxY);
    }

    private static ShpType ReadShpType(ReadOnlySpan<byte> source) => (ShpType)System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(source);

    private static double ReadDouble(ReadOnlySpan<byte> source)
    {
        var value = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(source));
        return value < Constants.NoDataLimit
            ? double.NaN
            : value;
    }

    private static int[] ReadPartIndexes(ref ReadOnlySpan<byte> source, int partCount)
    {
        var indexes = new int[partCount];
        for (var i = 0; i < partCount; i++)
        {
            indexes[i] = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(source);
            source = source[4..];
        }

        return indexes;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance", Justification = "Needed for enumeration.")]
    private static List<IEnumerable<T>> ReadParts<T>(ReadOnlySpan<byte> span, IEnumerable<int> parts, int pointCount, ReadMultiPointDelegate<T> readMultiPoint)
        where T : struct
    {
        using var enumerator = parts.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            return [];
        }

        var (firstPosition, secondPosition, thirdPosition) = CalculatePositions(pointCount);

        var list = new List<IEnumerable<T>>();
        var previous = enumerator.Current;
        while (enumerator.MoveNext())
        {
            var current = enumerator.Current;
            var count = current - previous;
            list.Add(readMultiPoint(ref span, firstPosition, secondPosition, thirdPosition, count));
            previous = current;

            firstPosition += count * 2 * sizeof(double);
            secondPosition += count * sizeof(double);
            thirdPosition += count * sizeof(double);
        }

        list.Add(readMultiPoint(ref span, firstPosition, secondPosition, thirdPosition, pointCount - previous));

        return list;
    }

    private static (int XY, int Z, int M) CalculatePositions(int pointCount)
    {
        const int Xy = 0;
        var z = Xy + (2 * sizeof(double)) + (2 * sizeof(double) * pointCount);
        var m = z + (2 * sizeof(double)) + (sizeof(double) * pointCount);
        return (Xy, z, m);
    }

    private static Point GetPoint(ReadOnlySpan<byte> source)
    {
        var x = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(source[..8]));
        var y = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(source[8..16]));
        return new(x, y);
    }

    private static PointZ GetPointZ(ReadOnlySpan<byte> source)
    {
        var x = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(source[..8]));
        var y = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(source[8..16]));
        var z = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(source[16..24]));
        return new(x, y, z);
    }

    private static PointZM GetPointZM(ReadOnlySpan<byte> source)
    {
        var x = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(source[..8]));
        var y = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(source[8..16]));
        var z = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(source[16..24]));
        var m = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(source[24..32]));
        return new(x, y, z, m);
    }

    private static PointM GetPointM(ReadOnlySpan<byte> source)
    {
        var x = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(source[..8]));
        var y = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(source[8..16]));
        var m = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(source[16..24]));
        return new(x, y, m);
    }

    private ShpType ReadShpType() => ReadShpType(data);

    private void CheckType(ShpType type)
    {
        if (this.ReadShpType() != type)
        {
            throw new InvalidGeometryTypeException();
        }
    }

    private IEnumerable<T> GetMultiPoint<T>(ReadMultiPointDelegate<T> createFunction)
    {
        var span = new ReadOnlySpan<byte>(data, 4, data.Length - 4);
        _ = ReadEnvelop(span);

        var count = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[32..]);
        var (firstPosition, secondPosition, thirdPosition) = CalculatePositions(count);
        span = span[36..];

        return createFunction(ref span, firstPosition, secondPosition, thirdPosition, count);
    }

    private IEnumerable<TPolyline> GetMultiLineString<TPolyline, TPoint>(ReadMultiPointDelegate<TPoint> readPoints, Func<IEnumerable<TPoint>, TPolyline> createFunction)
        where TPolyline : Polyline<TPoint>, new()
        where TPoint : struct => this.GetPolyLineOrPolygon(readPoints)
        .Select(createFunction);

    private IEnumerable<TPolygon> GetMultiPolygon<TPolygon, TPoint>(ReadMultiPointDelegate<TPoint> readPoints, Func<IEnumerable<TPoint>, TPolygon> createFunction)
        where TPolygon : Polygon<TPoint>, new()
        where TPoint : struct => this.GetPolyLineOrPolygon(readPoints)
        .Select(createFunction);

    private List<IEnumerable<T>> GetPolyLineOrPolygon<T>(ReadMultiPointDelegate<T> readPoints)
        where T : struct
    {
        var span = new ReadOnlySpan<byte>(data, 36, data.Length - 36);
        var partCount = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[..4]);
        var pointCount = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[4..8]);
        span = span[8..];
        var parts = ReadPartIndexes(ref span, partCount);
        return ReadParts(span, parts, pointCount, readPoints);
    }
}