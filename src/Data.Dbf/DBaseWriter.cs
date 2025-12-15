// -----------------------------------------------------------------------
// <copyright file="DBaseWriter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data.Dbf;

/// <summary>
/// This class represents a <see cref="Dbf"/> writer. You can create new and save <see cref="Dbf"/> files using this class and supporting classes.
/// </summary>
/// <param name="dbfStream">The <c>DBF</c> stream.</param>
/// <param name="dbtStream">The <c>DBT</c> stream.</param>
/// <param name="options">The options.</param>
/// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="DbfWriter"/> object is disposed; otherwise, <see langword="false"/>.</param>
public class DBaseWriter(Stream dbfStream, Stream? dbtStream, DbfWriterOptions? options = default, bool leaveOpen = false) : IDisposable
{
    private readonly DbfWriter dbfWriter = new(dbfStream, options, leaveOpen);

    private readonly Func<System.Text.Encoding, DbtWriter?> dbtFactory = encoding => dbtStream is not null ? new(dbtStream, encoding, leaveOpen) : default;

    private DbtWriter? dbtWriter;

    private bool disposedValue;

    /// <summary>
    /// Writes the header.
    /// </summary>
    /// <param name="columns">The columns.</param>
    public void Write(params IEnumerable<DbfColumn> columns)
    {
        var header = new DbfHeader();
        header.AddRange(columns);
        this.Write(header);
    }

    /// <summary>
    /// Writes the header.
    /// </summary>
    /// <param name="header">The header.</param>
    /// <param name="writeDataAddress">Set to <see langword="true"/> to write the <see cref="DbfColumn.DataAddress"/> value.</param>
    public void Write(DbfHeader header, bool writeDataAddress = true)
    {
        this.dbtWriter = this.dbtFactory(header.GetEncodingOrDefault());
        this.dbfWriter.Write(header, writeDataAddress);
    }

    /// <summary>
    /// Writes the values to the file.
    /// </summary>
    /// <param name="values">The values.</param>
    public void Write(params IEnumerable<object?> values)
    {
        var data = this.dbfWriter.CreateRecordBytes();

        foreach (var (column, value) in this.dbfWriter.Header.Zip(values, (field, value) => (field, value)))
        {
            if (column.DbfType is DbfColumn.DbfColumnType.Memo)
            {
                if (this.dbtWriter is null)
                {
                    throw new InvalidOperationException();
                }

                if (value is string stringValue)
                {
                    // handle this correctly
                    var index = this.dbtWriter.NextBlock;
                    this.dbtWriter.Write(stringValue);
                    this.dbfWriter.WriteTo(DbfColumn.DbfColumnType.Number, column.DataAddress, 10, numericPrecision: null, index, data);
                }
                else
                {
                    this.dbfWriter.WriteTo(DbfColumn.DbfColumnType.Number, column.DataAddress, 10, numericPrecision: null, value: null, data);
                }
            }
            else
            {
                this.dbfWriter.WriteTo(column, value, data);
            }
        }

        this.dbfWriter.Write(data);
    }

    /// <summary>
    /// Updates the header.
    /// </summary>
    /// <param name="recordCount">The record count.</param>
    /// <param name="writeDataAddress">Set to <see langword="true"/> to write the <see cref="DbfColumn.DataAddress"/> value.</param>
    public void Update(int recordCount, bool writeDataAddress = true) => this.dbfWriter.Update(recordCount, writeDataAddress);

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
        if (this.disposedValue)
        {
            return;
        }

        if (disposing)
        {
            this.dbfWriter.Dispose();
            this.dbtWriter?.Dispose();
        }

        this.disposedValue = true;
    }
}