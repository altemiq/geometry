// -----------------------------------------------------------------------
// <copyright file="ShxReader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

/// <summary>
/// The SHX reader.
/// </summary>
public class ShxReader : IDisposable
{
    private readonly Stream stream;

    private readonly bool leaveOpen;

    private bool disposedValue;

    /// <summary>
    /// Initialises a new instance of the <see cref="ShxReader"/> class.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="ShxReader"/> object is disposed; otherwise, <see langword="false"/>.</param>
    public ShxReader(Stream stream, bool leaveOpen = false)
    {
        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        System.Diagnostics.Contracts.Contract.EndContractBlock();
        (this.stream, this.leaveOpen, this.Header) = (stream, leaveOpen, Header.ReadFrom(stream));

        this.Count = (int)((this.Header.FileLength - Header.Length) / sizeof(int));
    }

    /// <summary>
    /// Gets the header.
    /// </summary>
    public Header Header { get; }

    /// <summary>
    /// Gets the count.
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// Reads the next SHX record.
    /// </summary>
    /// <returns>The SHX record.</returns>
    public ShxRecord? Read()
    {
        var index = (int)((this.stream.Position - (Header.Length * 2)) / (2 * sizeof(int)));
        return index != this.Count ? this.Read(index) : null;
    }

    /// <summary>
    /// Reads the SHX record at the specified index.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>The SHX record.</returns>
    public ShxRecord Read(int index)
    {
        if (index >= this.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        var position = Header.Size + (index * 2 * sizeof(int));
        if (this.stream.Position != position)
        {
            this.stream.Position = position;
        }

        return ReadCore(this.stream);

        static ShxRecord ReadCore(Stream stream)
        {
#if NETSTANDARD2_1_OR_GREATER
            Span<byte> span = stackalloc byte[8];
            _ = stream.Read(span);
#else
            var bytes = new byte[8];
            _ = stream.Read(bytes, 0, bytes.Length);
            ReadOnlySpan<byte> span = bytes;
#endif

            var offset = System.Buffers.Binary.BinaryPrimitives.ReadUInt32BigEndian(span[..4]);
            var contentLength = System.Buffers.Binary.BinaryPrimitives.ReadUInt32BigEndian(span[4..8]);
            return new(offset, contentLength);
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
            if (disposing && !this.leaveOpen)
            {
                this.stream.Dispose();
            }

            this.disposedValue = true;
        }
    }
}