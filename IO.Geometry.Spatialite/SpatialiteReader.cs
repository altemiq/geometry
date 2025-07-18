// -----------------------------------------------------------------------
// <copyright file="SpatialiteReader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Spatialite;

/// <summary>
/// The <c>SpatiaLite</c> reader.
/// </summary>
/// <remarks>
/// Initialises a new instance of the <see cref="SpatialiteReader"/> class.
/// </remarks>
/// <param name="dataReader">The <c>SQLite</c> <see cref="System.Data.Common.DbDataReader"/>.</param>
public class SpatialiteReader(Microsoft.Data.Sqlite.SqliteDataReader dataReader) : IDisposable
{
    private int geometryField = -1;

    private bool disposedValue;

    /// <summary>
    /// Initialises a new instance of the <see cref="SpatialiteReader"/> class.
    /// </summary>
    /// <param name="command">The <c>SQLite</c> command.</param>
    public SpatialiteReader(Microsoft.Data.Sqlite.SqliteCommand command)
        : this(command.ExecuteReader())
    {
    }

    /// <summary>
    /// Reads the next GAIA record.
    /// </summary>
    /// <returns>The GAIA record.</returns>
    public SpatialiteRecord? Read()
    {
        if (dataReader.Read())
        {
            if (this.geometryField is -1)
            {
                this.geometryField = GetGeometryOrdinal(dataReader);
            }

            return new(dataReader, this.geometryField);
        }

        return default;

        static int GetGeometryOrdinal(Microsoft.Data.Sqlite.SqliteDataReader dataReader)
        {
            for (var i = 0; i < dataReader.FieldCount; i++)
            {
                var dataType = dataReader.GetDataTypeName(i);
                if (string.Equals(dataType, "BLOB", StringComparison.Ordinal))
                {
                    var value = dataReader.GetValue(i);

                    // sniff this to see if it's a geometry column
                    if (value is byte[] { Length: >= 45 } bytes && GaiaRecord.TryCreate(bytes, out _))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

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
                dataReader.Dispose();
            }

            this.disposedValue = true;
        }
    }
}