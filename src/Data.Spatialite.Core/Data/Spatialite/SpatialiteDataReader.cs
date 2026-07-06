// -----------------------------------------------------------------------
// <copyright file="SpatialiteDataReader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data.Spatialite;

/// <summary>
/// The <c>SpatiaLite</c> reader.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1010:Generic interface should also be implemented", Justification = "The generic version is IEnumerable<object>")]
public class SpatialiteDataReader : System.Data.Common.DbDataReader, IGeometryDataReader
{
    private readonly Microsoft.Data.Sqlite.SqliteDataReader dataReader;

    private int geometryField = -1;

    private bool disposedValue;

    /// <summary>
    /// Initialises a new instance of the <see cref="SpatialiteDataReader"/> class.
    /// </summary>
    /// <param name="command">The <c>SQLite</c> command.</param>
    public SpatialiteDataReader(Microsoft.Data.Sqlite.SqliteCommand command)
        : this(command.ExecuteReader())
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="SpatialiteDataReader"/> class.
    /// </summary>
    /// <param name="dataReader">The <c>SQLite</c> <see cref="System.Data.Common.DbDataReader"/>.</param>
    internal SpatialiteDataReader(Microsoft.Data.Sqlite.SqliteDataReader dataReader) => this.dataReader = dataReader;

    /// <inheritdoc/>
    public override int FieldCount => this.dataReader.FieldCount;

    /// <inheritdoc/>
    public override int Depth => this.dataReader.Depth;

    /// <inheritdoc/>
    public override bool HasRows => this.dataReader.HasRows;

    /// <inheritdoc/>
    public override bool IsClosed => this.dataReader.IsClosed;

    /// <inheritdoc/>
    public override int RecordsAffected => this.dataReader.RecordsAffected;

    /// <inheritdoc/>
    public override object this[string name] => this[this.GetOrdinal(name)];

    /// <inheritdoc/>
    public override object this[int ordinal] => ordinal == this.geometryField ? this.GetGeometry(ordinal) : this.dataReader[ordinal];

    /// <inheritdoc/>
    public override bool Read()
    {
        if (!this.dataReader.Read())
        {
            return default;
        }

        if (this.geometryField is -1)
        {
            this.geometryField = GetGeometryOrdinal(this.dataReader);
        }

        return true;

        static int GetGeometryOrdinal(Microsoft.Data.Sqlite.SqliteDataReader dataReader)
        {
            for (var i = 0; i < dataReader.FieldCount; i++)
            {
                // sniff this to see if it's a geometry column
                if (string.Equals(dataReader.GetDataTypeName(i), "BLOB", StringComparison.Ordinal)
                    && dataReader.GetValue(i) is byte[] { Length: >= 45 } bytes
                    && Buffers.Binary.GaiaPrimitives.IsValid(bytes))
                {
                    return i;
                }
            }

            return -1;
        }
    }

    /// <inheritdoc/>
    public override bool GetBoolean(int ordinal) => this.dataReader.GetBoolean(ordinal);

    /// <inheritdoc/>
    public override byte GetByte(int ordinal) => this.dataReader.GetByte(ordinal);

    /// <inheritdoc/>
    public override long GetBytes(int ordinal, long dataOffset, byte[]? buffer, int bufferOffset, int length) => this.dataReader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);

    /// <inheritdoc/>
    public override char GetChar(int ordinal) => this.dataReader.GetChar(ordinal);

    /// <inheritdoc/>
    public override long GetChars(int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length) => this.dataReader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);

    /// <inheritdoc/>
    public override string GetDataTypeName(int ordinal) => this.dataReader.GetDataTypeName(ordinal);

    /// <inheritdoc/>
    public override DateTime GetDateTime(int ordinal) => this.dataReader.GetDateTime(ordinal);

    /// <inheritdoc/>
    public override decimal GetDecimal(int ordinal) => this.dataReader.GetDecimal(ordinal);

    /// <inheritdoc/>
    public override double GetDouble(int ordinal) => this.dataReader.GetDouble(ordinal);

    /// <inheritdoc/>
