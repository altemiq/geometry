// -----------------------------------------------------------------------
// <copyright file="ShxWriter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

/// <summary>
/// The SHX writer.
/// </summary>
public class ShxWriter : IDisposable
{
    private readonly Stream stream;

    private readonly bool leaveOpen;

    private bool disposedValue;

    private long start;

    /// <summary>
    /// Initialises a new instance of the <see cref="ShxWriter"/> class.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="ShxWriter"/> object is disposed; otherwise, <see langword="false"/>.</param>
    public ShxWriter(Stream stream, bool leaveOpen = false)
    {
        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        System.Diagnostics.Contracts.Contract.EndContractBlock();
        (this.stream, this.leaveOpen) = (stream, leaveOpen);
    }

    /// <summary>
    /// Writes the header.
    /// </summary>
    /// <param name="header">The header.</param>
    public void Write(Header header)
    {
        this.start = this.stream.Position;
        header.CopyTo(this.stream);
    }

    /// <summary>
    /// Writes the record.
    /// </summary>
    /// <param name="record">The record.</param>
    public void Write(ShxRecord record)
    {
        Span<byte> span = stackalloc byte[8];
        System.Buffers.Binary.BinaryPrimitives.WriteUInt32BigEndian(span[..4], record.Offset);
        System.Buffers.Binary.BinaryPrimitives.WriteUInt32BigEndian(span[4..8], record.ContentLength);
        this.stream.Write(span);
    }

    /// <summary>
    /// Updates the header.
    /// </summary>
    /// <param name="extents">The extents.</param>
    public void Update(EnvelopeZM extents) => ShpWriter.Update(this.stream, this.start, extents);

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
}