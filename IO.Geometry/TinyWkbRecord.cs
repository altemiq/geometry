// -----------------------------------------------------------------------
// <copyright file="TinyWkbRecord.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

/// <summary>
/// Represents an implementation of <see cref="Data.IGeometryRecord"/> that reads Tiny Well-Known Binary.
/// </summary>
public class TinyWkbRecord : Data.IGeometryRecord, IDisposable
{
    private readonly Stream stream;

    /// <summary>
    /// Initialises a new instance of the <see cref="TinyWkbRecord"/> class.
    /// </summary>
    /// <param name="bytes">The byte array to read from.</param>
    public TinyWkbRecord(byte[] bytes)
        : this(new MemoryStream(bytes ?? throw new ArgumentNullException(nameof(bytes)), writable: false))
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="TinyWkbRecord"/> class.
    /// </summary>
    /// <param name="stream">The stream to read from.</param>
    public TinyWkbRecord(Stream stream)
    {
        this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
        System.Diagnostics.Contracts.Contract.EndContractBlock();
    }

    /// <inheritdoc/>
    public Point GetPoint() => this.GetGeometry<Point>();

    /// <inheritdoc/>
    public PointZ GetPointZ() => this.GetGeometry<PointZ>();

    /// <inheritdoc/>
    public PointM GetPointM() => this.GetGeometry<PointM>();

    /// <inheritdoc/>
    public PointZM GetPointZM() => this.GetGeometry<PointZM>();

    /// <inheritdoc/>
    public IReadOnlyCollection<Point> GetMultiPoint() => this.GetEnumerableGeometry<Point>();

    /// <inheritdoc/>
    public IReadOnlyCollection<PointZ> GetMultiPointZ() => this.GetEnumerableGeometry<PointZ>();

    /// <inheritdoc/>
    public IReadOnlyCollection<PointM> GetMultiPointM() => this.GetEnumerableGeometry<PointM>();

    /// <inheritdoc/>
    public IReadOnlyCollection<PointZM> GetMultiPointZM() => this.GetEnumerableGeometry<PointZM>();

    /// <inheritdoc/>
    public Polyline GetLineString() => this.GetGeometry<Polyline>();

    /// <inheritdoc/>
    public PolylineZ GetLineStringZ() => this.GetGeometry<PolylineZ>();

    /// <inheritdoc/>
    public PolylineM GetLineStringM() => this.GetGeometry<PolylineM>();

    /// <inheritdoc/>
    public PolylineZM GetLineStringZM() => this.GetGeometry<PolylineZM>();

    /// <inheritdoc/>
    public IReadOnlyCollection<Polyline> GetMultiLineString() => this.GetEnumerableGeometry<Polyline>();

    /// <inheritdoc/>
    public IReadOnlyCollection<PolylineZ> GetMultiLineStringZ() => this.GetEnumerableGeometry<PolylineZ>();

    /// <inheritdoc/>
    public IReadOnlyCollection<PolylineM> GetMultiLineStringM() => this.GetEnumerableGeometry<PolylineM>();

    /// <inheritdoc/>
    public IReadOnlyCollection<PolylineZM> GetMultiLineStringZM() => this.GetEnumerableGeometry<PolylineZM>();

    /// <inheritdoc/>
    public Polygon GetPolygon() => this.GetGeometry<Polygon>();

    /// <inheritdoc/>
    public PolygonZ GetPolygonZ() => this.GetGeometry<PolygonZ>();

    /// <inheritdoc/>
    public PolygonM GetPolygonM() => this.GetGeometry<PolygonM>();

    /// <inheritdoc/>
    public PolygonZM GetPolygonZM() => this.GetGeometry<PolygonZM>();

    /// <inheritdoc/>
    public IReadOnlyCollection<Polygon> GetMultiPolygon() => this.GetEnumerableGeometry<Polygon>();

    /// <inheritdoc/>
    public IReadOnlyCollection<PolygonZ> GetMultiPolygonZ() => this.GetEnumerableGeometry<PolygonZ>();

    /// <inheritdoc/>
    public IReadOnlyCollection<PolygonM> GetMultiPolygonM() => this.GetEnumerableGeometry<PolygonM>();

