// -----------------------------------------------------------------------
// <copyright file="MapInfoRecord.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.MapInfo;

using Altemiq.Geometry;

/// <summary>
/// The <see cref="MapInfo"/> <see cref="Data.IGeometryDataRecord"/>.
/// </summary>
/// <param name="fields">The fields.</param>
/// <param name="mapRecord">The <see cref="MapInfo"/> record.</param>
/// <param name="dbfRecord">The <see cref="Data.Dbf"/> record.</param>
public class MapInfoRecord(IReadOnlyList<TabField> fields, MapRecord? mapRecord, Data.Dbf.DbfRecord dbfRecord) : Data.IGeometryRecord, System.Data.IDataRecord, IDisposable
{
    private readonly TabTableType tableType;

    private bool disposedValue;

    /// <summary>
    /// Initialises a new instance of the <see cref="MapInfoRecord"/> class.
    /// </summary>
    /// <param name="fields">The fields.</param>
    /// <param name="mapRecord">The <see cref="MapInfo"/> record.</param>
    /// <param name="dbfRecord">The <see cref="Data.Dbf"/> record.</param>
    /// <param name="tableType">The table type.</param>
    internal MapInfoRecord(IReadOnlyList<TabField> fields, MapRecord? mapRecord, Data.Dbf.DbfRecord dbfRecord, TabTableType tableType)
        : this(fields, mapRecord, dbfRecord) => this.tableType = tableType;

    /// <summary>
    /// Gets the feature ID.
    /// </summary>
    public int FeatureId => mapRecord?.FeatureId ?? default;

    /// <inheritdoc/>
    public int FieldCount => fields.Count;

    /// <inheritdoc/>
    public object this[string name] => this.GetValue(dbfRecord.GetOrdinal(name));

    /// <inheritdoc/>
    public object this[int i] => this.GetValue(i);

    /// <inheritdoc/>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    public bool GetBoolean(int i) => this.tableType is TabTableType.Native ? dbfRecord.GetSpan(i)[0] is not 0 : dbfRecord.GetBoolean(i);

    /// <inheritdoc/>
    public byte GetByte(int i) => dbfRecord.GetByte(i);

    /// <inheritdoc/>
    public long GetBytes(int i, long fieldOffset, byte[]? buffer, int bufferoffset, int length) => dbfRecord.GetBytes(i, fieldOffset, buffer, bufferoffset, length);

    /// <inheritdoc/>
    public char GetChar(int i) => dbfRecord.GetChar(i);

    /// <inheritdoc/>
    public long GetChars(int i, long fieldoffset, char[]? buffer, int bufferoffset, int length) => dbfRecord.GetChars(i, fieldoffset, buffer, bufferoffset, length);

    /// <inheritdoc/>
    public System.Data.IDataReader GetData(int i) => dbfRecord.GetData(i);

    /// <inheritdoc/>
    public string GetDataTypeName(int i) => fields[i].Type.ToString();

    /// <inheritdoc/>
    public DateTime GetDateTime(int i)
    {
        var field = fields[i];
        if (this.tableType is TabTableType.Native)
        {
            if (field.Type is TabFieldType.Date)
            {
                var span = dbfRecord.GetSpan(i);
                var year = System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(span[..sizeof(short)]);
                var month = span[sizeof(short)];
                var day = span[sizeof(short) + 1];

                return new(year, month, day, default, default, default, DateTimeKind.Unspecified);
            }

            if (field.Type is TabFieldType.Time)
            {
                var milliseconds = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(dbfRecord.GetSpan(i));
                return default(DateTime).AddMilliseconds(milliseconds);
            }

            if (field.Type is TabFieldType.DateTime)
            {
                var span = dbfRecord.GetSpan(i);
                var year = System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(span[..sizeof(short)]);
                var month = span[sizeof(short)];
                var day = span[sizeof(short) + 1];
                var milliseconds = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[(sizeof(short) + 2)..]);

                return new DateTime(year, month, day, default, default, default, DateTimeKind.Unspecified).AddMilliseconds(milliseconds);
            }
        }
        else if (this.tableType is TabTableType.DBF)
        {
            if (field.Type is TabFieldType.Date)
            {
                return dbfRecord.GetDateTime(i);
            }

            if (field.Type is TabFieldType.Time)
            {
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
                var time = dbfRecord.GetString(i).AsSpan();
#else
                var time = dbfRecord.GetString(i);
#endif
                var hours = int.Parse(time[0..2], provider: System.Globalization.CultureInfo.InvariantCulture);
                var minutes = int.Parse(time[2..4], provider: System.Globalization.CultureInfo.InvariantCulture);
                var seconds = int.Parse(time[4..6], provider: System.Globalization.CultureInfo.InvariantCulture);
                var milliseconds = int.Parse(time[6..], provider: System.Globalization.CultureInfo.InvariantCulture);

                var timeSpan = new TimeSpan(0, hours, minutes, seconds, milliseconds);
                return default(DateTime).Add(timeSpan);
            }

            if (field.Type is TabFieldType.DateTime)
            {
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
                var dateTime = dbfRecord.GetString(i).AsSpan();
#else
                var dateTime = dbfRecord.GetString(i);
#endif
                var year = int.Parse(dateTime[0..4], provider: System.Globalization.CultureInfo.InvariantCulture);
                var month = int.Parse(dateTime[4..6], provider: System.Globalization.CultureInfo.InvariantCulture);
                var day = int.Parse(dateTime[4..8], provider: System.Globalization.CultureInfo.InvariantCulture);
                var hour = int.Parse(dateTime[8..10], provider: System.Globalization.CultureInfo.InvariantCulture);
                var minute = int.Parse(dateTime[10..12], provider: System.Globalization.CultureInfo.InvariantCulture);
                var second = int.Parse(dateTime[12..14], provider: System.Globalization.CultureInfo.InvariantCulture);
                var millisecond = int.Parse(dateTime[14..], provider: System.Globalization.CultureInfo.InvariantCulture);

                return new(year, month, day, hour, minute, second, millisecond, DateTimeKind.Unspecified);
            }
        }

