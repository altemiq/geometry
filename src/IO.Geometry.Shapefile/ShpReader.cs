// -----------------------------------------------------------------------
// <copyright file="ShpReader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

/// <summary>
/// The SHP reader.
/// </summary>
public class ShpReader : IDisposable
{
    private readonly Stream stream;

    private readonly bool leaveOpen;

    private bool disposedValue;

    /// <summary>
    /// Initialises a new instance of the <see cref="ShpReader"/> class.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="ShpReader"/> object is disposed; otherwise, <see langword="false"/>.</param>
    public ShpReader(Stream stream, bool leaveOpen = false)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (this.stream, this.leaveOpen, this.Header) = (stream, leaveOpen, Header.ReadFrom(stream));
    }

    /// <summary>
    /// Gets the header.
    /// </summary>
    public Header Header { get; }

    /// <summary>
    /// Reads the next SHP record.
    /// </summary>
    /// <returns>The SHP record.</returns>
    public ShpRecord? Read() => this.stream.Position < this.stream.Length
        ? Read(this.stream)
        : default;

    /// <summary>
    /// Reads the SHP record at the specified index.
    /// </summary>
    /// <param name="record">The index record.</param>
    /// <returns>The SHP record.</returns>
    public ShpRecord Read(ShxRecord record)
    {
        var position = record.Offset * 2;
        if (this.stream.Position != position)
        {
            this.stream.Position = position;
        }

        return Read(this.stream, record.ContentLength);
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
            if (disposing && !this.leaveOpen)
            {
                this.stream.Dispose();
            }

            this.disposedValue = true;
        }
    }

    private static ShpRecord Read(Stream stream, uint contentLength = 0U)
    {
        var header = ShpRecordHeader.Read(stream);

        if (contentLength is 0)
        {
            contentLength = header.ContentLength;
        }

        var bytes = (int)(contentLength * 2);
        if (bytes < 4)
        {
            // must at least specify the shape type.
            throw new ArgumentOutOfRangeException(nameof(contentLength));
        }

        if (stream.Length - stream.Position < bytes)
        {
            throw new EndOfStreamException();
        }

        // Use ArrayPool for better memory efficiency with large records
        var recordData = System.Buffers.ArrayPool<byte>.Shared.Rent(bytes);
        try
        {
            var bytesRead = stream.Read(recordData, 0, bytes);
            if (bytesRead < bytes)
            {
                throw new EndOfStreamException();
            }

            // Create a copy of the data we need to avoid holding onto the pooled array
            var dataCopy = new byte[bytesRead];
            Array.Copy(recordData, dataCopy, bytesRead);
            return new(header, dataCopy);
        }
        finally
        {
            System.Buffers.ArrayPool<byte>.Shared.Return(recordData);
        }
    }
}