    /// <inheritdoc/>
    public IReadOnlyCollection<PolygonZM> GetMultiPolygonZM() => this.GetEnumerableGeometry<PolygonZM>();

    /// <inheritdoc/>
    public object GetGeometry()
    {
        if (this.stream.Position >= this.stream.Length)
        {
            throw new Data.InsufficientDataException();
        }

        var header = TinyWkbRecordHeader.Read(this.stream);
        return this.ReadGeometry(header);
    }

    /// <inheritdoc/>
    public bool IsNull() => this.stream.Position >= this.stream.Length;

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing && this.stream is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    private static ulong ReadVarUInt64(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[9];
        return VarIntBitConverter.ToUInt64(ReadVarIntData(stream, buffer));
    }

    private static ReadOnlySpan<byte> ReadVarIntData(Stream stream, Span<byte> buffer)
    {
        for (var i = 0; i < buffer.Length; i++)
        {
            buffer[i] = (byte)stream.ReadByte();
            if ((buffer[i] & 0x80) is 0)
            {
                return buffer[..(i + 1)];
            }
        }

        throw new ArgumentOutOfRangeException(nameof(stream));
    }

    private static Point CreatePoint(double[] coordinates) => new(coordinates[0], coordinates[1]);

    private static PointZ CreatePointZ(double[] coordinates) => new(coordinates[0], coordinates[1], coordinates[2]);

    private static PointM CreatePointM(double[] coordinates) => new(coordinates[0], coordinates[1], coordinates[2]);

    private static PointZM CreatePointZM(double[] coordinates) => new(coordinates[0], coordinates[1], coordinates[2], coordinates[3]);

    private IReadOnlyCollection<T> GetEnumerableGeometry<T>() =>
        this.GetGeometry() switch
        {
            IReadOnlyCollection<T> collection => collection,
            IEnumerable<T> enumerable => [..enumerable],
            _ => throw new InvalidGeometryTypeException(),
        };

    private T GetGeometry<T>() => (T)(this.GetGeometry() ?? throw new System.IO.InvalidDataException());

    private object ReadGeometry(TinyWkbRecordHeader header, out IList<long>? idList)
    {
        idList = null;

        if (header.HasSize)
        {
            _ = ReadVarUInt64(this.stream);
        }

        if (header.HasEmptyGeometry)
        {
            return (header.GeometryType, header.HasZ, header.HasM) switch
            {
                (TinyWkbGeometryType.Point, false, false) => Point.Empty,
                (TinyWkbGeometryType.Point, true, false) => PointZ.Empty,
                (TinyWkbGeometryType.Point, false, true) => PointM.Empty,
                (TinyWkbGeometryType.Point, true, true) => PointZM.Empty,
                (TinyWkbGeometryType.Linestring, false, false) => new Polyline(),
                (TinyWkbGeometryType.Linestring, true, false) => new PolylineZ(),
                (TinyWkbGeometryType.Linestring, false, true) => new PolylineM(),
                (TinyWkbGeometryType.Linestring, true, true) => new PolylineZM(),
                (TinyWkbGeometryType.Polygon, false, false) => new Polygon(),
                (TinyWkbGeometryType.Polygon, true, false) => new PolygonZ(),
                (TinyWkbGeometryType.Polygon, false, true) => new PolygonM(),
                (TinyWkbGeometryType.Polygon, true, true) => new PolygonZM(),

                (TinyWkbGeometryType.MultiPoint, false, false) => System.Linq.Enumerable.Empty<Point>(),
                (TinyWkbGeometryType.MultiPoint, true, false) => System.Linq.Enumerable.Empty<PointZ>(),
                (TinyWkbGeometryType.MultiPoint, false, true) => System.Linq.Enumerable.Empty<PointM>(),
                (TinyWkbGeometryType.MultiPoint, true, true) => System.Linq.Enumerable.Empty<PointZM>(),

                (TinyWkbGeometryType.MultiLinestring, false, false) => System.Linq.Enumerable.Empty<Polyline>(),
                (TinyWkbGeometryType.MultiLinestring, true, false) => System.Linq.Enumerable.Empty<PolylineZ>(),
                (TinyWkbGeometryType.MultiLinestring, false, true) => System.Linq.Enumerable.Empty<PolylineM>(),
                (TinyWkbGeometryType.MultiLinestring, true, true) => System.Linq.Enumerable.Empty<PolylineZM>(),
                (TinyWkbGeometryType.MultiPolygon, false, false) => System.Linq.Enumerable.Empty<Polygon>(),
                (TinyWkbGeometryType.MultiPolygon, true, false) => System.Linq.Enumerable.Empty<PolygonZ>(),
                (TinyWkbGeometryType.MultiPolygon, false, true) => System.Linq.Enumerable.Empty<PolygonM>(),
                (TinyWkbGeometryType.MultiPolygon, true, true) => System.Linq.Enumerable.Empty<PolygonZM>(),

                (TinyWkbGeometryType.GeometryCollection, _, _) => System.Linq.Enumerable.Empty<object>(),

                _ => throw new InvalidGeometryTypeException(),
            };
        }

