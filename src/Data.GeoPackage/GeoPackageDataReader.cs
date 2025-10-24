// -----------------------------------------------------------------------
// <copyright file="GeoPackageDataReader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data.GeoPackage;

/// <summary>
/// The <c>GeoPackage</c> data reader.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1010:Generic interface should also be implemented", Justification = "The generic version is IEnumerable<object>")]
public class GeoPackageDataReader : System.Data.Common.DbDataReader, IGeometryDataRecord
{
    private readonly Microsoft.Data.Sqlite.SqliteDataReader reader;
    private readonly Buffers.Binary.WkbPrimitives.WkbGeometryType type;

    /// <summary>
    /// Initialises a new instance of the <see cref="GeoPackageDataReader"/> class.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="type">The geometry type.</param>
    internal GeoPackageDataReader(Microsoft.Data.Sqlite.SqliteDataReader reader, Buffers.Binary.WkbPrimitives.WkbGeometryType type)
    {
        this.reader = reader;
        this.type = type;
    }

    private delegate T CreateFunction<out T>(ReadOnlySpan<byte> span);

    /// <inheritdoc />
    public override int FieldCount => this.reader.FieldCount;

    /// <inheritdoc />
    public override int RecordsAffected => this.reader.RecordsAffected;

    /// <inheritdoc />
    public override bool HasRows => this.reader.HasRows;

    /// <inheritdoc />
    public override bool IsClosed => this.reader.IsClosed;

    /// <inheritdoc />
    public override int Depth => this.reader.Depth;

    /// <inheritdoc />
    public override object this[int ordinal] => this.reader[ordinal];

    /// <inheritdoc />
    public override object this[string name] => this.reader[name];

    /// <inheritdoc />
    public override bool GetBoolean(int ordinal) => this.reader.GetBoolean(ordinal);

    /// <inheritdoc />
    public override byte GetByte(int ordinal) => this.reader.GetByte(ordinal);

