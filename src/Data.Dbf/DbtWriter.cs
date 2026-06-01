// -----------------------------------------------------------------------
// <copyright file="DbtWriter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data.Dbf;

/// <summary>
/// The DBT writer.
/// </summary>
public class DbtWriter : IDisposable
{
    private readonly bool leaveOpen;

    private readonly System.Text.Encoding encoding;

    private readonly byte[] remainderBuffer = new byte[512];

    private byte[] buffer;

    private bool disposedValue;

    /// <summary>
    /// Initialises a new instance of the <see cref="DbtWriter"/> class.
    /// </summary>
    /// <param name="stream">The DBT stream.</param>
    /// <param name="encoding">The encoding.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="DbtWriter"/> object is disposed; otherwise, <see langword="false"/>.</param>
    public DbtWriter(Stream stream, System.Text.Encoding encoding, bool leaveOpen = false)
    {
        ArgumentNullException.ThrowIfNull(stream);
        this.BaseStream = stream;
        this.encoding = encoding;

        this.NextBlock = 1;

        this.buffer = new byte[512];
        System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(this.buffer.AsSpan(), this.NextBlock);

        this.BaseStream.Write(this.buffer, 0, this.buffer.Length);
        this.leaveOpen = leaveOpen;
    }

    /// <summary>
    /// Gets the next block.
    /// </summary>
    public int NextBlock { get; private set; }

    /// <summary>
    /// Gets the block size.
    /// </summary>
    public int BlockSize { get; } = 512;

    /// <summary>
    /// Gets the underlying stream of the <see cref="DbtWriter"/>.
    /// </summary>
    internal Stream BaseStream { get; }

    /// <inheritdoc cref="BinaryWriter.Write(string)" />
    public void Write(string value)
    {
        // write to get to the next block boundary
        if (this.BaseStream.Length % this.BlockSize is not 0 and var remainder)
        {
            this.BaseStream.Write(this.remainderBuffer, 0, this.BlockSize - (int)remainder);
        }

        var byteCount = this.encoding.GetByteCount(value) + 2;

        if (byteCount > this.buffer.Length)
        {
            var newLength = this.buffer.Length;
            while (byteCount > newLength)
            {
                newLength *= 2;
            }

            Array.Resize(ref this.buffer, newLength);
        }

        var blocks = byteCount / this.BlockSize;
        if (blocks * this.BlockSize < byteCount)
        {
            blocks++;
        }

        _ = this.encoding.GetBytes(value, 0, value.Length, this.buffer, 0);
        this.buffer[byteCount - 1] = 0x1a;
        this.buffer[byteCount - 2] = 0x1a;

        this.BaseStream.Write(this.buffer, 0, byteCount);
        this.IncrementNextBlock(blocks);
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
        if (this.disposedValue)
        {
            return;
        }

        if (disposing && !this.leaveOpen)
        {
            this.BaseStream.Dispose();
        }

        this.disposedValue = true;
    }

    private void IncrementNextBlock(int blocksWritten)
    {
        this.NextBlock += blocksWritten;
        if (!this.BaseStream.CanSeek)
        {
            return;
        }

        var position = this.BaseStream.Position;
        this.BaseStream.Position = 0;

        System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(this.buffer.AsSpan(0, 4), this.NextBlock);
        this.BaseStream.Write(this.buffer, 0, 4);

        this.BaseStream.Position = position;
    }
}