        var internalReader = new Reader(this.stream, header);

        if (header.HasBoundingBox)
        {
            _ = internalReader.ReadBoundingBox();
        }

        return (header.GeometryType, header.HasZ, header.HasM) switch
        {
            (TinyWkbGeometryType.Point, false, false) => internalReader.ReadPoints(1, CreatePoint).SingleOrDefault(),
            (TinyWkbGeometryType.Point, true, false) => internalReader.ReadPoints(1, CreatePointZ).SingleOrDefault(),
            (TinyWkbGeometryType.Point, false, true) => internalReader.ReadPoints(1, CreatePointM).SingleOrDefault(),
            (TinyWkbGeometryType.Point, true, true) => internalReader.ReadPoints(1, CreatePointZM).SingleOrDefault(),
            (TinyWkbGeometryType.MultiPoint, false, false) => internalReader.ReadPoints(out idList, header.HasIdList, CreatePoint),
            (TinyWkbGeometryType.MultiPoint, true, false) => internalReader.ReadPoints(out idList, header.HasIdList, CreatePointZ),
            (TinyWkbGeometryType.MultiPoint, false, true) => internalReader.ReadPoints(out idList, header.HasIdList, CreatePointM),
            (TinyWkbGeometryType.MultiPoint, true, true) => internalReader.ReadPoints(out idList, header.HasIdList, CreatePointZM),

            (TinyWkbGeometryType.Linestring, false, false) => new Polyline(internalReader.ReadPoints(CreatePoint)),
            (TinyWkbGeometryType.Linestring, true, false) => new PolylineZ(internalReader.ReadPoints(CreatePointZ)),
            (TinyWkbGeometryType.Linestring, false, true) => new PolylineM(internalReader.ReadPoints(CreatePointM)),
            (TinyWkbGeometryType.Linestring, true, true) => new PolylineZM(internalReader.ReadPoints(CreatePointZM)),
            (TinyWkbGeometryType.MultiLinestring, false, false) => ReadMultiLineString(internalReader, header.HasIdList, CreatePoint).Select(p => new Polyline(p)).ToArray(),
            (TinyWkbGeometryType.MultiLinestring, true, false) => ReadMultiLineString(internalReader, header.HasIdList, CreatePointZ).Select(p => new PolylineZ(p)).ToArray(),
            (TinyWkbGeometryType.MultiLinestring, false, true) => ReadMultiLineString(internalReader, header.HasIdList, CreatePointM).Select(p => new PolylineM(p)).ToArray(),
            (TinyWkbGeometryType.MultiLinestring, true, true) => ReadMultiLineString(internalReader, header.HasIdList, CreatePointZM).Select(p => new PolylineZM(p)).ToArray(),

            (TinyWkbGeometryType.Polygon, false, false) => new Polygon(ReadPolygon(internalReader, CreatePoint)),
            (TinyWkbGeometryType.Polygon, true, false) => new PolygonZ(ReadPolygon(internalReader, CreatePointZ)),
            (TinyWkbGeometryType.Polygon, false, true) => new PolygonM(ReadPolygon(internalReader, CreatePointM)),
            (TinyWkbGeometryType.Polygon, true, true) => new PolygonZM(ReadPolygon(internalReader, CreatePointZM)),
            (TinyWkbGeometryType.MultiPolygon, false, false) => ReadMultiPolygon(internalReader, header.HasIdList, CreatePoint).Select(p => new Polygon(p)).ToArray(),
            (TinyWkbGeometryType.MultiPolygon, true, false) => ReadMultiPolygon(internalReader, header.HasIdList, CreatePointZ).Select(p => new PolygonZ(p)).ToArray(),
            (TinyWkbGeometryType.MultiPolygon, false, true) => ReadMultiPolygon(internalReader, header.HasIdList, CreatePointM).Select(p => new PolygonM(p)).ToArray(),
            (TinyWkbGeometryType.MultiPolygon, true, true) => ReadMultiPolygon(internalReader, header.HasIdList, CreatePointZM).Select(p => new PolygonZM(p)).ToArray(),

            (TinyWkbGeometryType.GeometryCollection, _, _) => ReadGeometryCollection(internalReader, header.HasIdList),

            _ => throw new InvalidGeometryTypeException(),
        };

