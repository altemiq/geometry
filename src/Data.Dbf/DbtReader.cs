// -----------------------------------------------------------------------
// <copyright file="DbtReader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data.Dbf;

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

        this.Version = buffer[16];
        var blockSize = System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(buffer.AsSpan(20, 2));
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
#if NETSTANDARD2_1_OR_GREATER
        var buffer = System.Buffers.ArrayPool<byte>.Shared.Rent(this.BlockSize);
#else
        var buffer = new byte[this.BlockSize];
#endif

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
#if NETSTANDARD2_1_OR_GREATER
        var chars = System.Buffers.ArrayPool<char>.Shared.Rent(this.BlockSize);
#else
        var chars = new char[this.BlockSize];
#endif

        var stringBuilder = new System.Text.StringBuilder();
        var count = this.encoding.GetChars(buffer, 0, this.BlockSize, chars, 0);
        stringBuilder.Append(chars, 0, count);

        do
        {
            _ = this.stream.Read(buffer, 0, this.BlockSize);

            // check to see if we have the field terminator
            index = IndexOfTerminator(buffer);
            count = this.encoding.GetChars(buffer, 0, index is -1 ? this.BlockSize : index, chars, 0);
            _ = stringBuilder.Append(chars, 0, count);
        }
        while (index is -1);

#if NETSTANDARD2_1_OR_GREATER
        System.Buffers.ArrayPool<char>.Shared.Return(chars);
        System.Buffers.ArrayPool<byte>.Shared.Return(buffer);
#endif

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
        if (this.disposedValue)
        {
            return;
        }

        if (disposing && !this.leaveOpen)
        {
            this.stream.Close();
            this.stream.Dispose();
        }

        this.disposedValue = true;
    }
}