// -----------------------------------------------------------------------
// <copyright file="IdReader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.MapInfo;

/// <summary>
/// The <c>ID</c> reader.
/// </summary>
public class IdReader : IDisposable
{
    private readonly Stream stream;

    private readonly bool leaveOpen;

    private readonly byte[] data;

    private readonly long dataOffset;

    private long currentOffset;

    private int size;

    private bool disposedValue;

    private int current;

    /// <summary>
    /// Initialises a new instance of the <see cref="IdReader"/> class.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="IdReader"/> object is disposed; otherwise, <see langword="false"/>.</param>
    public IdReader(Stream stream, bool leaveOpen = false)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(stream);
#else
        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }
#endif

        (this.stream, this.leaveOpen) = (stream, leaveOpen);

        // read the first bytes
        this.data = new byte[1024];
        this.dataOffset = this.currentOffset = this.stream.Position;
        this.size = this.stream.Read(this.data, 0, this.data.Length);
        this.current = -sizeof(int);
    }

    /// <summary>
    /// Reads the next file offset.
    /// </summary>
    /// <returns>The file offset.</returns>
    public long? Read()
    {
        var nextOffset = this.currentOffset + this.current + sizeof(int);
        return nextOffset >= this.stream.Length
            ? default(long?)
            : this.Read(nextOffset);
    }

    /// <summary>
    /// Reads the file offset.
    /// </summary>
    /// <param name="offset">The offset in the file.</param>
    /// <returns>The file offset.</returns>
    public long Read(long offset)
    {
        // get the offset to move to
        var offsetToMoveTo = this.dataOffset + offset;

        while (offsetToMoveTo >= (this.currentOffset + this.size))
        {
            // need to move to the next block
            this.currentOffset += this.size;
            this.size = this.stream.Read(this.data, 0, this.data.Length);
        }

        this.current = (int)(offsetToMoveTo - this.currentOffset);
        return System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(this.data.AsSpan(this.current, sizeof(int)));
    }

    /// <summary>
    /// Reads the file offset for the specified ID.
    /// </summary>
    /// <param name="featureId">The feature ID.</param>
    /// <returns>The file offset.</returns>
    public long Read(int featureId) => this.Read((long)((featureId - 1) * sizeof(int)));

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