        static LinearRing<T>[] ReadPolygon<T>(Reader reader, Func<double[], T> pointCreator)
            where T : struct
        {
            var ringCount = reader.ReadCount();
            var rings = new LinearRing<T>[ringCount];

            for (var i = 0; i < rings.Length; i++)
            {
                var points = reader.ReadPoints(pointCreator).ToList();
                rings[i] = new(points);
            }

            return rings;
        }

        static T[][] ReadMultiLineString<T>(Reader reader, bool readIdList, Func<double[], T> pointCreator)
        {
            var lineStringCount = reader.ReadCount();
            if (readIdList)
            {
                _ = reader.ReadIdList(lineStringCount);
            }

            var lineStrings = new T[lineStringCount][];

            for (var i = 0; i < lineStrings.Length; i++)
            {
                var points = reader.ReadPoints(pointCreator);
                lineStrings[i] = points;
            }

            return lineStrings;
        }

        static LinearRing<T>[][] ReadMultiPolygon<T>(Reader reader, bool readIdList, Func<double[], T> pointCreator)
            where T : struct
        {
            var polygonCount = reader.ReadCount();
            if (readIdList)
            {
                _ = reader.ReadIdList(polygonCount);
            }

            return ReadPolygons(reader, (int)polygonCount, pointCreator).ToArray();

            static IEnumerable<LinearRing<T>[]> ReadPolygons(Reader internalReader, int count, Func<double[], T> pointCreator)
            {
                for (var i = 0; i < count; i++)
                {
                    yield return ReadPolygon(internalReader, pointCreator);
                }
            }
        }

        IEnumerable<object?> ReadGeometryCollection(Reader reader, bool readIdList)
        {
            var geometryCount = reader.ReadCount();
            if (readIdList)
            {
                _ = reader.ReadIdList(geometryCount);
            }

            for (uint i = 0; i < geometryCount; i++)
            {
                yield return this.GetGeometry();
            }
        }
    }

    private object ReadGeometry(TinyWkbRecordHeader header) => this.ReadGeometry(header, out _);

    private sealed class Reader
    {
        private readonly Stream stream;

        private readonly int precisionXY;
        private readonly double descaleXY;

        private readonly bool hasZ;
        private readonly int precisionZ;
        private readonly double descaleZ;

        private readonly bool hasM;
        private readonly int precisionM;
        private readonly double descaleM;

        private readonly long[] previousCoordinates;

        public Reader(Stream stream, TinyWkbRecordHeader header)
        {
            this.stream = stream;

            this.precisionXY = header.PrecisionXY;
            this.descaleXY = this.precisionXY.Descale();

            this.hasZ = header.HasZ;
            this.precisionZ = this.hasZ ? header.PrecisionZ : 0;
            this.descaleZ = this.precisionZ.Descale();

            this.hasM = header.HasM;
            this.precisionM = this.hasM ? header.PrecisionM : 0;
            this.descaleM = this.precisionM.Descale();

            var coordinateCount = 2 + (this.hasZ ? 1 : 0) + (this.hasM ? 1 : 0);
            this.previousCoordinates = new long[coordinateCount];
        }

        public ulong ReadCount() => ReadVarUInt64(this.stream);

        public object ReadBoundingBox()
        {
            var (x, width) = (ReadVarInt64(this.stream), ReadVarInt64(this.stream));
            var (y, height) = (ReadVarInt64(this.stream), ReadVarInt64(this.stream));
            var (z, depth) = this.hasZ
                ? (ReadVarInt64(this.stream), ReadVarInt64(this.stream))
                : (0L, 0L);

