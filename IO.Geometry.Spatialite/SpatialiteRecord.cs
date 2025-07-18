// -----------------------------------------------------------------------
// <copyright file="SpatialiteRecord.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Spatialite;

using System.Data;
using Altemiq.Geometry;

/// <summary>
/// The spatialite record.
/// </summary>
/// <param name="dataReader">The data reader.</param>
/// <param name="geometryField">The geometry field.</param>
public class SpatialiteRecord(Microsoft.Data.Sqlite.SqliteDataReader dataReader, int geometryField = -1) : Data.IGeometryDataRecord, Data.IGeometryRecord
{
    /// <inheritdoc/>
    public int FieldCount => dataReader.FieldCount;

    /// <inheritdoc/>
    public object? this[int i] => i == geometryField ? this.GetGeometry() : dataReader[i];

    /// <inheritdoc/>
    public object? this[string name] => this[this.GetOrdinal(name)];

    /// <inheritdoc/>
    public bool GetBoolean(int i) => dataReader.GetBoolean(i);

    /// <inheritdoc/>
    public byte GetByte(int i) => dataReader.GetByte(i);

    /// <inheritdoc/>
    public long GetBytes(int i, long fieldOffset, byte[]? buffer, int bufferoffset, int length) => dataReader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);

    /// <inheritdoc/>
    public char GetChar(int i) => dataReader.GetChar(i);

    /// <inheritdoc/>
    public long GetChars(int i, long fieldoffset, char[]? buffer, int bufferoffset, int length) => dataReader.GetChars(i, fieldoffset, buffer, bufferoffset, length);

    /// <inheritdoc/>
    public IDataReader GetData(int i) => dataReader;

    /// <inheritdoc/>
    public string GetDataTypeName(int i) => dataReader.GetDataTypeName(i);

    /// <inheritdoc/>
    public DateTime GetDateTime(int i) => dataReader.GetDateTime(i);

    /// <inheritdoc/>
    public decimal GetDecimal(int i) => dataReader.GetDecimal(i);

    /// <inheritdoc/>
    public double GetDouble(int i) => dataReader.GetDouble(i);

    /// <inheritdoc/>
    public Type? GetFieldType(int i) => dataReader.GetFieldType(i);

    /// <inheritdoc/>
    public float GetFloat(int i) => dataReader.GetFloat(i);

    /// <inheritdoc/>
    public object GetGeometry() => this.GetGeometry(geometryField);

    /// <inheritdoc/>
    public object GetGeometry(int i) => this.GetGaiaRecord(i).GetGeometry();

    /// <inheritdoc/>
    public Guid GetGuid(int i) => dataReader.GetGuid(i);

    /// <inheritdoc/>
    public short GetInt16(int i) => dataReader.GetInt16(i);

    /// <inheritdoc/>
    public int GetInt32(int i) => dataReader.GetInt32(i);

    /// <inheritdoc/>
    public long GetInt64(int i) => dataReader.GetInt64(i);

    /// <inheritdoc/>
    public Point GetPoint() => this.GetPoint(geometryField);

    /// <inheritdoc/>
    public Point GetPoint(int i) => this.GetGaiaRecord(i).GetPoint();

    /// <inheritdoc/>
    public PointZ GetPointZ() => this.GetPointZ(geometryField);

    /// <inheritdoc/>
    public PointZ GetPointZ(int i) => this.GetGaiaRecord(i).GetPointZ();

    /// <inheritdoc/>
    public PointM GetPointM() => this.GetPointM(geometryField);

    /// <inheritdoc/>
    public PointM GetPointM(int i) => this.GetGaiaRecord(i).GetPointM();

    /// <inheritdoc/>
    public PointZM GetPointZM() => this.GetPointZM(geometryField);

    /// <inheritdoc/>
    public PointZM GetPointZM(int i) => this.GetGaiaRecord(i).GetPointZM();

    /// <inheritdoc/>
    public IReadOnlyCollection<Point> GetMultiPoint() => this.GetMultiPoint(geometryField);

    /// <inheritdoc/>
    public IReadOnlyCollection<Point> GetMultiPoint(int i) => this.GetGaiaRecord(i).GetMultiPoint();

    /// <inheritdoc/>
    public IReadOnlyCollection<PointZ> GetMultiPointZ() => this.GetMultiPointZ(geometryField);

    /// <inheritdoc/>
    public IReadOnlyCollection<PointZ> GetMultiPointZ(int i) => this.GetGaiaRecord(i).GetMultiPointZ();

    /// <inheritdoc/>
    public IReadOnlyCollection<PointM> GetMultiPointM() => this.GetMultiPointM(geometryField);

    /// <inheritdoc/>
    public IReadOnlyCollection<PointM> GetMultiPointM(int i) => this.GetGaiaRecord(i).GetMultiPointM();

    /// <inheritdoc/>
    public IReadOnlyCollection<PointZM> GetMultiPointZM() => this.GetMultiPointZM(geometryField);

    /// <inheritdoc/>
    public IReadOnlyCollection<PointZM> GetMultiPointZM(int i) => this.GetGaiaRecord(i).GetMultiPointZM();

    /// <inheritdoc/>
    public Polyline GetLineString() => this.GetLineString(geometryField);

    /// <inheritdoc/>
    public Polyline GetLineString(int i) => this.GetGaiaRecord(i).GetLineString();

    /// <inheritdoc/>
    public PolylineZ GetLineStringZ() => this.GetLineStringZ(geometryField);

    /// <inheritdoc/>
    public PolylineZ GetLineStringZ(int i) => this.GetGaiaRecord(i).GetLineStringZ();

    /// <inheritdoc/>
    public PolylineM GetLineStringM() => this.GetLineStringM(geometryField);

    /// <inheritdoc/>
    public PolylineM GetLineStringM(int i) => this.GetGaiaRecord(i).GetLineStringM();

    /// <inheritdoc/>
    public PolylineZM GetLineStringZM() => this.GetLineStringZM(geometryField);

    /// <inheritdoc/>
    public PolylineZM GetLineStringZM(int i) => this.GetGaiaRecord(i).GetLineStringZM();

    /// <inheritdoc/>
    public IReadOnlyCollection<Polyline> GetMultiLineString() => this.GetMultiLineString(geometryField);

    /// <inheritdoc/>
    public IReadOnlyCollection<Polyline> GetMultiLineString(int i) => this.GetGaiaRecord(i).GetMultiLineString();

    /// <inheritdoc/>
    public IReadOnlyCollection<PolylineZ> GetMultiLineStringZ() => this.GetMultiLineStringZ(geometryField);

    /// <inheritdoc/>
    public IReadOnlyCollection<PolylineZ> GetMultiLineStringZ(int i) => this.GetGaiaRecord(i).GetMultiLineStringZ();

    /// <inheritdoc/>
    public IReadOnlyCollection<PolylineM> GetMultiLineStringM() => this.GetMultiLineStringM(geometryField);

    /// <inheritdoc/>
    public IReadOnlyCollection<PolylineM> GetMultiLineStringM(int i) => this.GetGaiaRecord(i).GetMultiLineStringM();

    /// <inheritdoc/>
    public IReadOnlyCollection<PolylineZM> GetMultiLineStringZM() => this.GetMultiLineStringZM(geometryField);

    /// <inheritdoc/>
    public IReadOnlyCollection<PolylineZM> GetMultiLineStringZM(int i) => this.GetGaiaRecord(i).GetMultiLineStringZM();

    /// <inheritdoc/>
    public Polygon GetPolygon() => this.GetPolygon(geometryField);

    /// <inheritdoc/>
    public Polygon GetPolygon(int i) => this.GetGaiaRecord(i).GetPolygon();

    /// <inheritdoc/>
    public PolygonZ GetPolygonZ() => this.GetPolygonZ(geometryField);

    /// <inheritdoc/>
    public PolygonZ GetPolygonZ(int i) => this.GetGaiaRecord(i).GetPolygonZ();

    /// <inheritdoc/>
    public PolygonM GetPolygonM() => this.GetPolygonM(geometryField);

    /// <inheritdoc/>
    public PolygonM GetPolygonM(int i) => this.GetGaiaRecord(i).GetPolygonM();

    /// <inheritdoc/>
    public PolygonZM GetPolygonZM() => this.GetPolygonZM(geometryField);

    /// <inheritdoc/>
    public PolygonZM GetPolygonZM(int i) => this.GetGaiaRecord(i).GetPolygonZM();

    /// <inheritdoc/>
    public IReadOnlyCollection<Polygon> GetMultiPolygon() => this.GetMultiPolygon(geometryField);

    /// <inheritdoc/>
    public IReadOnlyCollection<Polygon> GetMultiPolygon(int i) => this.GetGaiaRecord(i).GetMultiPolygon();

    /// <inheritdoc/>
    public IReadOnlyCollection<PolygonZ> GetMultiPolygonZ() => this.GetMultiPolygonZ(geometryField);

    /// <inheritdoc/>
    public IReadOnlyCollection<PolygonZ> GetMultiPolygonZ(int i) => this.GetGaiaRecord(i).GetMultiPolygonZ();

    /// <inheritdoc/>
    public IReadOnlyCollection<PolygonM> GetMultiPolygonM() => this.GetMultiPolygonM(geometryField);

    /// <inheritdoc/>
    public IReadOnlyCollection<PolygonM> GetMultiPolygonM(int i) => this.GetGaiaRecord(i).GetMultiPolygonM();

    /// <inheritdoc/>
    public IReadOnlyCollection<PolygonZM> GetMultiPolygonZM() => this.GetMultiPolygonZM(geometryField);

    /// <inheritdoc/>
    public IReadOnlyCollection<PolygonZM> GetMultiPolygonZM(int i) => this.GetGaiaRecord(i).GetMultiPolygonZM();

    /// <inheritdoc/>
    public string GetName(int i) => dataReader.GetName(i);

    /// <inheritdoc/>
    public int GetOrdinal(string name) => dataReader.GetOrdinal(name);

    /// <inheritdoc/>
    public string GetString(int i) => dataReader.GetString(i);

    /// <inheritdoc/>
    public object GetValue(int i)
    {
        var value = dataReader.GetValue(i);

        // see if this is a gaia record
        return value is byte[] { Length: >= 45 } bytes && GaiaRecord.TryCreate(bytes, out var record)
            ? record.GetGeometry()
            : value;
    }

    /// <inheritdoc/>
    public int GetValues(object?[] values) => dataReader.GetValues(values);

    /// <inheritdoc/>
    public bool IsNull() => this.IsDBNull(geometryField);

    /// <inheritdoc/>
    public bool IsDBNull(int i) => dataReader.IsDBNull(i);

    private GaiaRecord GetGaiaRecord(int index)
    {
        return new GaiaRecord(GetBytesCore(index));

        byte[] GetBytesCore(int idx)
        {
            var byteCount = this.GetBytes(idx, default, default, default, default);
            var bytes = new byte[byteCount];
            return byteCount == this.GetBytes(idx, default, bytes, default, (int)byteCount)
                ? bytes
                : throw new Data.InsufficientDataException();
        }
    }
}