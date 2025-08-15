// -----------------------------------------------------------------------
// <copyright file="ShapefileRecord.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

/// <summary>
/// The Shapefile record.
/// </summary>
/// <param name="shpRecord">The SHP record.</param>
/// <param name="dbfRecord">The DBF record.</param>
public class ShapefileRecord(ShpRecord shpRecord, Dbf.DbfRecord dbfRecord) : Data.IGeometryRecord, System.Data.IDataRecord, IDisposable
{
    private bool disposedValue;

    /// <inheritdoc/>
    public int FieldCount => dbfRecord.FieldCount;

    /// <inheritdoc/>
    public object? this[string name] => dbfRecord[name];

    /// <inheritdoc/>
    public object? this[int i] => dbfRecord[i];

    /// <inheritdoc/>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    public bool GetBoolean(int i) => dbfRecord.GetBoolean(i);

    /// <inheritdoc/>
    public byte GetByte(int i) => dbfRecord.GetByte(i);

    /// <inheritdoc/>
    public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) => dbfRecord.GetBytes(i, fieldOffset, buffer, bufferoffset, length);

    /// <inheritdoc/>
    public char GetChar(int i) => dbfRecord.GetChar(i);

    /// <inheritdoc/>
    public long GetChars(int i, long fieldoffset, char[]? buffer, int bufferoffset, int length) => dbfRecord.GetChars(i, fieldoffset, buffer, bufferoffset, length);

    /// <inheritdoc/>
    public System.Data.IDataReader GetData(int i) => dbfRecord.GetData(i);

    /// <inheritdoc/>
    public string GetDataTypeName(int i) => dbfRecord.GetDataTypeName(i);

    /// <inheritdoc/>
    public DateTime GetDateTime(int i) => dbfRecord.GetDateTime(i);

    /// <inheritdoc/>
    public decimal GetDecimal(int i) => dbfRecord.GetDecimal(i);

    /// <inheritdoc/>
    public double GetDouble(int i) => dbfRecord.GetDouble(i);

    /// <inheritdoc/>
    public Type GetFieldType(int i) => dbfRecord.GetFieldType(i);

    /// <inheritdoc/>
    public float GetFloat(int i) => dbfRecord.GetFloat(i);

    /// <inheritdoc/>
    public Guid GetGuid(int i) => dbfRecord.GetGuid(i);

    /// <inheritdoc/>
    public short GetInt16(int i) => dbfRecord.GetInt16(i);

    /// <inheritdoc/>
    public int GetInt32(int i) => dbfRecord.GetInt32(i);

    /// <inheritdoc/>
    public long GetInt64(int i) => dbfRecord.GetInt64(i);

    /// <inheritdoc/>
    public string GetName(int i) => dbfRecord.GetName(i);

    /// <inheritdoc/>
    public int GetOrdinal(string name) => dbfRecord.GetOrdinal(name);

    /// <inheritdoc/>
    public string GetString(int i) => dbfRecord.GetString(i);

    /// <inheritdoc/>
    public object GetValue(int i) => dbfRecord.GetValue(i);

    /// <inheritdoc/>
    public int GetValues(object?[] values) => dbfRecord.GetValues(values);

    /// <inheritdoc/>
    public bool IsDBNull(int i) => dbfRecord.IsDBNull(i);

    /// <inheritdoc/>
    public Point GetPoint() => shpRecord.GetPoint();

    /// <inheritdoc/>
    public PointZ GetPointZ() => shpRecord.GetPointZ();

    /// <inheritdoc/>
    public PointM GetPointM() => shpRecord.GetPointM();

    /// <inheritdoc/>
    public PointZM GetPointZM() => shpRecord.GetPointZM();

    /// <inheritdoc/>
    public IMultiGeometry<Point> GetMultiPoint() => shpRecord.GetMultiPoint();

    /// <inheritdoc/>
    public IMultiGeometry<PointZ> GetMultiPointZ() => shpRecord.GetMultiPointZ();

    /// <inheritdoc/>
    public IMultiGeometry<PointM> GetMultiPointM() => shpRecord.GetMultiPointM();

    /// <inheritdoc/>
    public IMultiGeometry<PointZM> GetMultiPointZM() => shpRecord.GetMultiPointZM();

    /// <inheritdoc/>
    public Polyline GetLineString() => shpRecord.GetLineString();

    /// <inheritdoc/>
    public PolylineZ GetLineStringZ() => shpRecord.GetLineStringZ();

    /// <inheritdoc/>
    public PolylineM GetLineStringM() => shpRecord.GetLineStringM();

    /// <inheritdoc/>
    public PolylineZM GetLineStringZM() => shpRecord.GetLineStringZM();

    /// <inheritdoc/>
    public IMultiGeometry<Polyline> GetMultiLineString() => shpRecord.GetMultiLineString();

    /// <inheritdoc/>
    public IMultiGeometry<PolylineZ> GetMultiLineStringZ() => shpRecord.GetMultiLineStringZ();

    /// <inheritdoc/>
    public IMultiGeometry<PolylineM> GetMultiLineStringM() => shpRecord.GetMultiLineStringM();

    /// <inheritdoc/>
    public IMultiGeometry<PolylineZM> GetMultiLineStringZM() => shpRecord.GetMultiLineStringZM();

    /// <inheritdoc/>
    public Polygon GetPolygon() => shpRecord.GetPolygon();

    /// <inheritdoc/>
    public PolygonZ GetPolygonZ() => shpRecord.GetPolygonZ();

    /// <inheritdoc/>
    public PolygonM GetPolygonM() => shpRecord.GetPolygonM();

    /// <inheritdoc/>
    public PolygonZM GetPolygonZM() => shpRecord.GetPolygonZM();

    /// <inheritdoc/>
    public IMultiGeometry<Polygon> GetMultiPolygon() => shpRecord.GetMultiPolygon();

    /// <inheritdoc/>
    public IMultiGeometry<PolygonZ> GetMultiPolygonZ() => shpRecord.GetMultiPolygonZ();

    /// <inheritdoc/>
    public IMultiGeometry<PolygonM> GetMultiPolygonM() => shpRecord.GetMultiPolygonM();

    /// <inheritdoc/>
    public IMultiGeometry<PolygonZM> GetMultiPolygonZM() => shpRecord.GetMultiPolygonZM();

    /// <inheritdoc/>
    public IGeometry GetGeometry() => shpRecord.GetGeometry();

    /// <inheritdoc/>
    public bool IsNull() => shpRecord.IsNull();

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
                if (shpRecord is IDisposable geometryDisposable)
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
}