            var (m, length) = this.hasM
                ? (ReadVarInt64(this.stream), ReadVarInt64(this.stream))
                : (0L, 0L);

            return (this.hasZ, this.hasM) switch
            {
                (false, false) => Envelope.FromXYWH(this.GetValueXY(x), this.GetValueXY(y), this.GetValueXY(width), this.GetValueXY(height)),
                (true, false) => EnvelopeZ.FromXYZWHD(this.GetValueXY(x), this.GetValueXY(y), this.GetValueZ(z), this.GetValueXY(width), this.GetValueXY(height), this.GetValueZ(depth)),
                (false, true) => EnvelopeM.FromXYMWHL(this.GetValueXY(x), this.GetValueXY(y), this.GetValueM(m), this.GetValueXY(width), this.GetValueXY(height), this.GetValueM(length)),
                (true, true) => EnvelopeZM.FromXYZMWHDL(this.GetValueXY(x), this.GetValueXY(y), this.GetValueZ(z), this.GetValueM(m), this.GetValueXY(width), this.GetValueXY(height), this.GetValueZ(depth), this.GetValueM(length)),
            };
        }

        public T[] ReadPoints<T>(Func<double[], T> creator) => this.ReadPoints(this.ReadCount(), creator);

        public T[] ReadPoints<T>(ulong count, Func<double[], T> creator) => this.ReadPoints(readIdList: false, count, creator);

        public T[] ReadPoints<T>(out IList<long>? idList, bool readIdList, Func<double[], T> creator) => this.ReadPoints(out idList, readIdList, this.ReadCount(), creator);

        public long[] ReadIdList(ulong count)
        {
            var previousId = 0L;
            var idList = new long[count];
            for (var i = 0; i < idList.Length; i++)
            {
                idList[i] = previousId += ReadVarInt64(this.stream);
            }

            return idList;
        }

        private static long ReadVarInt64(Stream stream)
        {
            Span<byte> buffer = stackalloc byte[9];
            return VarIntBitConverter.ToInt64(ReadVarIntData(stream, buffer));
        }

        private static double GetValue(long input, double descale, int precision) => System.Math.Round(input * descale, precision, MidpointRounding.AwayFromZero);

        private double GetValueXY(long input) => GetValue(input, this.descaleXY, this.precisionXY);

        private double GetValueZ(long input) => GetValue(input, this.descaleZ, this.precisionZ);

        private double GetValueM(long input) => GetValue(input, this.descaleM, this.precisionM);

        private T[] ReadPoints<T>(bool readIdList, ulong count, Func<double[], T> creator) => this.ReadPoints(out _, readIdList, count, creator);

        private T[] ReadPoints<T>(out IList<long>? idList, bool readIdList, ulong count, Func<double[], T> creator)
        {
            if (count is 0)
            {
                idList = null;
                return [];
            }

            // read the ID list
            idList = readIdList ? this.ReadIdList(count) : [];

            var points = new T[count];

            for (var i = 0; i < points.Length; i++)
            {
                for (var j = 0; j < this.previousCoordinates.Length; j++)
                {
                    this.previousCoordinates[j] += ReadVarInt64(this.stream);
                }

                points[i] = (this.hasZ, this.hasM) switch
                {
                    (false, false) => creator([this.GetValueXY(this.previousCoordinates[0]), this.GetValueXY(this.previousCoordinates[1])]),
                    (true, false) => creator([this.GetValueXY(this.previousCoordinates[0]), this.GetValueXY(this.previousCoordinates[1]), this.GetValueZ(this.previousCoordinates[2])]),
                    (false, true) => creator([this.GetValueXY(this.previousCoordinates[0]), this.GetValueXY(this.previousCoordinates[1]), this.GetValueM(this.previousCoordinates[2])]),
                    (true, true) => creator([this.GetValueXY(this.previousCoordinates[0]), this.GetValueXY(this.previousCoordinates[1]), this.GetValueZ(this.previousCoordinates[2]), this.GetValueM(this.previousCoordinates[3])]),
                };
            }

            return points;
        }
    }
}