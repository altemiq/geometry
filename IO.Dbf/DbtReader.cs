// -----------------------------------------------------------------------
// <copyright file="DbtReader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Dbf;

/// <summary>
/// The DBT reader.
/// </summary>
public class DbtReader : IDisposable
{
    private readonly Stream stream;
    private readonly System.Text.Encoding encoding;
    private readonly bool leaveOpen;

    private bool disposedValue;

    /// <summary>
    /// Initialises a new instance of the <see cref="DbtReader"/> class.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="encoding">The encoding.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="DbtReader"/> object is disposed; otherwise, <see langword="false"/>.</param>
    public DbtReader(Stream stream, System.Text.Encoding encoding, bool leaveOpen = false)
    {
        this.stream = stream;
        this.encoding = encoding;
        this.leaveOpen = leaveOpen;

        // read the block size
        var buffer = new byte[512];
        _ = this.stream.Read(buffer, 0, buffer.Length);

        ReadOnlySpan<byte> span = buffer;

        this.Version = span[16];
        var blockSize = System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(span[20..22]);
        this.BlockSize = blockSize is 0 ? 512 : blockSize;
    }

    /// <summary>
    /// Gets the version.
    /// </summary>
    public byte Version { get; }

    /// <summary>
    /// Gets the block size.
    /// </summary>
    public int BlockSize { get; }

    /// <inheritdoc cref="System.Data.IDataRecord.GetString(int)" />
    public string GetString(int i)
    {
        var buffer = new byte[this.BlockSize];
        var offset = this.BlockSize * i;

        if (this.stream.CanSeek)
        {
            this.stream.Position = offset;
        }
        else
        {
            if (this.stream.Position > offset)
            {
                throw new InvalidOperationException();
            }

            // move this forward
            while (this.stream.Position < offset)
            {
                var bytesLeft = (int)(offset - this.stream.Position);
                var bytesToRead = Math.Min(buffer.Length, bytesLeft);
                _ = this.stream.Read(buffer, 0, bytesToRead);
            }
        }

        _ = this.stream.Read(buffer, 0, this.BlockSize);

        // check to see if we have the field terminator
        var index = IndexOfTerminator(buffer);
        if (index is not -1)
        {
            return this.encoding.GetString(buffer, 0, index);
        }

        // get the next string
        var stringBuilder = new System.Text.StringBuilder(this.encoding.GetString(buffer, 0, this.BlockSize));

        do
        {
            _ = this.stream.Read(buffer, 0, this.BlockSize);

            // check to see if we have the field terminator
            index = IndexOfTerminator(buffer);
            _ = stringBuilder.Append(this.encoding.GetString(buffer, 0, index is -1 ? this.BlockSize : index));
        }
        while (index is -1);

        return stringBuilder.ToString();

        static int IndexOfTerminator(byte[] buffer)
        {
            const byte Terminator = 0x1a;
            var count = buffer.Length;

            for (var i = 0; i < count; i++)
            {
                if (buffer[i] is Terminator && buffer[i + 1] is Terminator)
                {
                    return i;
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
            if (disposing && !this.leaveOpen)
            {
                this.stream.Close();
                this.stream.Dispose();
            }

            this.disposedValue = true;
        }
    }
}