        throw new InvalidOperationException();
    }

    /// <inheritdoc/>
    public decimal GetDecimal(int i) => fields[i].Type is TabFieldType.Decimal ? decimal.Parse(dbfRecord.GetString(i), System.Globalization.CultureInfo.InvariantCulture) : throw new InvalidOperationException();

    /// <inheritdoc/>
    public double GetDouble(int i) => this.tableType is TabTableType.Native ? System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(dbfRecord.GetSpan(i)) : dbfRecord.GetDouble(i);

    /// <inheritdoc/>
    public Type GetFieldType(int i) => fields[i].Type switch
    {
        TabFieldType.Unknown => typeof(object),
        TabFieldType.Char => typeof(string),
        TabFieldType.Integer => typeof(int),
        TabFieldType.SmallInt => typeof(short),
        TabFieldType.LargeInt => typeof(long),
        TabFieldType.Decimal => typeof(decimal),
        TabFieldType.Float => typeof(double),
#if NET6_0_OR_GREATER
        TabFieldType.Date => typeof(DateOnly),
        TabFieldType.Time => typeof(TimeOnly),
        TabFieldType.DateTime => typeof(DateTime),
#else
        TabFieldType.Date or TabFieldType.Time or TabFieldType.DateTime => typeof(DateTime),
#endif
        TabFieldType.Logical => typeof(bool),
        _ => throw new InvalidOperationException(),
    };

    /// <inheritdoc/>
    public float GetFloat(int i) => dbfRecord.GetFloat(i);

    /// <inheritdoc/>
    public Guid GetGuid(int i) => dbfRecord.GetGuid(i);

    /// <inheritdoc/>
    public short GetInt16(int i) => this.tableType is TabTableType.Native ? System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(dbfRecord.GetSpan(i)) : dbfRecord.GetInt16(i);

    /// <inheritdoc/>
    public int GetInt32(int i) => this.tableType is TabTableType.Native ? System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(dbfRecord.GetSpan(i)) : dbfRecord.GetInt32(i);

    /// <inheritdoc/>
    public long GetInt64(int i) => this.tableType is TabTableType.Native ? System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(dbfRecord.GetSpan(i)) : dbfRecord.GetInt64(i);

    /// <inheritdoc/>
    public string GetName(int i) => dbfRecord.GetName(i);

    /// <inheritdoc/>
    public int GetOrdinal(string name) => dbfRecord.GetOrdinal(name);

    /// <inheritdoc/>
    public string GetString(int i)
    {
        var value = dbfRecord.GetString(i);
        if (this.tableType is TabTableType.DBF)
        {
            for (var j = value.Length - 1; j >= 0; j--)
            {
                if (value[j] is not ' ')
                {
                    return value[..(j + 1)];
                }
            }

            return string.Empty;
        }

        return value;
    }

    /// <inheritdoc/>
    public object GetValue(int i) => fields[i] switch
    {
        { Type: TabFieldType.SmallInt } => this.GetInt16(i),
        { Type: TabFieldType.Integer } => this.GetInt32(i),
        { Type: TabFieldType.LargeInt } => this.GetInt64(i),
        { Type: TabFieldType.Float } => this.GetDouble(i),
        { Type: TabFieldType.Decimal } => this.GetDecimal(i),
        { Type: TabFieldType.Char } => this.GetString(i),
        { Type: TabFieldType.Logical } => this.GetBoolean(i),
        { Type: TabFieldType.Date or TabFieldType.Time or TabFieldType.DateTime } => this.GetDateTime(i),
        _ => throw new NotSupportedException(),
    };

    /// <inheritdoc/>
    public int GetValues(object?[] values) => dbfRecord.GetValues(values);

    /// <inheritdoc/>
    public bool IsDBNull(int i) => dbfRecord.IsDBNull(i);

    /// <inheritdoc/>
    public Altemiq.Geometry.Point GetPoint() => ThrowIfNull(mapRecord).GetPoint();

    /// <inheritdoc/>
    public PointZ GetPointZ() => ThrowIfNull(mapRecord).GetPointZ();

    /// <inheritdoc/>
    public PointM GetPointM() => ThrowIfNull(mapRecord).GetPointM();

    /// <inheritdoc/>
    public PointZM GetPointZM() => ThrowIfNull(mapRecord).GetPointZM();

    /// <inheritdoc/>
    public IMultiGeometry<Altemiq.Geometry.Point> GetMultiPoint() => ThrowIfNull(mapRecord).GetMultiPoint();

    /// <inheritdoc/>
    public IMultiGeometry<PointZ> GetMultiPointZ() => ThrowIfNull(mapRecord).GetMultiPointZ();

    /// <inheritdoc/>
    public IMultiGeometry<PointM> GetMultiPointM() => ThrowIfNull(mapRecord).GetMultiPointM();

    /// <inheritdoc/>
    public IMultiGeometry<PointZM> GetMultiPointZM() => ThrowIfNull(mapRecord).GetMultiPointZM();

    /// <inheritdoc/>
    public Polyline GetLineString() => ThrowIfNull(mapRecord).GetLineString();

    /// <inheritdoc/>
    public PolylineZ GetLineStringZ() => ThrowIfNull(mapRecord).GetLineStringZ();

    /// <inheritdoc/>
    public PolylineM GetLineStringM() => ThrowIfNull(mapRecord).GetLineStringM();

    /// <inheritdoc/>
    public PolylineZM GetLineStringZM() => ThrowIfNull(mapRecord).GetLineStringZM();

    /// <inheritdoc/>
    public IMultiGeometry<Polyline> GetMultiLineString() => ThrowIfNull(mapRecord).GetMultiLineString();

    /// <inheritdoc/>
    public IMultiGeometry<PolylineZ> GetMultiLineStringZ() => ThrowIfNull(mapRecord).GetMultiLineStringZ();

    /// <inheritdoc/>
    public IMultiGeometry<PolylineM> GetMultiLineStringM() => ThrowIfNull(mapRecord).GetMultiLineStringM();

    /// <inheritdoc/>
    public IMultiGeometry<PolylineZM> GetMultiLineStringZM() => ThrowIfNull(mapRecord).GetMultiLineStringZM();

    /// <inheritdoc/>
    public Polygon GetPolygon() => ThrowIfNull(mapRecord).GetPolygon();

    /// <inheritdoc/>
    public PolygonZ GetPolygonZ() => ThrowIfNull(mapRecord).GetPolygonZ();

    /// <inheritdoc/>
    public PolygonM GetPolygonM() => ThrowIfNull(mapRecord).GetPolygonM();

    /// <inheritdoc/>
    public PolygonZM GetPolygonZM() => ThrowIfNull(mapRecord).GetPolygonZM();

    /// <inheritdoc/>
    public IMultiGeometry<Polygon> GetMultiPolygon() => ThrowIfNull(mapRecord).GetMultiPolygon();

    /// <inheritdoc/>
    public IMultiGeometry<PolygonZ> GetMultiPolygonZ() => ThrowIfNull(mapRecord).GetMultiPolygonZ();

    /// <inheritdoc/>
    public IMultiGeometry<PolygonM> GetMultiPolygonM() => ThrowIfNull(mapRecord).GetMultiPolygonM();

    /// <inheritdoc/>
    public IMultiGeometry<PolygonZM> GetMultiPolygonZM() => ThrowIfNull(mapRecord).GetMultiPolygonZM();

    /// <inheritdoc/>
    public IGeometry GetGeometry() => ThrowIfNull(mapRecord).GetGeometry();

    /// <inheritdoc/>
    public bool IsNull() => ThrowIfNull(mapRecord).IsNull();

    /// <summary>
    /// Gets the geometry name.
    /// </summary>
    /// <returns>The geometry name.</returns>
    public string? GetGeometryTypeName() => mapRecord?.GeometryType.ToString().ToLowerInvariant();

    /// <summary>
    /// Disposes the resources for this instance.
    /// </summary>
    /// <param name="disposing">Set to <see langword="true"/> to dispose of managed resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                if (mapRecord is IDisposable geometryDisposable)
                {
                    geometryDisposable.Dispose();
                }

                if (dbfRecord is IDisposable dataDisposable)
                {
                    dataDisposable.Dispose();
                }
            }

            this.disposedValue = true;
        }
    }

    private static T ThrowIfNull<T>(T? value)
        where T : class => value ?? throw new InvalidOperationException();
}