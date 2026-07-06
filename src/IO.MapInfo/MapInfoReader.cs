// -----------------------------------------------------------------------
// <copyright file="MapInfoReader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapInfo;

/// <summary>
/// The <see cref="MapInfo"/> reader.
/// </summary>
public class MapInfoReader : IDisposable
{
    private readonly TabReader tabReader;

    private readonly Data.Dbf.DbfReader dbfReader;

    private readonly MapReader? mapReader;

    private readonly IdReader? idReader;

    private bool disposedValue;

    /// <summary>
    /// Initialises a new instance of the <see cref="MapInfoReader"/> class.
    /// </summary>
    /// <param name="path">The path to the file, with or without an extension.</param>
    public MapInfoReader(string path)
        : this(
              GetFileStreamOrThrow(path, Constants.TabExtension),
              GetFileStreamOrThrow(path, Constants.DbfExtension),
              GetFileStream(path, Constants.MapExtension),
              GetFileStream(path, Constants.IdExtension),
              leaveOpen: false)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="MapInfoReader"/> class.
    /// </summary>
    /// <param name="tabStream">The TAB stream.</param>
    /// <param name="dbfStream">The DBF stream.</param>
    /// <param name="mapStream">The MAP stream.</param>
    /// <param name="idStream">The ID stream.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="MapInfoReader"/> object is disposed; otherwise, <see langword="false"/>.</param>
    public MapInfoReader(
        Stream tabStream,
        Stream dbfStream,
        Stream? mapStream,
        Stream? idStream,
        bool leaveOpen)
    {
        this.tabReader = new(tabStream, leaveOpen);
        this.dbfReader = Data.Dbf.DbfReader.Open(dbfStream, leaveOpen);
        if (mapStream is not null)
        {
            this.mapReader = new(mapStream, leaveOpen);
        }

        if (idStream is not null)
        {
            this.idReader = new(idStream, leaveOpen);
        }
    }

    /// <summary>
    /// Gets the description.
    /// </summary>
    public string? Description => this.tabReader.Description;

    /// <summary>
    /// Gets the charset.
    /// </summary>
    public System.Text.Encoding Encoding => this.tabReader.Encoding;

    /// <summary>
    /// Reads the next MapInfo record.
    /// </summary>
    /// <returns>The MapInfo record.</returns>
    public MapInfoRecord? Read()
    {
        if (!TryGetOffset(this.idReader, out var offset))
        {
            return default;
        }

        return this.dbfReader.Read()
            ? new MapInfoRecord(this.tabReader.Fields, GetMapRecord(this.mapReader, offset), this.dbfReader.GetRecord())
            : default;

        static bool TryGetOffset(IdReader? reader, out long offset)
        {
            if (reader is not null)
            {
                if (reader.Read() is { } offsetFromId)
                {
                    offset = offsetFromId;
                    return true;
                }

                offset = default;
                return false;
            }

            offset = default;
            return true;
        }

        static MapRecord? GetMapRecord(MapReader? reader, long offset)
        {
            return reader is not null && offset is not 0L
                ? reader.Read(offset)
                : default;
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
                this.tabReader.Dispose();
                this.dbfReader.Dispose();
                this.mapReader?.Dispose();
                this.idReader?.Dispose();
            }

            this.disposedValue = true;
        }
    }

    private static FileStream? GetFileStream(string? path, string? extension)
    {
        path = Path.ChangeExtension(path, extension);
        return File.Exists(path) ? File.OpenRead(path) : default;
    }

    private static FileStream GetFileStreamOrThrow(string? path, string? extension) => GetFileStream(path, extension) ?? throw new InvalidOperationException();
}