#if NET6_0_OR_GREATER
    [return: System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicFields | System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicProperties)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Trimming", "IL2073:Target method return value does not satisfy 'DynamicallyAccessedMembersAttribute' requirements. The return value of the source method does not have matching annotations.", Justification = "Cannot change class outside our control")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Trimming", "IL2093:'DynamicallyAccessedMemberTypes' on the return value of method don't match overridden return value of method. All overridden members must have the same 'DynamicallyAccessedMembersAttribute' usage.", Justification = "IDataRecord interface has this, but DbDataReader doesn't")]
#endif
    public override Type GetFieldType(int ordinal) => this.dataReader.GetFieldType(ordinal);

    /// <inheritdoc/>
    public override float GetFloat(int ordinal) => this.dataReader.GetFloat(ordinal);

    /// <inheritdoc/>
    public Geometry.IGeometry GetGeometry(int i) => Buffers.Binary.GaiaPrimitives.ReadGeometry(this.GetBytes(i));

    /// <inheritdoc/>
    public override Guid GetGuid(int ordinal) => this.dataReader.GetGuid(ordinal);

    /// <inheritdoc/>
    public override short GetInt16(int ordinal) => this.dataReader.GetInt16(ordinal);

    /// <inheritdoc/>
    public override int GetInt32(int ordinal) => this.dataReader.GetInt32(ordinal);

    /// <inheritdoc/>
    public override long GetInt64(int ordinal) => this.dataReader.GetInt64(ordinal);

    /// <inheritdoc/>
    public Geometry.Point GetPoint(int i) => Buffers.Binary.GaiaPrimitives.ReadPoint(this.GetBytes(i));

    /// <inheritdoc/>
    public Geometry.PointZ GetPointZ(int i) => Buffers.Binary.GaiaPrimitives.ReadPointZ(this.GetBytes(i));

    /// <inheritdoc/>
    public Geometry.PointM GetPointM(int i) => Buffers.Binary.GaiaPrimitives.ReadPointM(this.GetBytes(i));

    /// <inheritdoc/>
    public Geometry.PointZM GetPointZM(int i) => Buffers.Binary.GaiaPrimitives.ReadPointZM(this.GetBytes(i));

    /// <inheritdoc/>
    public Geometry.IMultiGeometry<Geometry.Point> GetMultiPoint(int i) => Buffers.Binary.GaiaPrimitives.ReadMultiPoint(this.GetBytes(i));

    /// <inheritdoc/>
    public Geometry.IMultiGeometry<Geometry.PointZ> GetMultiPointZ(int i) => Buffers.Binary.GaiaPrimitives.ReadMultiPointZ(this.GetBytes(i));

    /// <inheritdoc/>
    public Geometry.IMultiGeometry<Geometry.PointM> GetMultiPointM(int i) => Buffers.Binary.GaiaPrimitives.ReadMultiPointM(this.GetBytes(i));

    /// <inheritdoc/>
    public Geometry.IMultiGeometry<Geometry.PointZM> GetMultiPointZM(int i) => Buffers.Binary.GaiaPrimitives.ReadMultiPointZM(this.GetBytes(i));

    /// <inheritdoc/>
    public Geometry.Polyline GetLineString(int i) => Buffers.Binary.GaiaPrimitives.ReadLineString(this.GetBytes(i));

    /// <inheritdoc/>
    public Geometry.PolylineZ GetLineStringZ(int i) => Buffers.Binary.GaiaPrimitives.ReadLineStringZ(this.GetBytes(i));

    /// <inheritdoc/>
    public Geometry.PolylineM GetLineStringM(int i) => Buffers.Binary.GaiaPrimitives.ReadLineStringM(this.GetBytes(i));

    /// <inheritdoc/>
    public Geometry.PolylineZM GetLineStringZM(int i) => Buffers.Binary.GaiaPrimitives.ReadLineStringZM(this.GetBytes(i));

    /// <inheritdoc/>
    public Geometry.IMultiGeometry<Geometry.Polyline> GetMultiLineString(int i) => Buffers.Binary.GaiaPrimitives.ReadMultiLineString(this.GetBytes(i));

    /// <inheritdoc/>
    public Geometry.IMultiGeometry<Geometry.PolylineZ> GetMultiLineStringZ(int i) => Buffers.Binary.GaiaPrimitives.ReadMultiLineStringZ(this.GetBytes(i));

    /// <inheritdoc/>
    public Geometry.IMultiGeometry<Geometry.PolylineM> GetMultiLineStringM(int i) => Buffers.Binary.GaiaPrimitives.ReadMultiLineStringM(this.GetBytes(i));

    /// <inheritdoc/>
    public Geometry.IMultiGeometry<Geometry.PolylineZM> GetMultiLineStringZM(int i) => Buffers.Binary.GaiaPrimitives.ReadMultiLineStringZM(this.GetBytes(i));

    /// <inheritdoc/>
    public Geometry.Polygon GetPolygon(int i) => Buffers.Binary.GaiaPrimitives.ReadPolygon(this.GetBytes(i));

    /// <inheritdoc/>
    public Geometry.PolygonZ GetPolygonZ(int i) => Buffers.Binary.GaiaPrimitives.ReadPolygonZ(this.GetBytes(i));

    /// <inheritdoc/>
    public Geometry.PolygonM GetPolygonM(int i) => Buffers.Binary.GaiaPrimitives.ReadPolygonM(this.GetBytes(i));

    /// <inheritdoc/>
    public Geometry.PolygonZM GetPolygonZM(int i) => Buffers.Binary.GaiaPrimitives.ReadPolygonZM(this.GetBytes(i));

    /// <inheritdoc/>
    public Geometry.IMultiGeometry<Geometry.Polygon> GetMultiPolygon(int i) => Buffers.Binary.GaiaPrimitives.ReadMultiPolygon(this.GetBytes(i));

    /// <inheritdoc/>
    public Geometry.IMultiGeometry<Geometry.PolygonZ> GetMultiPolygonZ(int i) => Buffers.Binary.GaiaPrimitives.ReadMultiPolygonZ(this.GetBytes(i));

    /// <inheritdoc/>
    public Geometry.IMultiGeometry<Geometry.PolygonM> GetMultiPolygonM(int i) => Buffers.Binary.GaiaPrimitives.ReadMultiPolygonM(this.GetBytes(i));

    /// <inheritdoc/>
    public Geometry.IMultiGeometry<Geometry.PolygonZM> GetMultiPolygonZM(int i) => Buffers.Binary.GaiaPrimitives.ReadMultiPolygonZM(this.GetBytes(i));

    /// <inheritdoc/>
    public override string GetName(int ordinal) => this.dataReader.GetName(ordinal);

    /// <inheritdoc/>
    public override int GetOrdinal(string name) => this.dataReader.GetOrdinal(name);

    /// <inheritdoc/>
    public override string GetString(int ordinal) => this.dataReader.GetString(ordinal);

    /// <inheritdoc/>
    public override object GetValue(int ordinal)
    {
        var value = this.dataReader.GetValue(ordinal);

        // see if this is a gaia record
        return value is byte[] { Length: >= 45 } bytes && Buffers.Binary.GaiaPrimitives.IsValid(bytes)
            ? Buffers.Binary.GaiaPrimitives.ReadGeometry(bytes)
            : value;
    }

    /// <inheritdoc/>
    public override int GetValues(object?[] values) => this.dataReader.GetValues(values);

    /// <inheritdoc/>
    public override bool IsDBNull(int ordinal) => this.dataReader.IsDBNull(ordinal);

    /// <inheritdoc/>
    public override bool NextResult() => this.dataReader.NextResult();

    /// <inheritdoc/>
    public override System.Collections.IEnumerator GetEnumerator() => this.dataReader.GetEnumerator();

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.dataReader.Dispose();
            }

            this.disposedValue = true;
        }

        base.Dispose(disposing);
    }

    private ReadOnlySpan<byte> GetBytes(int ordinal)
    {
        var byteCount = this.GetBytes(ordinal, default, default, default, default);
        var bytes = new byte[byteCount];
        return byteCount == this.GetBytes(ordinal, default, bytes, default, (int)byteCount)
            ? bytes
            : throw new Data.InsufficientDataException();
    }
}