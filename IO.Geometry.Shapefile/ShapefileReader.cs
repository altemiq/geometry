// -----------------------------------------------------------------------
// <copyright file="ShapefileReader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

/// <summary>
/// The Shapefile reader.
/// </summary>
public class ShapefileReader : IDisposable
{
    private readonly ShpReader shpReader;

    private readonly ShxReader shxReader;

    private readonly Dbf.DbfReader dbfReader;

    private readonly Stream? prjStream;

    private bool disposedValue;

    /// <summary>
    /// Initialises a new instance of the <see cref="ShapefileReader"/> class.
    /// </summary>
    /// <param name="path">The path to the file, with or without an extension.</param>
    public ShapefileReader(string path)
    {
        this.shpReader = new(File.OpenRead(Path.ChangeExtension(path, Constants.ShpExtension)), leaveOpen: false);
        this.shxReader = new(File.OpenRead(Path.ChangeExtension(path, Constants.ShxExtension)), leaveOpen: false);
        this.dbfReader = Dbf.DbfReader.Open(File.OpenRead(Path.ChangeExtension(path, Constants.DbfExtension)), leaveOpen: false);
        var prjFile = Path.ChangeExtension(path, Constants.PrjExtension);
        if (File.Exists(prjFile))
        {
            using var stream = File.OpenRead(prjFile);
            this.CoordinateReferenceSystem = PrjReader.Read(stream);
        }
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="ShapefileReader"/> class.
    /// </summary>
    /// <param name="shpStream">The SHP stream.</param>
    /// <param name="shxStream">The SHX stream.</param>
    /// <param name="dbfStream">The DBF stream.</param>
    /// <param name="prjStream">The PRJ stream.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the streams open after the <see cref="ShapefileReader"/> object is disposed; otherwise, <see langword="false"/>.</param>
    public ShapefileReader(Stream shpStream, Stream shxStream, Stream dbfStream, Stream? prjStream = default, bool leaveOpen = false)
    {
        this.shpReader = new(shpStream, leaveOpen);
        this.shxReader = new(shxStream, leaveOpen);
        this.dbfReader = new(dbfStream, leaveOpen);
        if (prjStream is not null)
        {
            this.CoordinateReferenceSystem = PrjReader.Read(prjStream);
            if (!leaveOpen)
            {
                this.prjStream = prjStream;
            }
        }
    }

    /// <summary>
    /// Gets the SHP header.
    /// </summary>
    public Header ShpHeader => this.shpReader.Header;

    /// <summary>
    /// Gets the DBF header.
    /// </summary>
    public Dbf.DbfHeader DbfHeader => this.dbfReader.Header;

    /// <summary>
    /// Gets the coordinate reference system.
    /// </summary>
    public Geodesy.WellKnownTextNode? CoordinateReferenceSystem { get; }

    /// <summary>
    /// Gets the count.
    /// </summary>
    public int Count => this.shxReader.Count;

    /// <summary>
    /// Reads the next Shapefile record.
    /// </summary>
    /// <returns>The Shapefile record.</returns>
    public ShapefileRecord? Read()
    {
        return this.shxReader.Read() switch
        {
            { } shxRecord => ReadRecord(shxRecord),
            _ => default,
        };

        ShapefileRecord? ReadRecord(ShxRecord shxRecord)
        {
            return (this.shpReader.Read(shxRecord), this.dbfReader.Read()) switch
            {
                ({ } shpRecord, true) => new(shpRecord, this.dbfReader.GetRecord()),
                _ => default,
            };
        }
    }

    /// <summary>
    /// Reads the Shapefile record at the specified index.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>The Shapefile record.</returns>
    public ShapefileRecord Read(int index) => (this.shpReader.Read(this.shxReader.Read(index)), this.dbfReader.Read(index)) switch
    {
        ({ } shpRecord, true) => new(shpRecord, this.dbfReader.GetRecord()),
        _ => throw new ArgumentOutOfRangeException(nameof(index)),
    };

    /// <inheritdoc/>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Gets the SHP reader.
    /// </summary>
    /// <returns>The SHP reader.</returns>
    internal ShpReader GetShpReader() => this.shpReader;

    /// <summary>
    /// Gets the SHX reader.
    /// </summary>
    /// <returns>The SHX reader.</returns>
    internal ShxReader GetShxReader() => this.shxReader;

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
                this.shpReader.Dispose();
                this.shxReader.Dispose();
                this.dbfReader.Dispose();
                this.prjStream?.Dispose();
            }

            this.disposedValue = true;
        }
    }
}