    /// <inheritdoc />
    public override long GetBytes(int ordinal, long dataOffset, byte[]? buffer, int bufferOffset, int length) => this.reader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);

    /// <inheritdoc />
    public override char GetChar(int ordinal) => this.reader.GetChar(ordinal);

    /// <inheritdoc />
    public override long GetChars(int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length) => this.reader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);

    /// <inheritdoc />
    public override string GetDataTypeName(int ordinal) => this.reader.GetDataTypeName(ordinal);

    /// <inheritdoc />
    public override DateTime GetDateTime(int ordinal) => this.reader.GetDateTime(ordinal);

    /// <inheritdoc />
    public override decimal GetDecimal(int ordinal) => this.reader.GetDecimal(ordinal);

    /// <inheritdoc />
    public override double GetDouble(int ordinal) => this.reader.GetDouble(ordinal);

    /// <inheritdoc />
    public override Type GetFieldType(int ordinal) => this.reader.GetFieldType(ordinal);

    /// <inheritdoc />
    public override float GetFloat(int ordinal) => this.reader.GetFloat(ordinal);

    /// <inheritdoc />
    public override Guid GetGuid(int ordinal) => this.reader.GetGuid(ordinal);

    /// <inheritdoc />
    public override short GetInt16(int ordinal) => this.reader.GetInt16(ordinal);

    /// <inheritdoc />
    public override int GetInt32(int ordinal) => this.reader.GetInt32(ordinal);

    /// <inheritdoc />
    public override long GetInt64(int ordinal) => this.reader.GetInt64(ordinal);

    /// <inheritdoc />
    public override string GetName(int ordinal) => this.reader.GetName(ordinal);

    /// <inheritdoc />
    public override int GetOrdinal(string name) => this.reader.GetOrdinal(name);

    /// <inheritdoc />
    public override string GetString(int ordinal) => this.reader.GetString(ordinal);

    /// <inheritdoc />
    public override object GetValue(int ordinal) => this.reader.GetValue(ordinal);

    /// <inheritdoc />
    public override int GetValues(object[] values)
    {
        var count = this.reader.GetValues(values);
        for (var i = 0; i < count; i++)
        {
            if (values[i] is not byte[] bytes)
            {
                continue;
            }

            var span = bytes.AsSpan();
            var header = ReadHeader(span);
            if (header.Successful)
            {
                values[i] = header.Empty
                    ? default(EmptyGeometry)
                    : GetGeometry(span[header.Size..], this.type);
            }
        }

        return count;
    }

    /// <inheritdoc />
    public override bool IsDBNull(int ordinal) => this.reader.IsDBNull(ordinal);

    /// <inheritdoc />
    public override bool NextResult() => this.reader.NextResult();

    /// <inheritdoc />
    public override bool Read() => this.reader.Read();

    /// <inheritdoc />
    public override System.Collections.IEnumerator GetEnumerator() => this.reader.GetEnumerator();

    /// <inheritdoc/>
    public Geometry.Point GetPoint(int i) => this.GetGeometry(i, Buffers.Binary.WkbPrimitives.WkbGeometryType.Point, Buffers.Binary.WkbPrimitives.ReadPoint);

    /// <inheritdoc/>
    public Geometry.PointZ GetPointZ(int i) => this.GetGeometry(i, Buffers.Binary.WkbPrimitives.WkbGeometryType.PointZ, Buffers.Binary.WkbPrimitives.ReadPointZ);

    /// <inheritdoc/>
    public Geometry.PointM GetPointM(int i) => this.GetGeometry(i, Buffers.Binary.WkbPrimitives.WkbGeometryType.PointM, Buffers.Binary.WkbPrimitives.ReadPointM);

    /// <inheritdoc/>
    public Geometry.PointZM GetPointZM(int i) => this.GetGeometry(i, Buffers.Binary.WkbPrimitives.WkbGeometryType.PointZM, Buffers.Binary.WkbPrimitives.ReadPointZM);

    /// <inheritdoc/>
    public Geometry.IMultiGeometry<Geometry.Point> GetMultiPoint(int i) => this.GetGeometry(i, Buffers.Binary.WkbPrimitives.WkbGeometryType.MultiPoint, Buffers.Binary.WkbPrimitives.ReadMultiPoint);

    /// <inheritdoc/>
    public Geometry.IMultiGeometry<Geometry.PointZ> GetMultiPointZ(int i) => this.GetGeometry(i, Buffers.Binary.WkbPrimitives.WkbGeometryType.MultiPointZ, Buffers.Binary.WkbPrimitives.ReadMultiPointZ);

    /// <inheritdoc/>
    public Geometry.IMultiGeometry<Geometry.PointM> GetMultiPointM(int i) => this.GetGeometry(i, Buffers.Binary.WkbPrimitives.WkbGeometryType.MultiPointM, Buffers.Binary.WkbPrimitives.ReadMultiPointM);

    /// <inheritdoc/>
    public Geometry.IMultiGeometry<Geometry.PointZM> GetMultiPointZM(int i) => this.GetGeometry(i, Buffers.Binary.WkbPrimitives.WkbGeometryType.MultiPointZM, Buffers.Binary.WkbPrimitives.ReadMultiPointZM);

    /// <inheritdoc/>
    public Geometry.Polyline GetLineString(int i) => this.GetGeometry(i, Buffers.Binary.WkbPrimitives.WkbGeometryType.LineString, Buffers.Binary.WkbPrimitives.ReadLineString);

    /// <inheritdoc/>
    public Geometry.PolylineZ GetLineStringZ(int i) => this.GetGeometry(i, Buffers.Binary.WkbPrimitives.WkbGeometryType.LineStringZ, Buffers.Binary.WkbPrimitives.ReadLineStringZ);

    /// <inheritdoc/>
    public Geometry.PolylineM GetLineStringM(int i) => this.GetGeometry(i, Buffers.Binary.WkbPrimitives.WkbGeometryType.LineStringM, Buffers.Binary.WkbPrimitives.ReadLineStringM);

    /// <inheritdoc/>
    public Geometry.PolylineZM GetLineStringZM(int i) => this.GetGeometry(i, Buffers.Binary.WkbPrimitives.WkbGeometryType.LineStringZM, Buffers.Binary.WkbPrimitives.ReadLineStringZM);

    /// <inheritdoc/>
    public Geometry.IMultiGeometry<Geometry.Polyline> GetMultiLineString(int i) => this.GetGeometry(i, Buffers.Binary.WkbPrimitives.WkbGeometryType.MultiLineString, Buffers.Binary.WkbPrimitives.ReadMultiLineString);

    /// <inheritdoc/>
    public Geometry.IMultiGeometry<Geometry.PolylineZ> GetMultiLineStringZ(int i) => this.GetGeometry(i, Buffers.Binary.WkbPrimitives.WkbGeometryType.MultiLineStringZ, Buffers.Binary.WkbPrimitives.ReadMultiLineStringZ);

    /// <inheritdoc/>
    public Geometry.IMultiGeometry<Geometry.PolylineM> GetMultiLineStringM(int i) => this.GetGeometry(i, Buffers.Binary.WkbPrimitives.WkbGeometryType.MultiLineStringM, Buffers.Binary.WkbPrimitives.ReadMultiLineStringM);

    /// <inheritdoc/>
    public Geometry.IMultiGeometry<Geometry.PolylineZM> GetMultiLineStringZM(int i) => this.GetGeometry(i, Buffers.Binary.WkbPrimitives.WkbGeometryType.MultiLineStringZM, Buffers.Binary.WkbPrimitives.ReadMultiLineStringZM);

    /// <inheritdoc/>
    public Geometry.Polygon GetPolygon(int i) => this.GetGeometry(i, Buffers.Binary.WkbPrimitives.WkbGeometryType.Polygon, Buffers.Binary.WkbPrimitives.ReadPolygon);

    /// <inheritdoc/>
    public Geometry.PolygonZ GetPolygonZ(int i) => this.GetGeometry(i, Buffers.Binary.WkbPrimitives.WkbGeometryType.PolygonZ, Buffers.Binary.WkbPrimitives.ReadPolygonZ);

    /// <inheritdoc/>
    public Geometry.PolygonM GetPolygonM(int i) => this.GetGeometry(i, Buffers.Binary.WkbPrimitives.WkbGeometryType.PolygonM, Buffers.Binary.WkbPrimitives.ReadPolygonM);

    /// <inheritdoc/>
    public Geometry.PolygonZM GetPolygonZM(int i) => this.GetGeometry(i, Buffers.Binary.WkbPrimitives.WkbGeometryType.PolygonZM, Buffers.Binary.WkbPrimitives.ReadPolygonZM);

    /// <inheritdoc/>
    public Geometry.IMultiGeometry<Geometry.Polygon> GetMultiPolygon(int i) => this.GetGeometry(i, Buffers.Binary.WkbPrimitives.WkbGeometryType.MultiPolygon, Buffers.Binary.WkbPrimitives.ReadMultiPolygon);

    /// <inheritdoc/>
    public Geometry.IMultiGeometry<Geometry.PolygonZ> GetMultiPolygonZ(int i) => this.GetGeometry(i, Buffers.Binary.WkbPrimitives.WkbGeometryType.MultiPolygonZ, Buffers.Binary.WkbPrimitives.ReadMultiPolygonZ);

    /// <inheritdoc/>
    public Geometry.IMultiGeometry<Geometry.PolygonM> GetMultiPolygonM(int i) => this.GetGeometry(i, Buffers.Binary.WkbPrimitives.WkbGeometryType.MultiPolygonM, Buffers.Binary.WkbPrimitives.ReadMultiPolygonM);

    /// <inheritdoc/>
    public Geometry.IMultiGeometry<Geometry.PolygonZM> GetMultiPolygonZM(int i) => this.GetGeometry(i, Buffers.Binary.WkbPrimitives.WkbGeometryType.MultiPolygonZM, Buffers.Binary.WkbPrimitives.ReadMultiPolygonZM);

    /// <inheritdoc />
    public Geometry.IGeometry GetGeometry(int i)
    {
        var buffer = this.GetBytes(i);

        var (successful, _, empty, _, _, size) = ReadHeader(buffer);
        if (!successful)
        {
            throw new InvalidDataException();
        }

        if (empty)
        {
            return default(EmptyGeometry);
        }

        return GetGeometry(buffer.AsSpan(size), this.type);
    }

    /// <summary>
    /// Reads the GeoPackage blob header.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <returns>The result.</returns>
    internal static (bool Successful, int Srid, bool Empty, bool LittleEndian, double[] Envelope, int Size) ReadHeader(ReadOnlySpan<byte> span)
    {
        if (span.Length < 45)
        {
            // cannot be an internal BLOB WKB geometry
            return default;
        }

        if (span is not [(byte)'G', (byte)'P', 0, var flags, ..])
        {
            return default;
        }

        var empty = (flags & (0x01 << 4)) is not 0;
        ////var extended = (flags & (0x01 << 5)) is not 0;
        var littleEndian = (flags & 0x01) is not 0;
        int envelopeFlags = (flags & (0x07 << 1)) >> 1;
        var hasXY = envelopeFlags is not 0;
        var hasZ = envelopeFlags is 2 or 4;
        var hasM = envelopeFlags is 3 or 4;

        var dimensions = (hasXY, hasZ, hasM) switch
        {
            (true, false, false) => 2,
            (true, true, false) or (true, false, true) => 3,
            (true, true, true) => 4,
            (false, false, false) => 0,
            _ => throw new InvalidOperationException(),
        };

        var envelope = new double[dimensions * 2];

        var srid = ReadInt32(span[4..], littleEndian);

        span = span[8..];
        var index = 0;
        if (hasXY)
        {
            envelope[0] = ReadDouble(span[..8], littleEndian);
            envelope[1] = ReadDouble(span[8..16], littleEndian);
            envelope[2] = ReadDouble(span[16..24], littleEndian);
            envelope[3] = ReadDouble(span[24..32], littleEndian);
            span = span[32..];
            index += 4;
        }

        if (hasZ)
        {
            envelope[index] = ReadDouble(span[..8], littleEndian);
            envelope[index + 1] = ReadDouble(span[8..16], littleEndian);
            span = span[16..];
            index += 2;
        }

        if (hasM)
        {
            envelope[index] = ReadDouble(span[..8], littleEndian);
            envelope[index + 1] = ReadDouble(span[8..16], littleEndian);
        }

        var size = 8 + (dimensions * 2 * sizeof(double));
        return (Successful: true, srid, empty, littleEndian, envelope, size);
    }

    private static double ReadDouble(ReadOnlySpan<byte> span, bool littleEndian) =>
        BitConverter.Int64BitsToDouble(littleEndian ? System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(span) : System.Buffers.Binary.BinaryPrimitives.ReadInt64BigEndian(span));

    private static int ReadInt32(ReadOnlySpan<byte> span, bool littleEndian) => littleEndian ? System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span) : System.Buffers.Binary.BinaryPrimitives.ReadInt32BigEndian(span);

    private static void CheckType(Buffers.Binary.WkbPrimitives.WkbGeometryType actual, Buffers.Binary.WkbPrimitives.WkbGeometryType expected)
    {
        if (actual != expected)
        {
            throw new Geometry.InvalidGeometryTypeException();
        }
    }

    private static Geometry.IGeometry GetGeometry(Span<byte> span, Buffers.Binary.WkbPrimitives.WkbGeometryType type) =>
        type switch
        {
            Buffers.Binary.WkbPrimitives.WkbGeometryType.Point => Buffers.Binary.WkbPrimitives.ReadPoint(span),
            Buffers.Binary.WkbPrimitives.WkbGeometryType.PointZ => Buffers.Binary.WkbPrimitives.ReadPointZ(span),
            Buffers.Binary.WkbPrimitives.WkbGeometryType.PointM => Buffers.Binary.WkbPrimitives.ReadPointM(span),
            Buffers.Binary.WkbPrimitives.WkbGeometryType.PointZM => Buffers.Binary.WkbPrimitives.ReadPointZM(span),
            Buffers.Binary.WkbPrimitives.WkbGeometryType.MultiPoint => Buffers.Binary.WkbPrimitives.ReadMultiPoint(span),
            Buffers.Binary.WkbPrimitives.WkbGeometryType.MultiPointZ => Buffers.Binary.WkbPrimitives.ReadMultiPointZ(span),
            Buffers.Binary.WkbPrimitives.WkbGeometryType.MultiPointM => Buffers.Binary.WkbPrimitives.ReadMultiPointM(span),
            Buffers.Binary.WkbPrimitives.WkbGeometryType.MultiPointZM => Buffers.Binary.WkbPrimitives.ReadMultiPointZM(span),
            Buffers.Binary.WkbPrimitives.WkbGeometryType.LineString => Buffers.Binary.WkbPrimitives.ReadLineString(span),
            Buffers.Binary.WkbPrimitives.WkbGeometryType.LineStringZ => Buffers.Binary.WkbPrimitives.ReadLineStringZ(span),
            Buffers.Binary.WkbPrimitives.WkbGeometryType.LineStringM => Buffers.Binary.WkbPrimitives.ReadLineStringM(span),
            Buffers.Binary.WkbPrimitives.WkbGeometryType.LineStringZM => Buffers.Binary.WkbPrimitives.ReadLineStringZM(span),
            Buffers.Binary.WkbPrimitives.WkbGeometryType.MultiLineString => Buffers.Binary.WkbPrimitives.ReadMultiLineString(span),
            Buffers.Binary.WkbPrimitives.WkbGeometryType.MultiLineStringZ => Buffers.Binary.WkbPrimitives.ReadMultiLineStringZ(span),
            Buffers.Binary.WkbPrimitives.WkbGeometryType.MultiLineStringM => Buffers.Binary.WkbPrimitives.ReadMultiLineStringM(span),
            Buffers.Binary.WkbPrimitives.WkbGeometryType.MultiLineStringZM => Buffers.Binary.WkbPrimitives.ReadMultiLineStringZM(span),
            Buffers.Binary.WkbPrimitives.WkbGeometryType.Polygon => Buffers.Binary.WkbPrimitives.ReadPolygon(span),
            Buffers.Binary.WkbPrimitives.WkbGeometryType.PolygonZ => Buffers.Binary.WkbPrimitives.ReadPolygonZ(span),
            Buffers.Binary.WkbPrimitives.WkbGeometryType.PolygonM => Buffers.Binary.WkbPrimitives.ReadPolygonM(span),
            Buffers.Binary.WkbPrimitives.WkbGeometryType.PolygonZM => Buffers.Binary.WkbPrimitives.ReadPolygonZM(span),
            Buffers.Binary.WkbPrimitives.WkbGeometryType.MultiPolygon => Buffers.Binary.WkbPrimitives.ReadMultiPolygon(span),
            Buffers.Binary.WkbPrimitives.WkbGeometryType.MultiPolygonZ => Buffers.Binary.WkbPrimitives.ReadMultiPolygonZ(span),
            Buffers.Binary.WkbPrimitives.WkbGeometryType.MultiPolygonM => Buffers.Binary.WkbPrimitives.ReadMultiPolygonM(span),
            Buffers.Binary.WkbPrimitives.WkbGeometryType.MultiPolygonZM => Buffers.Binary.WkbPrimitives.ReadMultiPolygonZM(span),
            _ => throw new Geometry.InvalidGeometryTypeException(),
        };

    private T GetGeometry<T>(int i, Buffers.Binary.WkbPrimitives.WkbGeometryType expected, CreateFunction<T> func) => this.GetGeometry(this.GetBytes(i), expected, func);

    private T GetGeometry<T>(ReadOnlySpan<byte> span, Buffers.Binary.WkbPrimitives.WkbGeometryType expected, CreateFunction<T> func)
    {
        CheckType(this.type, expected);
        var (successful, _, _, _, _, size) = ReadHeader(span);
        return successful ? func(span[size..]) : throw new InvalidDataException();
    }

    private byte[] GetBytes(int i)
    {
        var stream = this.reader.GetStream(i);
        var buffer = new byte[stream.Length];
        stream.Position = 0;
        return stream.Read(buffer, 0, buffer.Length) == buffer.Length ? buffer : throw new InsufficientDataException();
    }

    private readonly struct EmptyGeometry : Geometry.IGeometry
    {
        /// <inheritdoc/>
        double Geometry.IGeometry.MinX() => default;

        /// <inheritdoc/>
        double Geometry.IGeometry.MaxX() => default;

        /// <inheritdoc/>
        double Geometry.IGeometry.MinY() => default;

        /// <inheritdoc/>
        double Geometry.IGeometry.MaxY() => default;
    }
}