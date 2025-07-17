// -----------------------------------------------------------------------
// <copyright file="ShapefileWriter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

/// <summary>
/// The Shapefile writer.
/// </summary>
public class ShapefileWriter : IDisposable
{
    private readonly ShpWriter shpWriter;

    private readonly ShxWriter shxWriter;

    private readonly Dbf.DbfWriter dbfWriter;

    private int count;

    private bool disposedValue;

    /// <summary>
    /// Initialises a new instance of the <see cref="ShapefileWriter"/> class.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="type">The SHP type.</param>
    /// <param name="columns">The DBF columns.</param>
    public ShapefileWriter(string path, ShpType type, params Dbf.DbfColumn[] columns)
        : this(path, type, default, columns)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="ShapefileWriter"/> class.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="type">The SHP type.</param>
    /// <param name="wkid">The well-known ID.</param>
    /// <param name="columns">The DBF columns.</param>
    public ShapefileWriter(string path, ShpType type, int wkid, params Dbf.DbfColumn[] columns)
        : this(
              File.OpenWrite(Path.ChangeExtension(path, Constants.ShpExtension)),
              File.OpenWrite(Path.ChangeExtension(path, Constants.ShxExtension)),
              File.OpenWrite(Path.ChangeExtension(path, Constants.DbfExtension)),
              wkid is not 0 ? File.OpenWrite(Path.ChangeExtension(path, Constants.PrjExtension)) : null,
              type,
              wkid,
              leaveOpen: false,
              columns)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="ShapefileWriter"/> class.
    /// </summary>
    /// <param name="shpStream">The <c>SHP</c> stream.</param>
    /// <param name="shxStream">The <c>SHX</c> stream.</param>
    /// <param name="dbfStream">The <c>DBF</c> stream.</param>
    /// <param name="type">The SHP type.</param>
    /// <param name="leaveOpen">Set to <see langword="true"/> to leave the streams open when this instance is disposed.</param>
    /// <param name="columns">The DBF columns.</param>
    public ShapefileWriter(Stream shpStream, Stream shxStream, Stream dbfStream, ShpType type, bool leaveOpen, params Dbf.DbfColumn[] columns)
        : this(shpStream, shxStream, dbfStream, default, type, default, leaveOpen, columns)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="ShapefileWriter"/> class.
    /// </summary>
    /// <param name="shpStream">The <c>SHP</c> stream.</param>
    /// <param name="shxStream">The <c>SHX</c> stream.</param>
    /// <param name="dbfStream">The <c>DBF</c> stream.</param>
    /// <param name="prjStream">The <c>PRJ</c> stream.</param>
    /// <param name="type">The SHP type.</param>
    /// <param name="wkid">The well-known ID.</param>
    /// <param name="leaveOpen">Set to <see langword="true"/> to leave the streams open when this instance is disposed.</param>
    /// <param name="columns">The DBF columns.</param>
    public ShapefileWriter(Stream shpStream, Stream shxStream, Stream dbfStream, Stream? prjStream, ShpType type, int wkid, bool leaveOpen, params Dbf.DbfColumn[] columns)
    {
        var header = new Header(type);
        this.shpWriter = new(shpStream, leaveOpen);
        this.shpWriter.Write(header);

        this.shxWriter = new(shxStream, leaveOpen);
        this.shxWriter.Write(header);

        this.dbfWriter = new(dbfStream, new() { WriteTrailingDecimals = true }, leaveOpen: leaveOpen);

        // force no encoding in the header by default
        var dbfHeader = new Dbf.DbfHeader(Dbf.DbfVersion.DBase3WithoutMemo, null!);
        dbfHeader.AddColumns(columns);
        this.dbfWriter.Write(dbfHeader, writeDataAddress: false);

        if (wkid is not 0)
        {
            if (prjStream is null)
            {
                throw new InvalidOperationException();
            }

            PrjWriter.WriteTo(prjStream, wkid);
            if (!leaveOpen)
            {
                prjStream.Dispose();
            }
        }
    }

    /// <summary>
    /// Writes the geometry to the file.
    /// </summary>
    /// <param name="geometry">The geometry.</param>
    /// <param name="values">The values.</param>
    public void Write(object? geometry, params object?[] values)
    {
        var offset = (uint)this.shpWriter.BaseStream.Position;
        this.shpWriter.Write(geometry);
        var contentLength = (uint)(this.shpWriter.BaseStream.Position - offset - ShpRecordHeader.Size);
        this.shxWriter.Write(new ShxRecord(offset / 2, contentLength / 2));
        this.dbfWriter.Write(values);
        this.count++;
    }

    /// <summary>
    /// Updates the header.
    /// </summary>
    /// <param name="extents">The extents.</param>
    public void Update(EnvelopeZM extents)
    {
        this.shpWriter.Update(extents);
        this.shxWriter.Update(extents);
        this.dbfWriter.Update(this.count, writeDataAddress: false);
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
                this.shpWriter.Dispose();
                this.shxWriter.Dispose();
                this.dbfWriter.Dispose();
            }

            this.disposedValue = true;
        }
    }
}